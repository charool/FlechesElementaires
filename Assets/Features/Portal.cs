using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private MapType type;
    [SerializeField]
    private ArrowType rewardType;
    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag == "Player")
        {
            Player.instance.UnableDeplacement();
            Map.type = type;
            Map.rewardtype = rewardType;
            Map.instance.CreateMap();
            spawn.SetActive(false);
        }
    }
}
