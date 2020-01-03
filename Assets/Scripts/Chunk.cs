#pragma warning disable 0649
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    private const int ChunkWidth = 16, ChunkHeight = 12;
    private const int HalfChunkWidth = 8, HalfChunkHeight = 6;
    
    public Transform startPoint, endPoint;
    
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private RuleTile ruleTile;

    public Tilemap TileMap => tileMap;
    
    public void ExtendFromLeftNeighbor(Tilemap neighbor)
    {
        for (int y = 0; y < ChunkHeight; y++)
        {
            if (neighbor.HasTile(new Vector3Int(HalfChunkWidth, y - HalfChunkHeight, 0)))
            {
                tileMap.SetTile(new Vector3Int(-HalfChunkWidth, y - HalfChunkHeight, 0), ruleTile);
            }
        }
    }
    
    public void ExtendFromRightNeighbor(Tilemap neighbor)
    {
        for (int y = 0; y < ChunkHeight; y++)
        {
            if (neighbor.HasTile(new Vector3Int(-HalfChunkWidth, y - HalfChunkHeight, 0)))
            {
                tileMap.SetTile(new Vector3Int(HalfChunkWidth, y - HalfChunkHeight, 0), ruleTile);
            }
        }
    }
}
