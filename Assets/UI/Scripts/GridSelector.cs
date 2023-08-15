using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridSelector : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> btnImgPrefabs;

    [SerializeField]
    public List<GameObject> objPrefabs;

    [SerializeField]
    public List<BlockType> blockTypes;

    public GameObject selectGridPrefab;
    public GameObject unableGridPrefab;

    public GameObject selectButtonPrefab;
    public GameObject deleteButtonImagePrefab;

    public Tilemap selectionTilemap;
    public RectTransform selectionButtonPanel;

    private Vector3Int hoverPos;
    private Vector3Int prevHoverPos;
    private BlockType selectedBlock;
    private Vector2Int selectedBlockSize;

    private List<GameObject> instantiatedSelectGrids;
    private List<GameObject> instantiatedUnableGrids;
    private List<GameObject> instantiatedBlockObjs;

    [SerializeField]
    public readonly Dictionary<BlockType, Vector2Int> blockSizeData = new Dictionary<BlockType, Vector2Int>()
    {
        { BlockType.OIL_PRESS, new Vector2Int(1, 1) },
        { BlockType.SPINE_SMALL, new Vector2Int(1, 1) },
        { BlockType.SPINE_BIG, new Vector2Int(3, 1) },
        { BlockType.NONE, new Vector2Int(0, 0) },
        { BlockType.DELETE, new Vector2Int(0, 0) },
    };

    private Dictionary<BlockType, GameObject> blockObjectData = new Dictionary<BlockType, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialize selection info
        // Also instantiate buttons for selection UI
        if (objPrefabs.Count != blockTypes.Count || objPrefabs.Count != btnImgPrefabs.Count)
        {
            Debug.LogError("Number of block object prefabs(" + objPrefabs.Count + ") is not match with block types(" + blockTypes.Count + ")");
        }

        for (int i = 0; i < objPrefabs.Count; i++)
        {
            blockObjectData.Add(blockTypes[i], objPrefabs[i]);

            AddButton(blockTypes[i], btnImgPrefabs[i]);
        }

        AddButton(BlockType.DELETE, deleteButtonImagePrefab);

        // Initialize private values
        instantiatedSelectGrids = new List<GameObject>();
        instantiatedUnableGrids = new List<GameObject>();
        instantiatedBlockObjs = new List<GameObject>();

        selectedBlock = BlockType.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedBlock != BlockType.NONE && selectedBlock != BlockType.DELETE)
        {
            hoverPos = selectionTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (hoverPos != prevHoverPos)
            {
                foreach (GameObject g in instantiatedSelectGrids)
                {
                    Destroy(g);
                }

                instantiatedSelectGrids = new List<GameObject>();

                Vector3 targetPos = selectionTilemap.GetCellCenterWorld(hoverPos);
                Vector3 startPos = GetStartGridPos(targetPos);

                for (int i = 0; i < selectedBlockSize.x; i++)
                {
                    for (int j = 0; j < selectedBlockSize.y; j++)
                    {
                        GameObject g = Instantiate(selectGridPrefab);

                        instantiatedSelectGrids.Add(g);
                        g.GetComponent<Transform>().position =
                            new Vector3(
                                startPos.x + selectionTilemap.cellSize.x * i,
                                startPos.y + selectionTilemap.cellSize.y * j,
                                startPos.z
                            );
                    }
                }

                prevHoverPos = hoverPos;
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 targetPos = selectionTilemap.GetCellCenterWorld(hoverPos);
                Vector3 startPos = GetStartGridPos(targetPos);
                Vector3 endPos = GetEndGridPos(startPos);

                Debug.Log("" + targetPos + " " + startPos + " " + endPos);

                PlaceBlockObj(selectedBlock, (startPos + endPos) / 2, startPos);
            }
        }
        else
        {
            if (instantiatedSelectGrids.Count != 0)
            {
                foreach (GameObject g in instantiatedSelectGrids)
                {
                    Destroy(g);
                }

                instantiatedSelectGrids = new List<GameObject>();
            }
        }
    }

    public void SetBlock(BlockType type)
    {
        selectedBlock = type;
        selectedBlockSize = blockSizeData[type];

        foreach (GameObject g in instantiatedBlockObjs)
        {
            DeletableBlock deletable = g.GetComponent<DeletableBlock>();
            if (deletable != null)
            {
                deletable.SetDeletable(type == BlockType.DELETE);
            }
        }
    }

    private void AddButton(BlockType type, GameObject blockImgObj)
    {
        GameObject b = Instantiate(selectButtonPrefab);
        b.GetComponent<RectTransform>().SetParent(selectionButtonPanel);
        b.GetComponent<Button>().onClick.AddListener(() => {
            SetBlock(type);
        });

        GameObject bimg = Instantiate(blockImgObj);
        bimg.GetComponent<RectTransform>().SetParent(b.GetComponent<RectTransform>());
    }

    private void PlaceBlockObj(BlockType type, Vector3 position, Vector3 startPos)
    {
        GameObject g = Instantiate(blockObjectData[type]);
        g.GetComponent<Transform>().position = position;

        DeletableBlock deletable = g.GetComponent<DeletableBlock>();
        if (deletable != null)
        {
            deletable.SetOnDeleteCallback((target) => {
                instantiatedBlockObjs.Remove(target);
                Destroy(target);

                if (instantiatedUnableGrids.Count > 0)
                {
                    Debug.Log("delete");
                    foreach (GameObject g in instantiatedUnableGrids)
                    {
                        Destroy(g);
                    }

                    instantiatedUnableGrids = new List<GameObject>();
                }
            });

            deletable.SetOnHoverCallback((target) => {
                if (instantiatedBlockObjs.Contains(target))
                {
                    if (instantiatedUnableGrids.Count > 0)
                    {
                        foreach (GameObject g in instantiatedUnableGrids)
                        {
                            Destroy(g);
                        }

                        instantiatedUnableGrids = new List<GameObject>();
                    }

                    for (int i = 0; i < blockSizeData[type].x; i++)
                    {
                        for (int j = 0; j < blockSizeData[type].y; j++)
                        {
                            GameObject g = Instantiate(unableGridPrefab);

                            instantiatedUnableGrids.Add(g);
                            g.GetComponent<Transform>().position =
                                new Vector3(
                                    startPos.x + selectionTilemap.cellSize.x * i,
                                    startPos.y + selectionTilemap.cellSize.y * j,
                                    startPos.z
                                );
                        }
                    }
                }
            });

            deletable.SetOnExitHoverCallback((target) =>
            {
                if (instantiatedUnableGrids.Count > 0)
                {
                    foreach (GameObject g in instantiatedUnableGrids)
                    {
                        Destroy(g);
                    }

                    instantiatedUnableGrids = new List<GameObject>();
                }
            });
        }

        instantiatedBlockObjs.Add(g);
    }

    private Vector3 GetStartGridPos(Vector3 targetPos)
    {
        Vector3 startPos = new Vector3
            (targetPos.x - (selectionTilemap.cellSize.x * (selectedBlockSize.x / 2)),
            targetPos.y - (selectionTilemap.cellSize.y * (selectedBlockSize.y / 2)),
            0);

        return startPos;
    }

    private Vector3 GetEndGridPos(Vector3 startPos)
    {
        Vector3 endPos = new Vector3
            (startPos.x + (selectionTilemap.cellSize.x * (selectedBlockSize.x - 1)),
            startPos.y + (selectionTilemap.cellSize.y * (selectedBlockSize.y - 1)),
            0);

        return endPos;
    }
}

public enum BlockType
{
    NONE,
    DELETE,
    OIL_PRESS,
    SPINE_SMALL,
    SPINE_BIG
}

public struct BlockInfo
{
    Vector2Int size;
    GameObject targetObjPrefab;

    public BlockInfo(Vector2Int size, GameObject targetObjPrefab)
    {
        this.size = size;
        this.targetObjPrefab = targetObjPrefab;
    }
}