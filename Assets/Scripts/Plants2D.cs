using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System
{
    public class Plants2D : MonoBehaviour
{
    public GameObject branch;
    public GameObject toSave;
    private GameObject _tmpBranch;
    private Vector3 _tmpBranchBottomAngle;
    public GameObject leaf;
    public float speedOfGrowth = 1f;
    public Material[] leafsMaterials = new Material[3];

    public float thick;
    private float _thick1;

    public float thickController;

    public string axiom = "22220";
    public string axmTemp = "";

    public float penCorX;
    public float penCorY;
    public float penCorZ;

    public float stickAngZ = 14f;
    public float tmpStickAngZ;
    public float tmpStickAngY;

    public int level;

    public int itr = 12;

    public List<GameObject> stc = new List<GameObject>();

    private readonly Random _rand = new Random();

    private Vector3[] _bottomVerts;

    private void Start()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        _thick1 = thick - thick / thickController;
    }

    public void GenerateAxiom()
    {
        for (var i = 0; i < itr; i++)
        {
            foreach (var ch in axiom)
            {
                switch (ch)
                {
                    case '0':
                        axmTemp += "1[+20][-20]";
                        break;
                    case '1':
                        axmTemp += "21";
                        break;
                    case '[':
                        axmTemp += '[';
                        level += 1;
                        break;
                    case ']':
                        axmTemp += ']';
                        level -= 1;
                        break;
                    case '2' when (_rand.Next(0, 100) < 7) && (level > 2):
                        axmTemp += "3[^30]";
                        break;
                    case '2':
                        axmTemp += '2';
                        break;
                    default:
                        axmTemp += ch;
                        break;
                }
            }

            axiom = axmTemp;
            axmTemp = "";
        }
    }

    public void Grow()
    {
        toSave = new GameObject();
        while (transform.childCount > 1)
        {
            transform.GetChild(transform.childCount - 1).parent = toSave.transform;
        }
        penCorX = 0f;
        penCorY = 0f;
        penCorZ = 0f;
        stickAngZ = 0f;
        tmpStickAngZ = 0f;
        tmpStickAngY = 0f;
        level = 0;
        _tmpBranch = Instantiate(branch, new Vector3(penCorX, penCorY, penCorZ), Quaternion.Euler(0f, tmpStickAngY, tmpStickAngZ));
        _tmpBranch.transform.parent = transform;
                        
        var tmpBranchScale = _tmpBranch.transform.localScale;
                        
        thick = _thick1;
        _thick1 -= _thick1 / thickController;
        SetThickness(new []{thick, _thick1});
        _bottomVerts = _tmpBranch.GetComponent<MeshFilter>().mesh.vertices;
                        
        _tmpBranch.GetComponent<Branch>().thick = thick;
        _tmpBranch.GetComponent<Branch>().thick1 = _thick1;
                
        penCorX += (float)(Math.Cos((90 + tmpStickAngZ) * Math.PI / 180)) * (float)(Math.Sin((90 + tmpStickAngY) * Math.PI / 180)) * tmpBranchScale.y;
        penCorY += (float)(Math.Cos((tmpStickAngZ) * Math.PI / 180)) * tmpBranchScale.y;
        penCorZ += (float)(Math.Sin((tmpStickAngZ) * Math.PI / 180)) * (float)(Math.Sin((tmpStickAngY) * Math.PI / 180)) * tmpBranchScale.y;
        StartCoroutine(GrowPlant());
    }

    public void DestroyTrash()
    {
        Destroy(toSave);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator GrowPlant()
    {
        foreach (var ch in axiom)
        {
            tmpStickAngY = _rand.Next(0, 360);
            switch (ch)
            {
                case '+':
                    tmpStickAngZ = -stickAngZ - _rand.Next(20, 50);
                    break;
                case '-':
                    tmpStickAngZ = stickAngZ + _rand.Next(20, 50);
                    break;
                case '^':
                {
                    var ug = _rand.Next(-30, 30);

                    if (ug < 0)
                    {
                        tmpStickAngZ += -ug - 25;
                    }
                    else
                    {
                        tmpStickAngZ += ug + 25;
                    }

                    break;
                }
                case '[':
                    level += 1;
                    stc.Add(_tmpBranch);
                    break;
                case ']':
                {
                    level -= 1;
                    _tmpBranch = stc[stc.Count - 1];
                    _bottomVerts = _tmpBranch.GetComponent<MeshFilter>().mesh.vertices;
                    
                    var tmpBranchPosition = _tmpBranch.transform.position;
                    var tmpBranchRotation = _tmpBranch.transform.eulerAngles;
                    
                    stc.RemoveAt(stc.Count - 1);
                    
                    thick = _tmpBranch.GetComponent<Branch>().thick;
                    _thick1 = _tmpBranch.GetComponent<Branch>().thick1;

                    penCorX = tmpBranchPosition.x;
                    penCorY = tmpBranchPosition.y;
                    penCorZ = tmpBranchPosition.z;

                    tmpStickAngZ = tmpBranchRotation.z;
                    tmpStickAngY = tmpBranchRotation.y;
                    break;
                }
                case '0':
                    Instantiate(leaf, new Vector3(penCorX, penCorY, penCorZ), Quaternion.identity).transform.parent = transform;
                    leaf.GetComponent<MeshRenderer>().material = leafsMaterials[_rand.Next(0,3)];
                    break;
                default:
                {
                    if(_rand.Next(2, 10) > 4)
                    {
                        _tmpBranch = Instantiate(branch, new Vector3(penCorX, penCorY, penCorZ), Quaternion.Euler(0f, tmpStickAngY, tmpStickAngZ));
                        _tmpBranch.transform.parent = transform;
                        
                        var tmpBranchScale = _tmpBranch.transform.localScale;
                        
                        thick = _thick1;
                        _thick1 -= _thick1 / thickController;
                        SetThickness(new []{thick, _thick1});
                        
                        var tmpBranchMesh = _tmpBranch.GetComponent<MeshFilter>().mesh;
                        var tmpBranchVerts = tmpBranchMesh.vertices;
                        _bottomVerts.transform.eulerAngles = _tmpBranchBottomAngle;
                        for (var i = 0; i < 48; i++)
                        {
                            tmpBranchVerts[i + 48].x = _bottomVerts.GetComponent<MeshFilter>().mesh.vertices[i].x;
                            tmpBranchVerts[i + 48].z = _bottomVerts.GetComponent<MeshFilter>().mesh.vertices[i].z;
                        }

                        _bottomVerts = _tmpBranch;

                        tmpBranchMesh.vertices = tmpBranchVerts;
                        _tmpBranch.GetComponent<MeshFilter>().mesh = tmpBranchMesh;
                        
                        _tmpBranch.GetComponent<Branch>().thick = thick;
                        _tmpBranch.GetComponent<Branch>().thick1 = _thick1;
                
                        penCorX += (float)(Math.Cos((90 + tmpStickAngZ) * Math.PI / 180)) * (float)(Math.Sin((90 + tmpStickAngY) * Math.PI / 180)) * tmpBranchScale.y;
                        penCorY += (float)(Math.Cos((tmpStickAngZ) * Math.PI / 180)) * tmpBranchScale.y;
                        penCorZ += (float)(Math.Sin((tmpStickAngZ) * Math.PI / 180)) * (float)(Math.Sin((tmpStickAngY) * Math.PI / 180)) * tmpBranchScale.y;
                        _tmpBranchBottomAngle = _tmpBranch.transform.eulerAngles;
                        yield return new WaitForSeconds(speedOfGrowth);
                    }

                    break;
                }
            }
        }
        StopCoroutine(GrowPlant());
    }

    private void SetThickness(IReadOnlyList<float> thickness)
    {
        var mesh = _tmpBranch.GetComponent<MeshFilter>().mesh;

        var vertices = mesh.vertices;

        for (var i = 0; i < 48; i += 3)
        {
            vertices[i + 1] = new Vector3(vertices[i + 1].x * thickness[1] * thickness[1]/3f, vertices[i + 1].y, vertices[i + 1].z * thickness[1] * thickness[1]/3f);
            vertices[i + 2] = new Vector3(vertices[i + 2].x * thickness[1] * thickness[1]/3f, vertices[i + 2].y, vertices[i + 2].z * thickness[1] * thickness[1]/3f);
        }
        
        for (var i = 48; i < 96; i += 3)
        {
            vertices[i + 1] = new Vector3(vertices[i + 1].x * thickness[0] * thickness[0]/3f, vertices[i + 1].y, vertices[i + 1].z * thickness[0] * thickness[0]/3f);
            vertices[i + 2] = new Vector3(vertices[i + 2].x * thickness[0] * thickness[0]/3f, vertices[i + 2].y, vertices[i + 2].z * thickness[0] * thickness[0]/3f);
        }
        
        var tmpIdx1 = 1;
        var tmpIdx2 = 2;

        var tmpIdx3 = 48 + 1;
        var tmpIdx4 = 48 + 2;
        for (var i = 96; i < 192; i += 6)
        {
            var tmpVector = new Vector3(vertices[tmpIdx3].x, vertices[tmpIdx3].y, vertices[tmpIdx3].z);
            var tmpVector1 = new Vector3(vertices[tmpIdx1].x, vertices[tmpIdx1].y, vertices[tmpIdx1].z);
            var tmpVector2 = new Vector3(vertices[tmpIdx4].x, vertices[tmpIdx4].y, vertices[tmpIdx4].z);

            var tmpVector3 = new Vector3(vertices[tmpIdx1].x, vertices[tmpIdx1].y, vertices[tmpIdx1].z);
            var tmpVector4 = new Vector3(vertices[tmpIdx2].x, vertices[tmpIdx2].y, vertices[tmpIdx2].z);
            var tmpVector5 = new Vector3(vertices[tmpIdx4].x, vertices[tmpIdx4].y, vertices[tmpIdx4].z);


            vertices[i] = tmpVector;
            vertices[i + 1] = tmpVector1;
            vertices[i + 2] = tmpVector2;

            vertices[i + 3] = tmpVector3;
            vertices[i + 4] = tmpVector4;
            vertices[i + 5] = tmpVector5;
            tmpIdx1 += 3;
            tmpIdx2 += 3;
            tmpIdx3 += 3;
            tmpIdx4 += 3;
        }
        mesh.vertices = vertices;
        _tmpBranch.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void TurnBottom(Vector3 angle)
    {
        
    }
}
}
