using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    GameObject water;
    [SerializeField]
    GameObject lava;
    [SerializeField]
    GameObject cloud;

    public void Genere()
    {
        if (Map.type == MapType.Earth || Map.type == MapType.IceDesert)
        {
            water.SetActive(true);
            lava.SetActive(false);
            cloud.SetActive(false);
        }
        else if (Map.type == MapType.LavaDesert)
        {
            water.SetActive(false);
            lava.SetActive(true);
            cloud.SetActive(false);
        }
        else if (Map.type == MapType.Sky)
        {
            water.SetActive(false);
            lava.SetActive(false);
            cloud.SetActive(true);
        }
    }
}