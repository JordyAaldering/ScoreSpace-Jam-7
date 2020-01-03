﻿using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private float chunkSpeed = 1f;
    [SerializeField] private float minX = -10f;

    [SerializeField] private Chunk[] chunks = new Chunk[4];
    [SerializeField] private Chunk[] chunkPrefabs = new Chunk[0];

    private void Awake()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            if (chunks[i] != null)
            {
                if (i > 0)
                    chunks[i - 1].ExtendFromRightNeighbor(chunks[i].WallTileMap);
                continue;
            }

            Vector2 pos = i == 0 ? Vector2.zero : (Vector2) chunks[i - 1].endPoint.position - (Vector2) chunks[i - 1].startPoint.localPosition;
            Chunk chunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length - 1)], pos, Quaternion.identity, transform);
            chunk.gameObject.name = $"Chunk ({i + 1})";
            chunks[i] = chunk;
            
            chunks[i - 1].ExtendFromRightNeighbor(chunk.WallTileMap);
        }
    }

    private void Update()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            Chunk chunk = chunks[i];
            chunk.transform.position += chunkSpeed * Time.deltaTime * Vector3.left;
            
            if (chunks[i].endPoint.position.x < minX)
                SetChunk(i);
        }
    }

    private void SetChunk(int index)
    {
        int prev = index - 1;
        if (prev < 0) prev += chunks.Length;
        
        Destroy(chunks[index].gameObject);

        Vector2 pos = (Vector2) chunks[prev].endPoint.position - (Vector2) chunks[prev].startPoint.localPosition;
        Chunk chunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length - 1)], pos, Quaternion.identity, transform);
        chunk.gameObject.name = $"Chunk ({index + 1})";
        chunks[index] = chunk;
        
        chunks[prev].ExtendFromRightNeighbor(chunk.WallTileMap);
    }
}
