using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSelector : MonoBehaviour
{
    public GameObject selectGrid;
    public GameObject unableGrid;

    public Tilemap selectionTilemap;

    private Vector3Int hoverPos;
    private Vector3Int prevHoverPos;
    private BlockType selectedBlock;
    private Vector2Int selectedBlockSize;

    private List<GameObject> instantiatedGrids;

    public readonly Dictionary<BlockType, Vector2Int> BLOCK_DATA = new Dictionary<BlockType, Vector2Int>()
    {
        { BlockType.NONE, new Vector2Int(0, 0) },
        { BlockType.BlockA, new Vector2Int(2, 1) }
    };

    // Start is called before the first frame update
    void Start()
    {
        instantiatedGrids = new List<GameObject>();

        selectedBlock = BlockType.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedBlock != BlockType.NONE)
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
                Vector3 startPos = new Vector3
                    (targetPos.x - (selectionTilemap.cellSize.x * (selectedBlockSize.x / 2)),
                    targetPos.y - (selectionTilemap.cellSize.y * (selectedBlockSize.y / 2)),
                    0);

                for (int i = 0; i < selectedBlockSize.x; i++)
                {
                    for (int j = 0; j < selectedBlockSize.y; j++)
                    {
                        GameObject g = Instantiate(selectGrid);

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
        }

        Debug.Log(hoverPos);
    }

    public void SetBlock(int type)
    {
        selectedBlock = (BlockType) type;
        selectedBlockSize = BLOCK_DATA[(BlockType) type];
    }
}

public enum BlockType
{
    NONE,
    BlockA,
    BlockB
}

public struct BlockInfo
{
    Vector2Int size;
    
}