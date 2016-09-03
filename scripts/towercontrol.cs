using UnityEngine;
using System.Collections;

public class towercontrol : MonoBehaviour
{

    public bool multishot; //multi atk
    //public int multitarget;
    public int freeze;     //freeze ability

    public int poisonlevel; //poison ability
    public float poisontime;

    public int atkdmg;
    

    public GameObject[] target = new GameObject[100];
    public int multinum;   //the number of targets that are in the range
    public int multimax;   //the number of targets that allow to be shot
    public float shotspeed;
    public float shottimer;
    maincharacontrol c2 = null;
    public bool fast;
    // Use this for initialization
    void Start()
    {
        multinum = 0;
        multishot = false;
        multimax = 1;
        freeze = 0;

        //shotspeed = 0.5F;
        shottimer = Time.time;
        c2 = GameObject.Find("avatar").GetComponent<maincharacontrol>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((shottimer + shotspeed) <= Time.time))
        {
            //shoot
            for (int i = 1; i <= multimax; i++)
            {
                //Debug.Log(target[i].name);
                if (!(target[i] == null))
                {
                    
                    //if ((Vector3.Distance(this.transform.position, target[i].transform.position)) < 28f)
                    //{ 
                    shottimer = Time.time;
                    GameObject temp = (Instantiate(c2.projectile, this.transform.position, this.transform.rotation)) as GameObject;
                    bullet tempbullet = temp.GetComponent<bullet>();
                    tempbullet.targetenemy = target[i];
                    tempbullet.frozen = freeze;
                    tempbullet.fast = fast;
                    tempbullet.poison = poisonlevel;
                    tempbullet.poisontime = poisontime;

                    tempbullet.atkdmg = atkdmg;
                    
                    //}
                    //else
                    //{
                    //    for (int j = i; j < multinum; j++)
                    //    {
                    //        target[j] = target[j + 1];
                    //    }
                    //    target[multinum] = null;
                    //    multinum -= 1;

                    //}

                }
                else
                {
                    if ((target[i + 1] != null))
                    {
                        //Debug.Log(i);
                        for (int j = i; j < multinum; j++)
                        {
                            target[j] = target[j + 1];
                        }
                        target[multinum] = null;
                        multinum -= 1;
                    }
                }
            }
        }





    }

    void OnTriggerEnter(Collider other)
    {



        if (other.gameObject.tag == "enemy")
        {
            //Debug.Log(other.gameObject.tag);
            multinum += 1;

            target[multinum] = other.gameObject;
            //Debug.Log(multinum);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            //Debug.Log("leaving one enemy");
            for (int i = 1; i <= multinum; i++)
            {
                if (target[i] == other.gameObject)
                {
                    for (int j = i; j < multinum; j++)
                    {
                        target[j] = target[j + 1];
                    }
                    target[multinum] = null;
                    multinum -= 1;
                }
            }
        }
    }
}
