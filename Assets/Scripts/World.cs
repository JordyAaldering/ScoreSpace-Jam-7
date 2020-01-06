using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    [SerializeField] private float chunkSpeed = 1f;
    [SerializeField] private float chunkSpeedIncrement = 0.1f;

    [SerializeField] private Chunk[] chunks = new Chunk[4];
    [SerializeField] private Chunk[] chunkPrefabs = new Chunk[0];

    private Camera cam;
    private float lastAspect;
    private float minX = -10f;
    
    private void Awake()
    {
        cam = Camera.main;
        ResetMinX();

        FindObjectOfType<PlayerManager>().OnGameOverAction += () => chunkSpeedIncrement = 0f;
        
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
        chunkSpeed += Time.deltaTime * chunkSpeedIncrement;

        Vector3 dist = chunkSpeed * Time.deltaTime * Vector3.left;
        for (int i = 0; i < chunks.Length; i++)
        {
            Chunk chunk = chunks[i];
            chunk.transform.position += dist;
            
            if (chunks[i].endPoint.position.x < minX)
                SetChunk(i);
        }

        float nextAspect = cam.aspect;
        if (Math.Abs(nextAspect - lastAspect) > 0.1f)
        {
            lastAspect = nextAspect;
            ResetMinX();
        }
    }

    private void ResetMinX()
    {
        float halfHeight = cam.orthographicSize;
        minX = -cam.aspect * halfHeight - 1f;
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
