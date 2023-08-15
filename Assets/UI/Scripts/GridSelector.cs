using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GridSelector : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> btnImgPrefabs;

    [SerializeField]
    public List<GameObject> allObjPrefabs;

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
    private BlockInfo selectedBlockInfo;

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

    private Dictionary<BlockType, GameObject> blockObjectData;
    private Dictionary<BlockType, GameObject> blockBtnImgData;

    private MapLoader mapLoader;
    private MapData curMapData;

    // Start is called before the first frame update
    void Start()
    {
        blockObjectData = new Dictionary<BlockType, GameObject>();
        blockBtnImgData = new Dictionary<BlockType, GameObject>();

        // Initialize all blocks info
        if (allObjPrefabs.Count != blockTypes.Count || allObjPrefabs.Count != btnImgPrefabs.Count)
        {
            Debug.LogError("Number of block object prefabs(" + allObjPrefabs.Count + ") is not match with block types(" + blockTypes.Count + ")");
        }

        for (int i = 0; i < allObjPrefabs.Count; i++)
        {
            blockObjectData.Add(blockTypes[i], allObjPrefabs[i]);
            blockBtnImgData.Add(blockTypes[i], btnImgPrefabs[i]);
        }

        mapLoader = GetComponent<MapLoader>();
        curMapData = mapLoader.LoadMap(0);

        foreach (BlockInfo b in curMapData.installableBlocks)
        {
            AddButton(b);
        }

        BlockInfo deleteButtonInfo = new BlockInfo();
        deleteButtonInfo.type = BlockType.DELETE;
        AddButton(deleteButtonInfo);

        // Initialize private values
        instantiatedSelectGrids = new List<GameObject>();
        instantiatedUnableGrids = new List<GameObject>();
        instantiatedBlockObjs = new List<GameObject>();

        foreach (BlockInfo b in curMapData.defaultBlocks)
        {
            PlaceBlockObj(b);
        }

        BlockInfo nonBlockInfo = new BlockInfo();
        nonBlockInfo.type = BlockType.NONE;
        selectedBlockInfo = nonBlockInfo;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedBlockInfo.type != BlockType.NONE && selectedBlockInfo.type != BlockType.DELETE)
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
                Vector2Int blockSize = selectedBlockInfo.isHalfRotated ? new Vector2Int(blockSizeData[selectedBlockInfo.type].y, blockSizeData[selectedBlockInfo.type].x) : blockSizeData[selectedBlockInfo.type];
                Vector3 startPos = GetStartGridPos(targetPos, blockSize);

                for (int i = 0; i < blockSize.x; i++)
                {
                    for (int j = 0; j < blockSize.y; j++)
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
                Vector2Int blockSize = selectedBlockInfo.isHalfRotated ? new Vector2Int(blockSizeData[selectedBlockInfo.type].y, blockSizeData[selectedBlockInfo.type].x) : blockSizeData[selectedBlockInfo.type];
                selectedBlockInfo.startGridPos = selectionTilemap.WorldToCell(GetStartGridPos(targetPos, blockSize));
                PlaceBlockObj(selectedBlockInfo);
            }
        }
        else
        {
            if (instantiatedSelectGrids == null)
            {
                instantiatedSelectGrids = new List<GameObject>();
            }

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

    public void SetBlock(BlockInfo blockInfo)
    {
        selectedBlockInfo = blockInfo;

        foreach (GameObject g in instantiatedBlockObjs)
        {
            IDeletable deletable = g.GetComponent<IDeletable>();
            if (deletable != null)
            {
                deletable.SetDeletable(blockInfo.type == BlockType.DELETE);
            }
        }
    }

    private void AddButton(BlockInfo blockInfo)
    {
        GameObject b = Instantiate(selectButtonPrefab);
        b.GetComponent<RectTransform>().SetParent(selectionButtonPanel);
        BlockInfo blockInfo1_ = blockInfo;
        b.GetComponent<Button>().onClick.AddListener(() => {
            SetBlock(blockInfo1_);
        });

        if (blockInfo.type != BlockType.DELETE)
        {
            GameObject bimg = Instantiate(blockBtnImgData[blockInfo.type]);
            bimg.GetComponent<RectTransform>().SetParent(b.GetComponent<RectTransform>());
            bimg.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, blockInfo.rotation));
        } else
        {
            GameObject bimg = Instantiate(deleteButtonImagePrefab);
            bimg.GetComponent<RectTransform>().SetParent(b.GetComponent<RectTransform>());
        }
    }

    private void PlaceBlockObj(BlockInfo blockInfo)
    {
        GameObject g = Instantiate(blockObjectData[blockInfo.type]);
        g.GetComponent<BlockBase>().SetBlockRotation(blockInfo.isHalfRotated, blockInfo.rotation);
        g.GetComponent<BlockBase>().SetStartGridPos(blockInfo.startGridPos);
        g.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, blockInfo.rotation));

        Vector2Int blockSize = blockInfo.isHalfRotated ? new Vector2Int(blockSizeData[blockInfo.type].y, blockSizeData[blockInfo.type].x) : blockSizeData[blockInfo.type]; 
        Vector3 startPos = selectionTilemap.GetCellCenterWorld(blockInfo.startGridPos);
        Vector3 endPos = GetEndGridPos(startPos, blockSize);
        Vector3 position = (startPos + endPos) / 2;

        g.GetComponent<Transform>().position = position;

        IDeletable deletable = g.GetComponent<IDeletable>();
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
                Debug.Log("Asdf");
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

                    for (int i = 0; i < blockSize.x; i++)
                    {
                        for (int j = 0; j < blockSize.y; j++)
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

    private Vector3 GetStartGridPos(Vector3 targetPos, Vector2Int blockSize)
    {
        Vector3 startPos = new Vector3
            (targetPos.x - (selectionTilemap.cellSize.x * (blockSize.x / 2)),
            targetPos.y - (selectionTilemap.cellSize.y * (blockSize.y / 2)),
            0);

        return startPos;
    }

    private Vector3 GetEndGridPos(Vector3 startPos, Vector2Int blockSize)
    {
        Vector3 endPos = new Vector3
            (startPos.x + (selectionTilemap.cellSize.x * (blockSize.x - 1)),
            startPos.y + (selectionTilemap.cellSize.y * (blockSize.y - 1)),
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

[Serializable]
public struct BlockInfo
{
    public BlockType type;
    public bool isHalfRotated;
    public int rotation;
    public Vector3Int startGridPos;
}