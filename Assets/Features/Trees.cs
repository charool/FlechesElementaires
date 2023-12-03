using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    [SerializeField]
    private ProceduralMesh terrain;
    [SerializeField]
    private int number;
    [SerializeField]
    private float size;
    [SerializeField]
    private GameObject tree1;
    [SerializeField]
    private GameObject tree2;
    [SerializeField]
    private GameObject treeSnow;
    [SerializeField]
    private GameObject treeDead;
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
                if (Random.Range(0f,1f) < 0.5f)
                {
                    continue;
                }
                int index = 12 * (i * nbCaseX + j);
                int indexNew = 3 * 4 * nbCaseX * nbCaseX + 12 * (i * nbCaseX + j);
                Vector3 mid = (vertices[index + 8] + vertices[index + 9] + 
                    vertices[index + 10] + vertices[index + 11]) / 4f;

                if (mid.y > Map.instance.waterLevel) { AddTree(mid,i,j); }

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

                            Vector3 pos = (kk / number + noise1) * vec1 + 
                                (k / number + noise2) * vec2 + dep;
                            if (pos.y < Map.instance.waterLevel) { break; }
                            pos = (midface - pos) / number / 3f + pos;
                            AddTree(pos,i,j);
                        }
                    }
                }
            }
        }


        //GetComponent<ProceduralMesh>().SetMeshGrassData(verticesw,trianglesw,uvw);
        //GetComponent<ProceduralMesh>().Aplly();

        void AddTree(Vector3 pos,int i,int j)
        {
            if(Random.value > arbreProba) { return; }
            int biomeIndex = GetComponentInParent<Chunk>().GetCell(i, j).biomeIndex;
            int cellTextureIndex = GetComponentInParent<Chunk>().GetCell(i, j).textureIndex;
            if (Map.type == MapType.Earth || Map.type == MapType.Sky)
            {
                if((biomeIndex == (int)BiomeType.GrassLand1) ||
                    (biomeIndex == (int)BiomeType.Mountain))
                {
                    GameObject g = Instantiate(tree2);
                    g.transform.SetParent(transform);
                    g.transform.position = pos + transform.position;
                }
                else if ((biomeIndex == (int)BiomeType.GrassLand2))
                {
                    GameObject g = Instantiate(tree1);
                    g.transform.SetParent(transform);
                    g.transform.position = pos + transform.position;
                }
            }
            else 
            if (Map.type == MapType.IceDesert)
            {
                if ((biomeIndex == (int)BiomeType.GrassLand1) ||
                    (biomeIndex == (int)BiomeType.GrassLand2) ||
                    (biomeIndex == (int)BiomeType.Mountain))
                {
                    GameObject g;
                    if (cellTextureIndex == (int)CellType.Snow || cellTextureIndex == (int)CellType.Ice)
                    {
                        g = Instantiate(treeSnow);
                    }
                    else { g = Instantiate(tree2); }
                    g.transform.SetParent(transform);
                    g.transform.position = pos + transform.position;
                }
            }
            else 
            if (Map.type == MapType.LavaDesert)
            {
                print("lava Tree");
                if ((biomeIndex == (int)BiomeType.GrassLand1) ||
                    (biomeIndex == (int)BiomeType.GrassLand2) ||
                    (biomeIndex == (int)BiomeType.Mountain))
                {
                    GameObject g = Instantiate(treeDead);
                    g.transform.SetParent(transform);
                    g.transform.position = pos + transform.position;
                }
            }
        }
    }
    [SerializeField] float arbreProba;
}
