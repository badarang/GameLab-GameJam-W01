using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    [SerializeField]
    public BlockType type;
    public Vector3Int startGridPos;

    public void SetStartGridPos(Vector3Int startGridPos)
    {
        this.startGridPos = startGridPos;
    }
}
