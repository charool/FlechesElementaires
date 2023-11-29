using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
    private Mesh mesh;
    List<Vector3> _vertices = new List<Vector3>(); 
    List<int> _triangles = new List<int>(); 
    List<Color> _colors = new List<Color>();
    List<Vector4> _indexes = new List<Vector4>();
    List<Vector2> _uv = new List<Vector2>();
    List<Vector3> _normals = null;

    public List<Vector3> GetVertices() { return _vertices; }
    public List<int> GetTriangles() { return _triangles; }

    [SerializeField] private bool collide;

    private void Awake()
    {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetMeshData(Vector3[] vertices, int[] triangles, Color[] colors, Vector4[] indexes, Vector2[] uv)
    {
        _vertices.AddRange(vertices);
        _triangles.AddRange(triangles);
        _colors.AddRange(colors);
        _indexes.AddRange(indexes);
        _uv.AddRange(uv);
        _normals = null;
    }
    public void SetMeshGrassData(List<Vector3> vertices, List<int> triangles, List<Vector2> uv)
    {
        _vertices.AddRange(vertices);
        _triangles.AddRange(triangles);
        _uv.AddRange(uv);
        _normals = null;
    }

    public void Aplly()
    {
        mesh.SetVertices(_vertices);
        mesh.SetTriangles(_triangles, 0);

        int nbCaseX = Chunk.nbCaseX;
        if (collide)
        {
            mesh.SetColors(_colors);
            mesh.SetUVs(1, _indexes);
            for (int i = 0; i < 4 * 4 * nbCaseX * nbCaseX; i++)
            {
                _uv[i] = new Vector2(_vertices[i].x, _vertices[i].z);
            }
        }

        mesh.SetUVs(0, _uv);

        if(_normals != null)
        {
            for (int k = 0; k < nbCaseX * nbCaseX; k++)
            {
                int index = 3 * 4 * nbCaseX * nbCaseX;
                _normals[index + 12 * k + 0] = Vector3.up;
                _normals[index + 12 * k + 1] = 
                _normals[index + 12 * k + 2] = -Vector3.Cross
                    (_vertices[index + 12 * k + 2] - _vertices[index + 12 * k + 1], Vector3.forward).normalized;
                _normals[index + 12 * k + 3] = Vector3.up;
                _normals[index + 12 * k + 4] = 
                _normals[index + 12 * k + 5] = -Vector3.Cross
                    (_vertices[index + 12 * k + 5] - _vertices[index + 12 * k + 4], Vector3.left).normalized;
                _normals[index + 12 * k + 6] = Vector3.up;
                _normals[index + 12 * k + 7] = 
                _normals[index + 12 * k + 8] = -Vector3.Cross
                    (_vertices[index + 12 * k + 8] - _vertices[index + 12 * k + 7], Vector3.back).normalized;
                _normals[index + 12 * k + 9] = Vector3.up;
                _normals[index + 12 * k + 10] = 
                _normals[index + 12 * k + 11] = -Vector3.Cross
                    (_vertices[index + 12 * k + 11] - _vertices[index + 12 * k + 10], Vector3.right).normalized;
            }
            mesh.SetNormals(_normals);
        }
        else
        {
            mesh.RecalculateNormals();
            _normals = mesh.normals.ToList();
        }
        if (collide) { this.GetComponent<MeshCollider>().sharedMesh = mesh; }
    }

    public void AddMeshData(List<Vector3> vertices, List<int> triangles, List<Color> colors, List<Vector4> indexes, List<Vector2> uv)
    {
        int offset = _vertices.Count;

        if (vertices.Count == 0) { return; }
        _vertices.AddRange(vertices);
        _colors.AddRange(colors);
        _indexes.AddRange(indexes);
        _uv.AddRange(uv);

        for (int i = 0; i < triangles.Count; i++)
        {
            _triangles.Add(triangles[i] + offset);
        }
    }

    public void Border()
    {
        int nbCaseX = Chunk.nbCaseX;

        for (int i = 0; i < nbCaseX; i++)
        {
            for (int j = 0; j < nbCaseX; j++)
            {
                int indexNew = 3 * 4 * nbCaseX * nbCaseX + 12 * (i * nbCaseX + j);
                int indexT = 3 * 4 * (i * nbCaseX + j);

                _triangles.Add(indexT + 11);
                _triangles.Add(indexT + 10);
                _triangles.Add(indexT + 8);
                _triangles.Add(indexT + 8);
                _triangles.Add(indexT + 10);
                _triangles.Add(indexT + 9);

                _triangles.Add(indexT + 1);
                _triangles.Add(indexT + 11);
                _triangles.Add(indexT + 0);
                _triangles.Add(indexT + 0);
                _triangles.Add(indexT + 11);
                _triangles.Add(indexT + 8);

                _triangles.Add(indexT + 2);
                _triangles.Add(indexT + 8);
                _triangles.Add(indexT + 3);
                _triangles.Add(indexT + 3);
                _triangles.Add(indexT + 8);
                _triangles.Add(indexT + 9);

                _triangles.Add(indexT + 4);
                _triangles.Add(indexT + 9);
                _triangles.Add(indexT + 5);
                _triangles.Add(indexT + 5);
                _triangles.Add(indexT + 9);
                _triangles.Add(indexT + 10);

                _triangles.Add(indexT + 6);
                _triangles.Add(indexT + 10);
                _triangles.Add(indexT + 7);
                _triangles.Add(indexT + 7);
                _triangles.Add(indexT + 10);
                _triangles.Add(indexT + 11);
                
                _triangles.Add(indexT + 0);
                _triangles.Add(indexNew + 2);
                _triangles.Add(indexNew + 1);
                _triangles.Add(indexT + 0);
                _triangles.Add(indexT + 2);
                _triangles.Add(indexNew + 2);

                _triangles.Add(indexT + 3);
                _triangles.Add(indexNew + 5);
                _triangles.Add(indexNew + 4);
                _triangles.Add(indexT + 3);
                _triangles.Add(indexT + 4);
                _triangles.Add(indexNew + 5);

                _triangles.Add(indexT + 5);
                _triangles.Add(indexNew + 8);
                _triangles.Add(indexNew + 7);
                _triangles.Add(indexT + 5);
                _triangles.Add(indexT + 6);
                _triangles.Add(indexNew + 8);

                _triangles.Add(indexT + 7);
                _triangles.Add(indexNew + 11);
                _triangles.Add(indexNew + 10);
                _triangles.Add(indexT + 7);
                _triangles.Add(indexT + 1);
                _triangles.Add(indexNew + 11);

                _triangles.Add(indexT + 1);
                _triangles.Add(indexT + 0);
                _triangles.Add(indexNew + 0);

                _triangles.Add(indexT + 2);
                _triangles.Add(indexT + 3);
                _triangles.Add(indexNew + 3);

                _triangles.Add(indexT + 4);
                _triangles.Add(indexT + 5);
                _triangles.Add(indexNew + 6);

                _triangles.Add(indexT + 6);
                _triangles.Add(indexT + 7);
                _triangles.Add(indexNew + 9);

                _triangles.Add(indexT + 1);
                _triangles.Add(indexNew + 0);
                _triangles.Add(indexNew + 11);

                _triangles.Add(indexT + 0);
                _triangles.Add(indexNew + 1);
                _triangles.Add(indexNew + 0);

                _triangles.Add(indexT + 2);
                _triangles.Add(indexNew + 3);
                _triangles.Add(indexNew + 2);

                _triangles.Add(indexT + 3);
                _triangles.Add(indexNew + 4);
                _triangles.Add(indexNew + 3);

                _triangles.Add(indexT + 4);
                _triangles.Add(indexNew + 6);
                _triangles.Add(indexNew + 5);

                _triangles.Add(indexT + 5);
                _triangles.Add(indexNew + 7);
                _triangles.Add(indexNew + 6);

                _triangles.Add(indexT + 6);
                _triangles.Add(indexNew + 9);
                _triangles.Add(indexNew + 8);

                _triangles.Add(indexT + 7);
                _triangles.Add(indexNew + 10);
                _triangles.Add(indexNew + 9);
            }
        }
        Aplly();
    }
}
