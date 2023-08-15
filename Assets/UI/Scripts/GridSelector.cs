using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridSelector : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> objPrefabs;

    [SerializeField]
    public List<BlockType> blockTypes;

    public GameObject selectGridPrefab;
    public GameObject unableGridPrefab;

    public GameObject selectButtonPrefab;

    public Tilemap selectionTilemap;
    public RectTransform selectionButtonPanel;

    private Vector3Int hoverPos;
    private Vector3Int prevHoverPos;
    private BlockType selectedBlock;
    private Vector2Int selectedBlockSize;

    private List<GameObject> instantiatedGrids;
    private List<GameObject> instantiatedBlockObjs;

    [SerializeField]
    public readonly Dictionary<BlockType, Vector2Int> blockSizeData = new Dictionary<BlockType, Vector2Int>()
    {
        { BlockType.BlockA, new Vector2Int(2, 1) },
        { BlockType.BlockB, new Vector2Int(2, 3) },
        { BlockType.NONE, new Vector2Int(0, 0) },
        { BlockType.DELETE, new Vector2Int(0, 0) },
    };

    private Dictionary<BlockType, GameObject> blockObjectData = new Dictionary<BlockType, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialize selection info
        // Also instantiate buttons for selection UI
        if (objPrefabs.Count != blockTypes.Count)
        {
            Debug.LogError("Number of block object prefabs(" + objPrefabs.Count + ") is not match with block types(" + blockTypes.Count + ")");
        }

        for (int i = 0; i < objPrefabs.Count; i++)
        {
            blockObjectData.Add(blockTypes[i], objPrefabs[i]);

            AddButton(blockTypes[i]);
        }

        AddButton(BlockType.DELETE);

        // Initialize private values
        instantiatedGrids = new List<GameObject>();
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
                foreach (GameObject g in instantiatedGrids)
                {
                    Destroy(g);
                }

                instantiatedGrids = new List<GameObject>();

                Vector3 targetPos = selectionTilemap.GetCellCenterWorld(hoverPos);
                Vector3 startPos = GetStartGridPos(targetPos);

                for (int i = 0; i < selectedBlockSize.x; i++)
                {
                    for (int j = 0; j < selectedBlockSize.y; j++)
                    {
                        GameObject g = Instantiate(selectGridPrefab);

                        instantiatedGrids.Add(g);
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

                PlaceBlockObj(selectedBlock, (startPos + endPos) / 2);
            }
        }
        else
        {
            if (instantiatedGrids.Count != 0)
            {
                foreach (GameObject g in instantiatedGrids)
                {
                    Destroy(g);
                }

                instantiatedGrids = new List<GameObject>();
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

    private void AddButton(BlockType type)
    {
        GameObject b = Instantiate(selectButtonPrefab);
        b.GetComponent<RectTransform>().parent = selectionButtonPanel;
        b.GetComponent<Button>().onClick.AddListener(() => {
            SetBlock(type);
        });
    }

    private void PlaceBlockObj(BlockType type, Vector3 position)
    {
        GameObject g = Instantiate(blockObjectData[type]);
        g.GetComponent<Transform>().position = position;

        DeletableBlock deletable = g.GetComponent<DeletableBlock>();
        if (deletable != null)
        {
            deletable.SetOnDeleteCallback((target) => {
                instantiatedBlockObjs.Remove(target);
                Destroy(target);
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
    BlockA,
    BlockB
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