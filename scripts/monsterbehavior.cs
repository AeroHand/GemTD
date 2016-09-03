using UnityEngine;
using System.Collections;

public class monsterbehavior : MonoBehaviour {
    
    private int pathnum;
    public float speed = 1.0F;

    public bool[] freeze;       //the freeze modifier
    public float[] freezetimer; //the indepent freeze timer for each level
    public int freezenum;       //the freeze modifiers number

    public float[] poisontimer;  //the poision modifier
    public int poisonpointer;
    public float poisondmg;

    private float startTime;
    private float journeyLength;
    //private Vector3 thispos;
    private Vector3 nextpos;
    public float health;
    public float fullhealth;
    maincharacontrol c2 = null;

    private GameObject gbar;
    private GameObject hbar;

    private bool deathflag;
    // Use this for initialization
    void Start () {
        freeze = new bool[8];

        freezetimer = new float[8];

        for (int i=1;i<=7;i++)   //initialize freeze modifier
        {
            freeze[i] = false;
            freezetimer[i] = 0;

        }
        poisontimer = new float[151];
        for (int i = 1; i <= 150; i++)  //initialize poison modifier
        {
            poisontimer[i] = 0;
        }
        poisonpointer = 0;

        //health = 80;
        pathnum = 1;
        speed = 0.1F;
        startTime = Time.time;
        c2 = GameObject.Find("avatar").GetComponent<maincharacontrol>();
        gbar = transform.Find("gbar").gameObject;
        hbar = transform.Find("healthbar").gameObject;
        deathflag = true;
        
    }
	
	// Update is called once per frame
	void Update () {
        //update health bar
        gbar.transform.localScale = new Vector3(health / fullhealth*0.8F, 0.101F,0.101F);
        gbar.transform.position = hbar.transform.position + Vector3.left * (fullhealth-health)/(fullhealth*2)*0.8F*2;
        
        
        //thispos = c2.basevector.transform.position+Vector3.right * c2.pathx[pathnum] * 2 + Vector3.back * c2.pathy[pathnum] * 2;
        //thispos = transform.position;
        nextpos = c2.basevector.transform.position+Vector3.right * c2.pathx[pathnum + 1] * 2 + Vector3.back * c2.pathy[pathnum + 1] * 2;

        //update freeze time
        freezenum = 0;
        for (int i=1;i<=7;i++)
        {
            if (freeze[i])
            {
                //check its timer
                freezetimer[i] -= Time.deltaTime;
                if (freezetimer[i] > 0)
                {
                    //still slow
                    freezenum += i;
                }
                else
                {
                    //defreeze
                    freeze[i] = false;
                }

            }
        }

        //update poison timer
        int ii = 1;
        poisondmg = 0;
        while (ii <= poisonpointer) {
            if (poisontimer[ii] <= 0)
            {
                //this poison has ended
                for (int j = ii; j < poisonpointer; j++) {
                    //move all the poisontimer a little bit forward
                    poisontimer[j] = poisontimer[j + 1];
                }
                poisonpointer--;
                

            }
            else
            {
                poisontimer[ii] -= Time.deltaTime;
                poisondmg += 1;
            }
            ii++;
        }
        health -= poisondmg * 0.02F;

        if ((health <= 0)&&(deathflag)) {
            deathflag = false;
            c2.monstercount -= 1;
            c2.endflag = true;
            c2.totalenergy += 5;
            c2.curenergy += 5;
            
            Destroy(this.gameObject);
            
        }

        if ((Vector3.Distance(transform.position,nextpos)<0.05))
        {
            pathnum += 1;

        }

        transform.position = Vector3.MoveTowards(transform.position, nextpos, speed-0.01F*freezenum);
        //Debug.Log(speed - 0.01F * freezenum);
        
	}
}
