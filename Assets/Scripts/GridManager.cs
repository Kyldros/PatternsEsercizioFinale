using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] Tilemap grid;
    [SerializeField] List<TileData> tileDatas;
    [SerializeField] public TileBase terrainTile;
    [SerializeField] public TileBase doorTile;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private static GridManager _instance;
    public static GridManager Instance
    {
        get => _instance;
        private set => _instance = value;  
    }

    private void Start()
    {
        _instance = this;
    }

    private void Awake()
    {
        if (grid == null)
        {
            grid = GetComponentInChildren<GridMap>().GetComponent<Tilemap>();
            if (grid == null)
                Debug.LogError("GridMap Not Found!");
        }

        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (TileData data in tileDatas)
        {
            foreach(TileBase tile in data.GetTiles())
            {
                dataFromTiles.Add(tile, data);
            }
        }
    }

    public Tilemap GetTilemap()
    {
        return grid;
    }

    public TileData GetTileData(Vector2 worldPosition)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        TileBase tile = grid.GetTile(gridPosition);

        return dataFromTiles[tile];
    }

    public bool TileIsWalkable(Vector2 position)
    {
        bool checkTile = GetTileData(position).GetWalkable();

        return checkTile;
    }

    public void ChangeTile(Vector2 worldPosition, TileBase tile)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        grid.SetTile(gridPosition, tile);
    }

}
