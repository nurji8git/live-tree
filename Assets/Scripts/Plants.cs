using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plants : MonoBehaviour
{
    public GameObject branch;
    private GameObject tmp_branch;
    public GameObject leaf;
    public float speed_of_growth = 1f;
    private float x;
    private float y;

    public Button growBtn;
    public Button generateAxiomBtn;

    public string axiom = "22220";
    public string axmTemp = "";
    public string current_e;

    public float pen_cor_x = 0f;
    public float pen_cor_y = 0f;

    public float stick_ang_z = 14f;
    public float tmp_stick_ang_z = 0f;

    public int level = 0;

    public int itr = 12;

    public List<GameObject> stc = new List<GameObject>();

    public System.Random rand = new System.Random();

    void Start()
    {
        growBtn.enabled = true;
        generateAxiomBtn.enabled = true;
        this.transform.position = new Vector3(0f, 0f, 0f);
    }

    public void GenerateAxiom()
    {
        generateAxiomBtn.enabled = false;
        for (int i = 0; i < itr; i++)
        {
            foreach (char ch in axiom)
            {
                if (ch == '0')
                {
                    axmTemp += "1[+20][-20]";
                }
                else if (ch == '1')
                {
                    axmTemp += "21";
                }
                else if (ch == '[')
                {
                    axmTemp += '[';
                    level += 1;
                }
                else if (ch == ']')
                {
                    axmTemp += ']';
                    level -= 1;
                }
                else if (ch == '2')
                {
                    if ((rand.Next(0, 100) < 7) && (level > 2))
                    {
                        axmTemp += "3[^30]";
                    }
                    else
                    {
                        axmTemp += '2';
                    }
                }
                else
                {
                    axmTemp += ch;
                }
            }

            axiom = axmTemp;
            axmTemp = "";
        }
    }

    public void Grow()
    {
        growBtn.enabled = false;
        pen_cor_x = 0f;
        pen_cor_y = 0f;
        stick_ang_z = 14f;
        tmp_stick_ang_z = 0f;
        level = 0;
        while (this.transform.childCount > 1)
        {
            Destroy(this.transform.GetChild(this.transform.childCount - 1));
        }
        StartCoroutine(GrowPlant());
    }

    IEnumerator GrowPlant()
    {
        foreach (char ch in axiom)
        {
            current_e = ch + "";
            int ug = 0;
            if (ch == '+')
            {
                tmp_stick_ang_z -= stick_ang_z + rand.Next(0, 13);
            }
            else if (ch == '-')
            {
                tmp_stick_ang_z += stick_ang_z + rand.Next(0, 13);
            }
            else if (ch == '^')
            {
                ug = rand.Next(-30, 30);

                if (ug < 0)
                {
                    tmp_stick_ang_z -= ug - 25;
                }
                else
                {
                    tmp_stick_ang_z += ug + 25;
                }
            }
            else if (ch == '[')
            {
                level += 1;
                stc.Add(tmp_branch);
            }
            else if (ch == ']')
            {
                level -= 1;
                tmp_branch = stc[stc.Count - 1];
                stc.RemoveAt(stc.Count - 1);

                pen_cor_x = tmp_branch.transform.position.x;
                pen_cor_y = tmp_branch.transform.position.y;
                tmp_stick_ang_z = tmp_branch.transform.eulerAngles.z;
            }
            else if (ch == '0')
            {
                Instantiate(leaf, new Vector3(pen_cor_x + (float)Math.Cos((90 + tmp_stick_ang_z) * Math.PI / 180), pen_cor_y + (float)Math.Sin((90 + tmp_stick_ang_z) * Math.PI / 180), 0f), Quaternion.identity).transform.parent = this.transform;
            }
            else if (rand.Next(2, 10) > 4)
            {
                float d_cor_x = (float)Math.Cos((90 + tmp_stick_ang_z) * Math.PI / 180);
                float d_cor_y = (float)Math.Sin((90 + tmp_stick_ang_z) * Math.PI / 180);

                float cor_x = (float)(pen_cor_x + d_cor_x);
                float cor_y = (float)(pen_cor_y + d_cor_y);
                tmp_branch = Instantiate(branch, new Vector3(cor_x, cor_y, 0f), Quaternion.Euler(0f, 0f, tmp_stick_ang_z));

                tmp_branch.transform.parent = this.transform;

                x = d_cor_x / 20;
                y = d_cor_y / 20;

                tmp_branch.transform.position = new Vector3(tmp_branch.transform.position.x - d_cor_x, tmp_branch.transform.position.y - d_cor_y);
                for (int i = 0; i < 20; i++)
                {
                    yield return new WaitForSeconds(speed_of_growth / 20f);
                    tmp_branch.transform.position += new Vector3(x, y);
                }

                pen_cor_x = pen_cor_x + d_cor_x;
                pen_cor_y = pen_cor_y + d_cor_y;
            }
        }
        StopCoroutine(GrowPlant());
    }
}
