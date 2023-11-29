using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    public static int maxSize = 100;
    [SerializeField]
    public float waterLevel = 1f;
    [SerializeField]
    public float deepWaterLevel = 1.1f;
    [SerializeField]
    GameObject chunkPrefab;

    public static int seed;
    public static MapType type = MapType.Spawn;
    public static Map instance;

    private GameObject[,] chunks;

    [SerializeField]
    private int chunkRendererDistance;

    private void Start()
    {
        instance = GetComponent<Map>();
    }

    public Chunk GetChunk(int x,int z)
    {
        if (0 <= x && 0 <= z && x < chunks.GetLength(1) && z < chunks.GetLength(0))
        {
            return chunks[x, z].GetComponent<Chunk>();
        }
        return null;
    }
    public Chunk GetChunk(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / Chunk.nbCaseX);
        int z = Mathf.FloorToInt(pos.z / Chunk.nbCaseX);
        return GetChunk(x,z);
    }

    public void UpdateChunks(Chunk previous, Chunk next)
    {
        Vector2Int id = previous.indexXZ;

        chunks[id.x, id.y].GetComponent<Chunk>().Unactive();
        for (int i = 0; i < chunkRendererDistance+1; i++)
        {
            for (int j = 0; j < chunkRendererDistance+1; j++)
            {
                if(i == 0 && j ==0) { continue; }
                Chunk chunk = GetChunk(id.x + i, id.y + j);
                if(chunk != null && chunk.gameObject.activeSelf == true) { chunk.GetComponent<Chunk>().Unactive(); }
                chunk = GetChunk(id.x - i, id.y + j);
                if (chunk != null && chunk.gameObject.activeSelf == true) { chunk.GetComponent<Chunk>().Unactive(); }
                chunk = GetChunk(id.x + i, id.y - j);
                if (chunk != null && chunk.gameObject.activeSelf == true) { chunk.GetComponent<Chunk>().Unactive(); }
                chunk = GetChunk(id.x - i, id.y - j);
                if (chunk != null && chunk.gameObject.activeSelf == true) { chunk.GetComponent<Chunk>().Unactive(); }
            }
        }
        id = next.indexXZ;
        chunks[id.x, id.y].SetActive(true);
        for (int i = 0; i < chunkRendererDistance+1; i++)
        {
            for (int j = 0; j < chunkRendererDistance+1; j++)
            {
                if (i == 0 && j == 0) { continue; }
                Chunk chunk = GetChunk(id.x + i, id.y + j);
                if (chunk != null) { chunk.GetComponent<Chunk>().Active();}
                chunk = GetChunk(id.x - i, id.y + j);
                if (chunk != null) { chunk.GetComponent<Chunk>().Active(); }
                chunk = GetChunk(id.x + i, id.y - j);
                if (chunk != null) { chunk.GetComponent<Chunk>().Active(); }
                chunk = GetChunk(id.x - i, id.y - j);
                if (chunk != null) { chunk.GetComponent<Chunk>().Active(); }
            }
        }
    }

    public void CreateMap()
    {
        chunks = new GameObject[maxSize,maxSize];
        Vector3 playerPos = new Vector3((float)maxSize * (float)Chunk.nbCaseX / 2f, 0f, (float)maxSize * (float)Chunk.nbCaseX / 2f);
        for (int i = 0; i < chunks.GetLength(1); i++)
        {
            for (int j = 0; j < chunks.GetLength(0); j++)
            {
                GameObject chunk = Instantiate(chunkPrefab);
                chunk.name = i.ToString() + " " + j.ToString();
                chunk.transform.position = 
                    new Vector3((float)j * (float)Chunk.nbCaseX,0f, (float)i * (float)Chunk.nbCaseX);
                chunk.transform.parent = transform;
                chunks[j, i] = chunk;
                Chunk c = chunk.GetComponent<Chunk>();
                c.ID = j + Map.maxSize * i;
                c.indexXZ = new Vector2Int(j, i);
                chunk.SetActive(false);
                if (i >= maxSize/2 - chunkRendererDistance && j >= maxSize / 2 - chunkRendererDistance &&
                    i <= maxSize / 2 + chunkRendererDistance && j <= maxSize / 2 + chunkRendererDistance)
                {
                    c.Active();
                }
            }
        }
        Player.instance.Init(playerPos);
    }

    public void Destroy()
    {
        foreach (GameObject chunk in chunks) 
        { 
            Destroy(chunk);
        }
        chunks = new GameObject[maxSize, maxSize];
    }
}
public enum MapType
{
    Spawn,Map1
}
