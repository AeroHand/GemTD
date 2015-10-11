using UnityEngine;
using System.Collections;

public class towercontrol : MonoBehaviour
{

    public bool multishot; //multi atk
    //public int multitarget;
    public int freeze;     //freeze ability

    public int poisonlevel; //poison ability
    public float poisontime;


    public GameObject[] target = new GameObject[10];
    public int multinum;
    public int multimax;
    public float shotspeed;
    public float shottimer;
    maincharacontrol c2 = null;

    // Use this for initialization
    void Start()
    {
        multinum = 0;
        multishot = false;
        multimax = 1;
        freeze = 0;

        shotspeed = 0.5F;
        shottimer = Time.time;
        c2 = GameObject.Find("avatar").GetComponent<maincharacontrol>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((shottimer + shotspeed) <= Time.time) && (multinum>0) && (!((target[1]) == null)) )
        {
            //shoot
            for (int i = 1; i <= multimax; i++)
            {
                if (!(target[i] == null))
                {
                    shottimer = Time.time;
                    GameObject temp = (Instantiate(c2.projectile, this.transform.position, this.transform.rotation)) as GameObject;
                    bullet tempbullet = temp.GetComponent<bullet>();
                    tempbullet.targetenemy = target[i];
                    tempbullet.frozen = freeze;

                    tempbullet.poison = poisonlevel;
                    tempbullet.poisontime = poisontime;
                }
            }
        }


        if (((target[1])==null)&&(!((target[2]) == null))) {
            for (int j = 1; j < multinum; j++)
            {
                target[j] = target[j + 1];
            }
            multinum -= 1;
        }
    

    }

    void OnTriggerEnter(Collider other)
    {
        
        
            //Debug.Log(other.gameObject.tag);
            if (other.gameObject.tag == "enemy")
            {
                multinum += 1;
                
                target[multinum] = other.gameObject;
            }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            for (int i = 1; i <= multinum; i++)
            {
                if (target[i] == other.gameObject)
                {
                    for (int j = i; j < multinum; j++)
                    {
                        target[j] = target[j + 1];
                    }
                    multinum -= 1;
                }
            }
        }
    }
}
