using UnityEditor;
using UnityEngine;

public class GrowBranch : MonoBehaviour
{
    Mesh mesh;
    int[] triangles;
    Vector3[] vertices;

    public float vertical_size;
    public float horizontal_size;
    public int horizontal_sections;
    float angle;
    float tmp_angle;
    public float r;
    public float[] thickness = new float[2];
    public Vector3 rot = new Vector3();


    public void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        
    }

    public void FixedUpdate()
    {
        UpdateMesh(horizontal_sections, vertical_size, horizontal_size);
    }

    public void UpdateMesh(int horizontal_sections, float v_size, float h_size)
    {
        mesh.Clear();
        angle = 0f;
        tmp_angle = 360f / horizontal_sections;

        vertices = new Vector3[horizontal_sections * 12];

        for (int i = 0; i < horizontal_sections * 3; i+=3)
        {
            vertices[i] = new Vector3(0, v_size, 0);
            vertices[i + 1] = new Vector3(vertices[0].x + Mathf.Cos(angle * Mathf.PI / 180.0f) * h_size * thickness[0], vertices[i].y, vertices[0].z + Mathf.Sin(angle * Mathf.PI / 180.0f) * h_size * thickness[0]);
            angle += tmp_angle;
            vertices[i + 2] = new Vector3(vertices[0].x + Mathf.Cos(angle * Mathf.PI / 180.0f) * h_size * thickness[0], vertices[i].y, vertices[0].z + Mathf.Sin(angle * Mathf.PI / 180.0f) * h_size * thickness[0]);
        }

        for (int i = horizontal_sections * 3; i < horizontal_sections * 6; i += 3)
        {
            vertices[i] = new Vector3(0, 0, 0);
            vertices[i + 1] = new Vector3(vertices[0].x + Mathf.Cos(angle * Mathf.PI / 180.0f) * h_size * thickness[1], vertices[i].y, vertices[0].z + Mathf.Sin(angle * Mathf.PI / 180.0f) * h_size * thickness[1]);
            angle += tmp_angle;
            vertices[i + 2] = new Vector3(vertices[0].x + Mathf.Cos(angle * Mathf.PI / 180.0f) * h_size * thickness[1], vertices[i].y, vertices[0].z + Mathf.Sin(angle * Mathf.PI / 180.0f) * h_size * thickness[1]);
        }

        int tmp_idx1 = 1;
        int tmp_idx2 = 2;

        int tmp_idx3 = horizontal_sections * 3 + 1;
        int tmp_idx4 = horizontal_sections * 3 + 2;
        for (int i = horizontal_sections * 6; i < horizontal_sections * 12; i+=6)
        {
            Vector3 tmp_vector = new Vector3(vertices[tmp_idx3].x, vertices[tmp_idx3].y, vertices[tmp_idx3].z);
            Vector3 tmp_vector1 = new Vector3(vertices[tmp_idx1].x, vertices[tmp_idx1].y, vertices[tmp_idx1].z);
            Vector3 tmp_vector2 = new Vector3(vertices[tmp_idx4].x, vertices[tmp_idx4].y, vertices[tmp_idx4].z);

            Vector3 tmp_vector3 = new Vector3(vertices[tmp_idx1].x, vertices[tmp_idx1].y, vertices[tmp_idx1].z);
            Vector3 tmp_vector4 = new Vector3(vertices[tmp_idx2].x, vertices[tmp_idx2].y, vertices[tmp_idx2].z);
            Vector3 tmp_vector5 = new Vector3(vertices[tmp_idx4].x, vertices[tmp_idx4].y, vertices[tmp_idx4].z);


            vertices[i] = tmp_vector;
            vertices[i + 1] = tmp_vector1;
            vertices[i + 2] = tmp_vector2;

            vertices[i + 3] = tmp_vector3;
            vertices[i + 4] = tmp_vector4;
            vertices[i + 5] = tmp_vector5;
            tmp_idx1 += 3;
            tmp_idx2 += 3;
            tmp_idx3 += 3;
            tmp_idx4 += 3;
        }

        SetTriangles();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public void SetTriangles()
    {
        triangles = new int[horizontal_sections * 12];

        for (int i = 0; i < horizontal_sections * 3; i += 3)
        {
            triangles[i] = i;
            triangles[i + 1] = i + 2;
            triangles[i + 2] = i + 1;
        }

        for (int i = horizontal_sections * 3; i < horizontal_sections * 6; i++)
        {
            triangles[i] = i;
        }

        for (int i = horizontal_sections * 6; i < horizontal_sections * 12; i+=6)
        {
            triangles[i] = i;
            triangles[i + 1] = i + 1;
            triangles[i + 2] = i + 2;

            triangles[i + 3] = i + 3;
            triangles[i + 4] = i + 4;
            triangles[i + 5] = i + 5;
        }

    }

    

    public void SaveAsset()
    {
        GameObject branch_1 = new GameObject();
        branch_1.AddComponent<MeshFilter>();
        branch_1.AddComponent<MeshRenderer>();
        branch_1.GetComponent<MeshFilter>().mesh = mesh;
        AssetDatabase.CreateAsset(mesh, "Assets/Branch_1.asset");
        AssetDatabase.SaveAssets();
    }
}
