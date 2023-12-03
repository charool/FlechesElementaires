using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debut : MonoBehaviour
{
    private void OnTriggerEnter(Collider __other)
    {
        if (__other.CompareTag("Player"))
        {
            MessageManager.Instance.Broadcast("Statue");
        }
    }
    private void OnTriggerExit(Collider __other)
    {
        if (__other.CompareTag("Player"))
        {
            MessageManager.Instance.Shown = false;
        }
    }
}
