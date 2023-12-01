using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    float reloadTime;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    Transform SpawnPoint;
    GameObject currentArrow;
    bool isReloading;

    public void Fire()
    {
        print("fire");
        if (isReloading) return;
        //Vector3 force = SpawnPoint.TransformDirection(Vector3.forward * firePower);
        currentArrow = Instantiate(arrowPrefab, SpawnPoint);
        currentArrow.transform.localPosition = Vector3.zero;
        currentArrow.GetComponent<Arrow>().Shoot(SpawnPoint);
        currentArrow = null;
        //Reload();
    }
}
