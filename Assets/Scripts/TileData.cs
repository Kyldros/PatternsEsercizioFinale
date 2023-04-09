using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    [SerializeField] bool walkable = false;
    [SerializeField] TileBase[] tiles;

    public bool GetWalkable()
    {
        return walkable;
    }

    public TileBase[] GetTiles()
    {
        return tiles;
    }

}
