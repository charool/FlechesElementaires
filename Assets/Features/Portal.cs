using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private MapType type;
    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag == "Player")
        {
            Player.instance.UnableDeplacement();
            Map.type = type;
            Map.instance.CreateMap();
            spawn.SetActive(false);
        }
    }
}
