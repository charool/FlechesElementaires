using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Chunk currentChunk;

    public static Player instance;

    public static bool asDefeatEarth = false;
    public static bool asDefeatIce = false;
    public static bool asDefeatElec = false;
    public static bool asDefeatLava = false;

    private bool activeDeplacement = false;
    private bool activeDeplacementCheck = false;

    private void Awake()
    {
        instance = this;
    }
    public void Init(Vector3 pos)
    {
        transform.position = pos + new Vector3(0f,5f,0f);
        currentChunk = Map.instance.GetChunk(pos);
    }

    public void UnableDeplacement()
    {
        activeDeplacement = false;
        activeDeplacementCheck = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<Controller>().enabled = false;
    }

    private void Update()
    {
        if (!activeDeplacement) 
        {
            if (!activeDeplacementCheck) { activeDeplacementCheck = true; }
            else 
            { 
                GetComponent<CharacterController>().enabled = true;
                GetComponent<Controller>().enabled = true;
                activeDeplacement = true; 
            }
        }
    }

    void LateUpdate()
    {
        if(Map.type == MapType.Spawn) { return; }
        Vector3 pos = transform.position;
        Chunk chunk = Map.instance.GetChunk(pos);

        if (chunk.ID != currentChunk.ID) 
        {
            Map.instance.UpdateChunks(currentChunk, chunk);
            currentChunk = chunk;
        }
    }
}
