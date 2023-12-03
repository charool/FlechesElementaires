using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    float reloadTime;
    [SerializeField]
    Transform SpawnPoint;
    bool isReloading;

    public void Fire(PlayerSelection __p)
    {
        if (!isReloading) {
            GameObject arrowPrefab = __p.FireCurrentArrowType();

            if (arrowPrefab != null) {
                GameObject currentArrow = Instantiate(arrowPrefab, SpawnPoint);
                currentArrow.transform.localPosition = Vector3.zero;
                currentArrow.GetComponent<Arrow>().Shoot(SpawnPoint);
                AudioManager.Instance.Play("Effects/arrow");
            }
        }
    }
}
