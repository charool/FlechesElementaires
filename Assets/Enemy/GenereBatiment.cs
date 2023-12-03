using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GenereBatiment : MonoBehaviour
{
    [SerializeField] Tower tower;
    [SerializeField] House house;

    [SerializeField]
    private ProceduralMesh terrain;
    [SerializeField]
    private float probaBat;
    public void Genere()
    {
        List<Vector3> vertices = terrain.GetVertices();
        int nbCaseX = Chunk.nbCaseX;

        if(Random.value < probaBat) 
        {
            int i = nbCaseX / 2;
            int j = nbCaseX / 2;
            int index = 12 * (i * nbCaseX + j);
            int indexNew = 3 * 4 * nbCaseX * nbCaseX + 12 * (i * nbCaseX + j);
            Vector3 mid = (vertices[index + 8] + vertices[index + 9] +
                vertices[index + 10] + vertices[index + 11]) / 4f;

            if (mid.y > Map.instance.waterLevel)
            {
                if (Random.value < 0.5f)
                {
                    House g = Instantiate(house);
                    g.transform.SetParent(transform);
                    g.transform.position = mid + transform.position;
                }
                else
                {
                    Tower g = Instantiate(tower);
                    g.transform.SetParent(transform);
                    g.transform.position = mid + transform.position;
                    StartCoroutine(Delay(g));
                }
            }
        }
    }
    private IEnumerator Delay(Tower T)
    {
        yield return new WaitForSeconds(0.2f);
        T.Spawn();
    }
}
