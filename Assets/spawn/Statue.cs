using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    [SerializeField]
    GameObject StatueMenu;
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) { return; }
        StatueMenu.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        StatueMenu.SetActive(false);
    }
}
