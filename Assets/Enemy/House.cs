using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    bool hasGive = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasGive) { return; }
        if (other.CompareTag("Player"))
        {
            hasGive = true;
            other.GetComponent<PlayerSelection>().AddNumberOfArrow(Map.rewardtype, (byte)Random.Range(1, 4));
            other.GetComponent<PlayerSelection>().AddNumberOfArrow(ArrowType.Clasique, (byte)Random.Range(5, 10));
            if(Map.type == MapType.Sky)
            {

                other.GetComponent<PlayerSelection>().AddNumberOfArrow(ArrowType.Wind, (byte)Random.Range(2, 5));
            }
        }
    }
}
