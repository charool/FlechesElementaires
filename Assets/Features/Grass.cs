using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private ProceduralMesh terrain;
    [SerializeField]
    private int number;
    [SerializeField]
    private float size;
    [SerializeField]
    private GameObject grass;
    public void Genere()
    {
        List<Vector3> vertices = terrain.GetVertices();
        List<int> triangles = terrain.GetTriangles();

        List<Vector3> verticesw = new List<Vector3>();
        List<int> trianglesw = new List<int>();
        List<Vector2> uvw = new List<Vector2>();

        int nbCaseX = Chunk.nbCaseX;

        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                if ((GetComponentInParent<Chunk>().GetCell(i, j).textureIndex != (int)CellType.Grass1
                    && GetComponentInParent<Chunk>().GetCell(i, j).textureIndex != (int)CellType.Grass2)
                    || Random.Range(0f,1f) < 0.5f)
                {
                    continue;
                }
                int index = 12 * (i * nbCaseX + j);
                int indexNew = 3 * 4 * nbCaseX * nbCaseX + 12 * (i * nbCaseX + j);
                Vector3 mid = (vertices[index + 8] + vertices[index + 9] + 
                    vertices[index + 10] + vertices[index + 11]) / 4f;

                if (mid.y > Map.instance.waterLevel) { AddGrass(mid); }

                for (int face = 0; face < 4; face++)
                {
                    //Vector3 dep = vertices[indexNew + 0 + 3 * face];
                    //Vector3 vec1 = vertices[indexNew + (3 + 3 * face) % 12] - dep;
                    Vector3 dep = (vertices[index + 0 + 2 * face] + vertices[index + 1 + 2 * face])/2f;
                    Vector3 sec = (vertices[index + 0 + 2 * (face+1)%8] + vertices[index + 1 + 2 * (face+1)%8]) / 2f;
                    Vector3 vec1 = sec - dep;
                    Vector3 vec2 = mid - dep;
                    Vector3 midface = (mid + dep + sec) / 3f;


                    for (int k = 0; k < number; k++)
                    {
                        for(int kk = 0; kk < number - k; kk++)
                        {
                            float noise1 = Random.Range(-0.05f, 0.05f);
                            float noise2 = Random.Range(-0.05f, 0.05f);

                            Vector3 pos = (kk / 5f+ noise1) * vec1 + 
                                (k / 5f + noise2) * vec2 + dep;
                            if (pos.y < Map.instance.waterLevel) { break; }
                            pos = (midface - pos) / 15f + pos;
                            AddGrass(pos);
                        }
                    }
                }
            }
        }


        //GetComponent<ProceduralMesh>().SetMeshGrassData(verticesw,trianglesw,uvw);
        //GetComponent<ProceduralMesh>().Aplly();

        void AddGrass(Vector3 pos)
        {
            GameObject g = Instantiate(grass);
            g.transform.SetParent(transform);
            g.transform.position = pos + transform.position;/*
            int index = verticesw.Count;

            verticesw.Add(pos + Vector3.left * size);
            verticesw.Add(pos + Vector3.right * size);
            verticesw.Add(pos + Vector3.right * size + Vector3.up * size * 2f);
            verticesw.Add(pos + Vector3.left * size + Vector3.up * size * 2f);

            trianglesw.Add(index + 0);
            trianglesw.Add(index + 3);
            trianglesw.Add(index + 2);
            trianglesw.Add(index + 0);
            trianglesw.Add(index + 2);
            trianglesw.Add(index + 1);

            uvw.Add(new Vector2(0, 0));
            uvw.Add(new Vector2(1, 0));
            uvw.Add(new Vector2(1, 1));
            uvw.Add(new Vector2(0, 1));*/
        }
    }
}
