#pragma warning disable 0649
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    [SerializeField] private int chunkWidth = 16, chunkHeight = 12;
    private int halfChunkWidth, halfChunkHeight;
    
    public Transform startPoint, endPoint;
    
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private RuleTile wallRuleTile;

    public Tilemap WallTileMap => wallTileMap;

    private void Awake()
    {
        halfChunkWidth = chunkWidth / 2;
        halfChunkHeight = chunkHeight / 2;
    }

    public void ExtendFromLeftNeighbor(Tilemap neighbor)
    {
        for (int y = 0; y < chunkHeight; y++)
        {
            if (neighbor.HasTile(new Vector3Int(halfChunkWidth, y - halfChunkHeight, 0)))
                wallTileMap.SetTile(new Vector3Int(-halfChunkWidth, y - halfChunkHeight, 0), wallRuleTile);
        }
    }
    
    public void ExtendFromRightNeighbor(Tilemap neighbor)
    {
        for (int y = 0; y < chunkHeight; y++)
        {
            if (neighbor.HasTile(new Vector3Int(-halfChunkWidth, y - halfChunkHeight, 0)))
                wallTileMap.SetTile(new Vector3Int(halfChunkWidth, y - halfChunkHeight, 0), wallRuleTile);
        }
    }
}
