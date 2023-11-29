using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int ID;
    public Vector2Int indexXZ;
    [SerializeField]
    private GameObject terrain;
    [SerializeField]
    private GameObject grass;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private float step = 0.5f;

    public static int nbCaseX = 6; // even
    public bool generated = false;

    Cell[,] cells;

    private const float borderFactor = 0.1f;

    public void Unactive()
    {
        gameObject.SetActive(false);
    }

    public void Active()
    {
        gameObject.SetActive(true);

        if(!generated)
        {
            generated = true;
            Generate();
        }

    }

    public Cell GetCell(int z, int x)
    {
        return cells[z, x];
    }

    private Color red = new Color(1f, 0f, 0f, 0f);
    private Color green = new Color(0f, 1f, 0f, 0f);
    private Color blue = new Color(0f, 0f, 1f, 0f);
    private Color black = new Color(0f, 0f, 0f, 1f);

    private void Generate()
    {
        cells = new Cell[nbCaseX,nbCaseX];

        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                cells[j, i] = new Cell();
                Cell cell = cells[j,i];
                cell.textureIndex = GetTextureIndex(i, j);
            }
        }

        Vector3[] vertices = new Vector3[(3 + 1 + 2) * 4 * nbCaseX * nbCaseX];
        Color[] colors = new Color[(3 + 1 + 2) * 4 * nbCaseX * nbCaseX];
        Vector4[] indexes = new Vector4[(3 + 1 + 2) * 4 * nbCaseX * nbCaseX];
        Vector2[] UV = new Vector2[(3 + 1 + 2) * 4 * nbCaseX * nbCaseX];
        int[] triangles = new int[3 * 4 * nbCaseX * nbCaseX];

        List<Vector3> verticesAdd = new List<Vector3>();
        List<int> trianglesAdd = new List<int>();
        List<Color> colorsAdd = new List<Color>();
        List<Vector4> indexesAdd = new List<Vector4>();
        List<Vector2> UVAdd = new List<Vector2>();

        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                int index = 3 * 4 * (i * nbCaseX + j);

                int height0; int height1; int height2; int height3;

                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        height0 = height1 = height2 = height3 = GetHeigth(i, j);
                    }
                    else
                    {
                        height0 = height3 = GetHeigth(i, j - 1);
                        height1 = height2 = GetHeigth(i, j + 1);
                        if (Mathf.Abs(height0 - height1) > 1)
                        {
                            height0 = height1 = height2 = height3 = Mathf.Min(height0, height1);
                        }
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        height0 = height1 = GetHeigth(i - 1, j);
                        height2 = height3 = GetHeigth(i + 1, j);
                        if (Mathf.Abs(height0 - height2) > 1)
                        { height0 = height1 = height2 = height3 = Mathf.Min(height0, height2); }
                    }
                    else
                    {
                        int height0Mem = height0 = GetHeigth(i - 1, j - 1);
                        int height1Mem = height1 = GetHeigth(i - 1, j + 1);
                        int height2Mem = height2 = GetHeigth(i + 1, j + 1);
                        int height3Mem = height3 = GetHeigth(i + 1, j - 1);
                        if (Mathf.Abs(height0Mem - height1Mem) > 1)
                        { height0 = height1 = Mathf.Min(height0Mem, height1Mem); }
                        if (Mathf.Abs(height0Mem - height3Mem) > 1)
                        { height0 = height3 = Mathf.Min(height0Mem, height3Mem); }
                        if (Mathf.Abs(height2Mem - height1Mem) > 1)
                        { height2 = height1 = Mathf.Min(height2Mem, height1Mem); }
                        if (Mathf.Abs(height2Mem - height3Mem) > 1)
                        { height2 = height3 = Mathf.Min(height2Mem, height3Mem); }

                        height0Mem = height0;
                        height1Mem = height1;
                        height2Mem = height2;
                        height3Mem = height3;
                        if (Mathf.Abs(height0Mem - height1Mem) > 1)
                        { height0 = height1 = Mathf.Min(height0Mem, height1Mem); }
                        if (Mathf.Abs(height0Mem - height3Mem) > 1)
                        { height0 = height3 = Mathf.Min(height0Mem, height3Mem); }
                        if (Mathf.Abs(height2Mem - height1Mem) > 1)
                        { height2 = height1 = Mathf.Min(height2Mem, height1Mem); }
                        if (Mathf.Abs(height2Mem - height3Mem) > 1)
                        { height2 = height3 = Mathf.Min(height2Mem, height3Mem); }
                    }
                }
                float height4 = SetMidHeight(height0, height1, height2, height3);

                vertices[index + 0] = new Vector3((float)j, height0 * step, (float)i);
                vertices[index + 1] = new Vector3((float)j, height0 * step, (float)i);
                vertices[index + 2] = new Vector3((float)j + 1f, height1 * step, (float)i);
                vertices[index + 3] = new Vector3((float)j + 1f, height1 * step, (float)i);
                vertices[index + 4] = new Vector3((float)j + 1f, height2 * step, (float)i + 1f);
                vertices[index + 5] = new Vector3((float)j + 1f, height2 * step, (float)i + 1f);
                vertices[index + 6] = new Vector3((float)j, height3 * step, (float)i + 1f);
                vertices[index + 7] = new Vector3((float)j, height3 * step, (float)i + 1f);
                vertices[index + 8] = new Vector3((float)j + 0.5f, height4 * step, (float)i + 0.5f);
                vertices[index + 9] = new Vector3((float)j + 0.5f, height4 * step, (float)i + 0.5f);
                vertices[index + 10] = new Vector3((float)j + 0.5f, height4 * step, (float)i + 0.5f);
                vertices[index + 11] = new Vector3((float)j + 0.5f, height4 * step, (float)i + 0.5f);

                colors[index + 0] =
                colors[index + 1] =
                colors[index + 2] =
                colors[index + 3] =
                colors[index + 4] =
                colors[index + 5] =
                colors[index + 6] =
                colors[index + 7] =
                colors[index + 8] =
                colors[index + 9] =
                colors[index + 10] =
                colors[index + 11] = red - new Color(0f,0f,0f,0.35f);

                int tIndex = GetTextureIndex(i, j);

                indexes[index + 0] = 
                indexes[index + 1] = new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i - 1, j),
                    GetTextureIndex(i, j - 1),
                    GetTextureIndex(i - 1, j - 1));
                indexes[index + 2] = 
                indexes[index + 3] = new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i - 1, j),
                    GetTextureIndex(i, j + 1),
                    GetTextureIndex(i - 1, j + 1));
                indexes[index + 4] = 
                indexes[index + 5] = new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i + 1, j),
                    GetTextureIndex(i, j + 1),
                    GetTextureIndex(i + 1, j + 1));
                indexes[index + 6] = 
                indexes[index + 7] = new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i + 1, j),
                    GetTextureIndex(i, j - 1),
                    GetTextureIndex(i + 1, j - 1));
                indexes[index + 8] = 
                indexes[index + 9] = 
                indexes[index + 10] = 
                indexes[index + 11] = new Vector4(tIndex, tIndex, tIndex, tIndex);

                int indexT = 3 * 4 * (i * nbCaseX + j);

                triangles[indexT + 0] = index + 8;
                triangles[indexT + 1] = index + 2;
                triangles[indexT + 2] = index + 0;

                triangles[indexT + 3] = index + 9;
                triangles[indexT + 4] = index + 4;
                triangles[indexT + 5] = index + 3;

                triangles[indexT + 6] = index + 10;
                triangles[indexT + 7] = index + 6;
                triangles[indexT + 8] = index + 5;

                triangles[indexT + 9] = index + 11;
                triangles[indexT + 10] = index + 1;
                triangles[indexT + 11] = index + 7;
            }
        }
        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                int index = 3 * 4 * (i * nbCaseX + j);
                int indexNew = 3 * 4 * (nbCaseX * nbCaseX) + 12 * (i * nbCaseX + j);
                vertices[indexNew + 0] = vertices[index + 0];
                vertices[indexNew + 3] = vertices[index + 2];
                vertices[indexNew + 6] = vertices[index + 4];
                vertices[indexNew + 9] = vertices[index + 6];
                vertices[indexNew + 1] = 0.9f * vertices[indexNew + 0] + 0.1f * vertices[indexNew + 3];
                vertices[indexNew + 2] = 0.9f * vertices[indexNew + 3] + 0.1f * vertices[indexNew + 0];
                vertices[indexNew + 4] = 0.9f * vertices[indexNew + 3] + 0.1f * vertices[indexNew + 6];
                vertices[indexNew + 5] = 0.9f * vertices[indexNew + 6] + 0.1f * vertices[indexNew + 3];
                vertices[indexNew + 7] = 0.9f * vertices[indexNew + 6] + 0.1f * vertices[indexNew + 9];
                vertices[indexNew + 8] = 0.9f * vertices[indexNew + 9] + 0.1f * vertices[indexNew + 6];
                vertices[indexNew + 10] = 0.9f * vertices[indexNew + 9] + 0.1f * vertices[indexNew + 0];
                vertices[indexNew + 11] = 0.9f * vertices[indexNew + 0] + 0.1f * vertices[indexNew + 9];

                colors[indexNew + 0] = 
                colors[indexNew + 3] =
                colors[indexNew + 6] =
                colors[indexNew + 9] = (red + green + blue + black) / 4f;
                colors[indexNew + 1] =
                colors[indexNew + 2] = 
                colors[indexNew + 7] = 
                colors[indexNew + 8] = (green + red) / 2f - new Color(0f, 0f, 0f, 0.35f); ;
                colors[indexNew + 4] =
                colors[indexNew + 5] =
                colors[indexNew + 10] =
                colors[indexNew + 11] = (blue + red) / 2f - new Color(0f, 0f, 0f, 0.35f); ;


                indexes[indexNew + 0] =
                indexes[indexNew + 1] =
                indexes[indexNew + 11] = new Vector4(GetTextureIndex(i,j),
                    GetTextureIndex(i - 1, j),
                    GetTextureIndex(i, j - 1),
                    GetTextureIndex(i - 1, j - 1));

                indexes[indexNew + 3] =
                indexes[indexNew + 4] =
                indexes[indexNew + 2] = new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i - 1, j),
                    GetTextureIndex(i, j + 1),
                    GetTextureIndex(i - 1, j + 1));

                indexes[indexNew + 6] =
                indexes[indexNew + 7] =
                indexes[indexNew + 5] = new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i + 1, j),
                    GetTextureIndex(i, j + 1),
                    GetTextureIndex(i + 1, j + 1));
                indexes[indexNew + 9] =
                indexes[indexNew + 10] =
                indexes[indexNew + 8] = 
                    new Vector4(GetTextureIndex(i, j),
                    GetTextureIndex(i + 1, j),
                    GetTextureIndex(i, j - 1),
                    GetTextureIndex(i + 1, j - 1));

                for(int k = 0; k < 12; k++)
                {
                    UV[index + k] = new Vector2(vertices[index + k].x, vertices[index + k].z);
                    UV[indexNew + k] = new Vector2(vertices[indexNew + k].x, vertices[indexNew + k].z);
                }
            }
        }
        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                int index = 3 * 4 * (i * nbCaseX + j);
                int height0;
                int height1;
                int height2;
                int height3;
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {

                    }
                    else
                    {
                        height0 = height3 = GetHeigth(i, j - 1);
                        height1 = height2 = GetHeigth(i, j + 1);
                        if (height0 + 1 < height1)
                        {
                            //int indexNeighbor = 3 * 4 * (i * nbCaseX + j + 1);
                            //AddCliff(1 * 2 + index, 2 * 2 + index, 0 * 2 + indexNeighbor);
                            //AddCliff(2 * 2 + index, 3 * 2 + indexNeighbor, 0 * 2 + indexNeighbor);
                            AddCliff(1, (i, j), 2, (i, j), 0, (i, j + 1));
                            AddCliff(2, (i, j), 3, (i, j + 1), 0, (i, j + 1));//3
                        }
                        else if (height1 + 1 < height0)
                        {
                            //int indexNeighbor = 3 * 4 * (i * nbCaseX + j - 1);
                            //AddCliff(0 * 2 + index, 1 * 2 + indexNeighbor, 3 * 2 + index);
                            //AddCliff(3 * 2 + index, 1 * 2 + indexNeighbor, 2 * 2 + indexNeighbor);
                            AddCliff(0, (i, j), 1, (i, j - 1), 3, (i, j));
                            AddCliff(3, (i, j), 1, (i, j - 1), 2, (i, j - 1));
                        }
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        height0 = height1 = GetHeigth(i - 1, j);
                        height2 = height3 = GetHeigth(i + 1, j);
                        if (height0 + 1 < height2)
                        {
                            //int indexNeighbor = 3 * 4 * ((i + 1) * nbCaseX + j);
                            //AddCliff(2 * 2 + index, 3 * 2 + index, 1 * 2 + indexNeighbor);
                            //AddCliff(3 * 2 + index, 0 * 2 + indexNeighbor, 1 * 2 + indexNeighbor);
                            AddCliff(2, (i, j), 3, (i, j), 1, (i + 1, j));
                            AddCliff(3, (i, j), 0, (i + 1, j), 1, (i + 1, j));
                        }
                        else if (height2 + 1 < height0)
                        {
                            //int indexNeighbor = 3 * 4 * ((i - 1) * nbCaseX + j);
                            //AddCliff(0 * 2 + index, 1 * 2 + index, 3 * 2 + indexNeighbor);
                            //AddCliff(1 * 2 + index, 2 * 2 + indexNeighbor, 3 * 2 + indexNeighbor);
                            AddCliff(0, (i, j), 1, (i, j), 3, (i - 1, j));
                            AddCliff(1, (i, j), 2, (i - 1, j), 3, (i - 1, j));
                        }
                    }
                    else
                    {
                        int height0Mem = height0 = GetHeigth(i - 1, j - 1);
                        int height1Mem = height1 = GetHeigth(i - 1, j + 1);
                        int height2Mem = height2 = GetHeigth(i + 1, j + 1);
                        int height3Mem = height3 = GetHeigth(i + 1, j - 1);
                        if (Mathf.Abs(height0Mem - height1Mem) > 1)
                        { height0 = height1 = Mathf.Min(height0Mem, height1Mem); }
                        if (Mathf.Abs(height0Mem - height3Mem) > 1)
                        { height0 = height3 = Mathf.Min(height0Mem, height3Mem); }
                        if (Mathf.Abs(height2Mem - height1Mem) > 1)
                        { height2 = height1 = Mathf.Min(height2Mem, height1Mem); }
                        if (Mathf.Abs(height2Mem - height3Mem) > 1)
                        { height2 = height3 = Mathf.Min(height2Mem, height3Mem); }

                        height0Mem = height0;
                        height1Mem = height1;
                        height2Mem = height2;
                        height3Mem = height3;
                        if (Mathf.Abs(height0Mem - height1Mem) > 1)
                        { height0 = height1 = Mathf.Min(height0Mem, height1Mem); }
                        if (Mathf.Abs(height0Mem - height3Mem) > 1)
                        { height0 = height3 = Mathf.Min(height0Mem, height3Mem); }
                        if (Mathf.Abs(height2Mem - height1Mem) > 1)
                        { height2 = height1 = Mathf.Min(height2Mem, height1Mem); }
                        if (Mathf.Abs(height2Mem - height3Mem) > 1)
                        { height2 = height3 = Mathf.Min(height2Mem, height3Mem); }

                        height0Mem = GetHeigth(i - 1, j - 1);
                        height1Mem = GetHeigth(i - 1, j + 1);
                        height2Mem = GetHeigth(i + 1, j + 1);
                        height3Mem = GetHeigth(i + 1, j - 1);

                        if (height0 < height0Mem && height1 < height1Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i - 1) * nbCaseX + j - 1);
                            //int indexNeighbor2 = 3 * 4 * ((i - 1) * nbCaseX + j + 1);
                            //AddCliff(0 * 2 + index, 1 * 2 + index, 2 * 2 + indexNeighbor);
                            //AddCliff(1 * 2 + index, 3 * 2 + indexNeighbor2, 2 * 2 + indexNeighbor);
                            AddCliff(0, (i, j), 1, (i, j), 2, (i - 1, j - 1));
                            AddCliff(1, (i, j), 3, (i - 1, j + 1), 2, (i - 1, j - 1));
                        }
                        else if (height0 < height0Mem && height1 + 1 == height0Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i - 1) * nbCaseX + j - 1);
                            //AddCliff(0 * 2 + index, 1 * 2 + index, 2 * 2 + indexNeighbor);
                            AddCliff(0, (i, j), 1, (i, j), 2, (i - 1, j - 1));
                        }
                        else if (height1 < height1Mem && height0 + 1 == height1Mem)
                        {
                            //int indexNeighbor2 = 3 * 4 * ((i - 1) * nbCaseX + j + 1);
                            //AddCliff(1 * 2 + index, 3 * 2 + indexNeighbor2, 0 * 2 + index);
                            AddCliff(1, (i, j), 3, (i - 1, j + 1), 0, (i, j));
                        }

                        if (height0 < height0Mem && height3 < height3Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i - 1) * nbCaseX + j - 1);
                            //int indexNeighbor2 = 3 * 4 * ((i + 1) * nbCaseX + j - 1);
                            //AddCliff(3 * 2 + index, 0 * 2 + index, 1 * 2 + indexNeighbor2);
                            //AddCliff(0 * 2 + index, 2 * 2 + indexNeighbor, 1 * 2 + indexNeighbor2);
                            AddCliff(3, (i, j), 0, (i, j), 1, (i + 1, j - 1));
                            AddCliff(0, (i, j), 2, (i - 1, j - 1), 1, (i + 1, j - 1));
                        }
                        else if (height0 < height0Mem && height3 + 1 == height0Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i - 1) * nbCaseX + j - 1);
                            //AddCliff(0 * 2 + index, 2 * 2 + indexNeighbor, 3 * 2 + index);
                            AddCliff(0, (i, j), 2, (i - 1, j - 1), 3, (i, j));
                        }
                        else if (height3 < height3Mem && height0 + 1 == height3Mem)
                        {
                            //int indexNeighbor2 = 3 * 4 * ((i + 1) * nbCaseX + j - 1);
                            //AddCliff(3 * 2 + index, 0 * 2 + index, 1 * 2 + indexNeighbor2);
                            AddCliff(3, (i, j), 0, (i, j), 1, (i + 1, j - 1));
                        }

                        if (height2 < height2Mem && height3 < height3Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i + 1) * nbCaseX + j - 1);
                            //int indexNeighbor2 = 3 * 4 * ((i + 1) * nbCaseX + j + 1);
                            //AddCliff(2 * 2 + index, 3 * 2 + index, 1 * 2 + indexNeighbor);
                            //AddCliff(2 * 2 + index, 1 * 2 + indexNeighbor, 0 * 2 + indexNeighbor2);
                            AddCliff(2, (i, j), 3, (i, j), 1, (i + 1, j - 1));
                            AddCliff(2, (i, j), 1, (i + 1, j - 1), 0, (i + 1, j + 1));
                        }
                        else if (height2 < height2Mem && height3 + 1 == height2Mem)
                        {
                            //int indexNeighbor2 = 3 * 4 * ((i + 1) * nbCaseX + j + 1);
                            //AddCliff(2 * 2 + index, 3 * 2 + index, 0 * 2 + indexNeighbor2);
                            AddCliff(2, (i, j), 3, (i, j), 0, (i + 1, j + 1));
                        }
                        else if (height3 < height3Mem && height2 + 1 == height3Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i + 1) * nbCaseX + j - 1);
                            //AddCliff(2 * 2 + index, 3 * 2 + index, 1 * 2 + indexNeighbor);
                            AddCliff(2, (i, j), 3, (i, j), 1, (i + 1, j - 1));
                        }

                        if (height2 < height2Mem && height1 < height1Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i + 1) * nbCaseX + j + 1);
                            //int indexNeighbor2 = 3 * 4 * ((i - 1) * nbCaseX + j + 1);
                            //AddCliff(1 * 2 + index, 2 * 2 + index, 3 * 2 + indexNeighbor2);
                            //AddCliff(3 * 2 + indexNeighbor2, 2 * 2 + index, 0 * 2 + indexNeighbor);
                            AddCliff(1, (i, j), 2, (i, j), 3, (i - 1, j + 1));
                            AddCliff(3, (i - 1, j + 1), 2, (i, j), 0, (i + 1, j + 1));
                        }
                        else if (height2 < height2Mem && height1 + 1 == height2Mem)
                        {
                            //int indexNeighbor = 3 * 4 * ((i + 1) * nbCaseX + j + 1);
                            //AddCliff(1 * 2 + index, 2 * 2 + index, 0 * 2 + indexNeighbor);
                            AddCliff(1, (i, j), 2, (i, j), 0, (i + 1, j + 1));
                        }
                        else if (height1 < height1Mem && height2 + 1 == height1Mem)
                        {
                            //int indexNeighbor2 = 3 * 4 * ((i - 1) * nbCaseX + j + 1);
                            //AddCliff(1 * 2 + index, 2 * 2 + index, 3 * 2 + indexNeighbor2);
                            AddCliff(1, (i, j), 2, (i, j), 3, (i - 1, j + 1));
                        }
                    }
                }
            }
        }

        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                int indexT = 3 * 4 * (i * nbCaseX + j);
                for (int k = 0; k < 4; k++)
                {
                    Vector3 mid = (vertices[triangles[indexT + k * 3 + 0]] +
                        vertices[triangles[indexT + k * 3 + 1]] +
                        vertices[triangles[indexT + k * 3 + 2]])/3f;

                    vertices[triangles[indexT + k * 3 + 0]] +=
                        (mid - vertices[triangles[indexT + k * 3 + 0]]).normalized * borderFactor;
                    vertices[triangles[indexT + k * 3 + 1]] +=
                        (mid - vertices[triangles[indexT + k * 3 + 1]]).normalized * borderFactor;
                    vertices[triangles[indexT + k * 3 + 2]] +=
                        (mid - vertices[triangles[indexT + k * 3 + 2]]).normalized * borderFactor;
                }
            }
        }

        ProceduralMesh mesh = terrain.GetComponent<ProceduralMesh>();
        mesh.SetMeshData(vertices, triangles, colors, indexes, UV);
        mesh.AddMeshData(verticesAdd, trianglesAdd, colorsAdd, indexesAdd, UVAdd);
        mesh.Aplly();

        mesh.Border();

        grass.GetComponent<Grass>().Generate();


        void AddCliff(int t1, (int, int) id1, int t2, (int, int) id2, int t3, (int, int) id3)
        {
            /*if (id1.Item1 > 5 || id1.Item2 > 5 || id2.Item1 > 5 || id2.Item2 > 5 || id3.Item1 > 5 || id3.Item2 > 5 ||
                id1.Item1 < 0 || id1.Item2 < 0 || id2.Item1 < 0 || id2.Item2 < 0 || id3.Item1 < 0 || id3.Item2 < 0)
            {
                print("ok");
                return;
            }*/
            int offset = verticesAdd.Count;
            int i1 = t1 * 2 + 3 * 4 * ((id1.Item1 % 6) * nbCaseX + id1.Item2 % 6);
            int i2 = t2 * 2 + 3 * 4 * ((id2.Item1 % 6) * nbCaseX + id2.Item2 % 6);
            int i3 = t3 * 2 + 3 * 4 * ((id3.Item1 % 6) * nbCaseX + id3.Item2 % 6);
            Vector3 v1 = vertices[i1] + new Vector3(6f, 0f, 0f) * (id1.Item2 / 6)
                + new Vector3(0f, 0f, 6f) * (id1.Item1 / 6);
            if (id1.Item1 / 6 != 0 || id1.Item2 / 6 != 0)
            {
                v1.y = GetHeigth(id1.Item1, id1.Item2) * step;
            }
            Vector3 v2 = vertices[i2] + new Vector3(6f, 0f, 0f) * (id2.Item2 / 6)
                + new Vector3(0f, 0f, 6f) * (id2.Item1 / 6);
            if (id2.Item1 / 6 != 0 || id2.Item2 / 6 != 0)
            {
                v2.y = GetHeigth(id2.Item1, id2.Item2) * step;
            }
            Vector3 v3 = vertices[i3] + new Vector3(6f, 0f, 0f) * (id3.Item2 / 6)
                + new Vector3(0f, 0f, 6f) * (id3.Item1 / 6);
            if (id3.Item1 / 6 != 0 || id3.Item2 / 6 != 0)
            {
                v3.y = GetHeigth(id3.Item1, id3.Item2) * step;
            }
            verticesAdd.Add(v1);
            verticesAdd.Add(v2);
            verticesAdd.Add(v3);
            colorsAdd.Add(colors[i1]);
            colorsAdd.Add(colors[i2]);
            colorsAdd.Add(colors[i2]);
            indexesAdd.Add(new Vector4(0, 0, 0, 0));//
            indexesAdd.Add(new Vector4(0, 0, 0, 0));//
            indexesAdd.Add(new Vector4(0, 0, 0, 0));//
            if (Mathf.Abs((v1 - v2).x) + Mathf.Abs((v1 - v3).x) <= 0.1f)
            {
                UVAdd.Add(new Vector2(v1.y, v1.z));
                UVAdd.Add(new Vector2(v2.y, v2.z));
                UVAdd.Add(new Vector2(v3.y, v3.z));
            }
            else
            {
                UVAdd.Add(new Vector2(v1.y, v1.x));
                UVAdd.Add(new Vector2(v2.y, v2.x));
                UVAdd.Add(new Vector2(v3.y, v3.x));
            }
            trianglesAdd.Add(offset + 0);
            trianglesAdd.Add(offset + 1);
            trianglesAdd.Add(offset + 2);
        }
    }

    [SerializeField]
    float scale = 1f;



    private int GetTextureIndex(int z, int x)
    {
        // return <= 5
        float abs = (float)(x + nbCaseX * indexXZ.x) * scale + Map.seed;
        float ord = (float)(z + nbCaseX * indexXZ.y) * scale;
        float val = 4f * Mathf.PerlinNoise(abs, ord) +
            2f * Mathf.PerlinNoise(abs * 2, ord * 2) +
            1f * Mathf.PerlinNoise(abs * 3, ord * 3) +
            0.5f * Mathf.PerlinNoise(abs * 4, ord * 4) +
            0.25f * Mathf.PerlinNoise(abs * 5, ord * 5);
        return Mathf.RoundToInt(val);
    }

    private int GetHeigth(int z ,int x)
    {
        // return <= 5
        float abs = (float)(x + nbCaseX * indexXZ.x)  * scale + Map.seed;
        float ord = (float)(z + nbCaseX * indexXZ.y)  * scale;
        float val = 4f * Mathf.PerlinNoise(abs, ord) +
            2f * Mathf.PerlinNoise(abs*2, ord*2) +
            1f * Mathf.PerlinNoise(abs*3, ord*3) +
            0.5f * Mathf.PerlinNoise(abs*4, ord*4) +
            0.25f * Mathf.PerlinNoise(abs*5, ord*5);
        return Mathf.RoundToInt(val);
    }

    float SetMidHeight(int i0, int i1, int i2, int i3)
    {
        if(i0 == i1 && i1 == i2 || i0 == i1 && i1 == i3 || i0 == i2 && i2 == i3)
        {
            return i0;
        }
        if(i3 == i1 && i1 == i2) { return i1; }
        if(i0 == i1 && i2 == i3 || i0 == i3 && i1 == i2)
        {
            return (float)(i0 + i1 + i2 + i3) / 4f;
        }
        int maxI = Mathf.Max(i0, i1, i2, i3);
        if (i0 == i1 && i2 != i3) { return (float)(i1 + i2 + i3) / 3f; }//
        if (i0 == i2 && i1 != i3) { return (float)(i1 + i2 + i3) / 3f; }
        if (i0 == i3 && i1 != i2) { return (float)(i1 + i2 + i3) / 3f; }//
        if (i1 == i3 && i0 != i2) { return (float)(i0 + i2 + i3) / 3f; }
        if (i1 == i2 && i0 != i3) { return (float)(i0 + i2 + i3) / 3f; }//
        if (i2 == i3 && i0 != i1) { return (float)(i0 + i1 + i3) / 3f; }//
        return (float)(i0 + i1 + i2 + i3) / 4f;
    }
}

/*
                Color c1; Color c2; Color c3; Color c4;
                if (i % 2 == 0)
                {
                    if (j % 2 == 0) { c1 = red; c2 = green; c3 = black; c4 = blue; }
                    else { c1 = green; c2 = red; c3 = blue; c4 = black; }
                }
                else
                {
                    if (j % 2 == 0) { c1 = blue; c2 = black; c3 = green; c4 = red; }
                    else { c1 = black; c2 = blue; c3 = red; c4 = green; }
                }

                colors[index + 0] = c1;
                colors[index + 1] = c1;
                colors[index + 2] = c2;
                colors[index + 3] = c2;
                colors[index + 4] = c3;
                colors[index + 5] = c3;
                colors[index + 6] = c4;
                colors[index + 7] = c4;
                colors[index + 8] = (red + green + blue + black) / 4f;
                colors[index + 9] = (red + green + blue + black) / 4f;
                colors[index + 10] = (red + green + blue + black) / 4f;
                colors[index + 11] = (red + green + blue + black) / 4f;
*/