using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    bool hasGive = false;
    public ArrowType type;
    [SerializeField]
    List<Enemy> toSpawn;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Enemy[] elem;

    List<Enemy> enemys = new List<Enemy>();
    bool hasSpawn = false;

    public void Spawn()
    {
        if(hasSpawn) { return; }
        if (Random.value < probaElem)
        {
            Enemy g = Instantiate(elem[(int)Map.type]);
            g.transform.position = spawnPoint.position;
            enemys.Add(g);
            g.SetTower(this);
            Map.enemies.Add(g.gameObject);
            return;
        }
        foreach (Enemy obj in toSpawn)
        {
            if (Random.value < 0.5f) { continue; }
            Enemy g = Instantiate(obj);
            g.transform.position = spawnPoint.position;
            enemys.Add(g);
            g.SetTower(this);
            Map.enemies.Add(g.gameObject);
        }
    }
    [SerializeField] float probaElem;

    public void Remove(Enemy enemy)
    {
        enemys.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasGive) { return; }
        if (other.CompareTag("Player") && enemys.Count == 0)
        {
            hasGive = true;
            other.GetComponent<PlayerSelection>().AddNumberOfArrow(Map.rewardtype, (byte)Random.Range(5, 10));
            other.GetComponent<PlayerSelection>().AddNumberOfArrow(ArrowType.Clasique, (byte)Random.Range(10, 20));
            if (Map.type == MapType.Sky)
            {
                other.GetComponent<PlayerSelection>().AddNumberOfArrow(ArrowType.Wind, (byte)Random.Range(5, 10));
            }
        }
    }
}
