using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class maincharacontrol : MonoBehaviour
{
    //crafting list
    public int[][] craftlist;
    public int[][] craftcounter;

    //crafted towerlist
    public Object[] crafttower;

    //tower stats
    public int[][] toweratk;
    public int[][] toweratkspd;

    //creep stats
    public int[] creephp;
    public float[] creepspeed;

    //UI components
    public Text energyshow;
    public Text waveinfo;
    public Text towername;
    public Text towerlevel;
    public Text towerdescription;
    public Image towerimage;

    //UI abilities
    public Image buildbt;
    public Image up1bt;
    public Image up2bt;
    public Image craftbt;
    public Image eliminatebt;


    public string curgamestate;                 //状态机
    public int wavenumber = 1;                      //波数
    public int curgemcount;                     //本波宝石数量
    public int pathstartpoint;
    public Object[] gems = new Object[8];     //可出现宝石种类
    public Object wallunit;                     //墙
    public int curx = 0;                        //x，y 现有的xy
    public int cury = 0;
    public GameObject[][] mazegem = new GameObject[37][];
    public Object projectile;      //子弹

    private string[] basename;
    private string[] basedescription;

    private bool vyes;
    private bool yyes; 
    
    public Sprite[] baseimage=new Sprite[9];

    public int[][] mazedir = new int[37][];
    public int[][] mazerep = new int[37][];
    public int[][] mazelv = new int[37][];
    public int[][] mazefuture = new int[37][];

    //迷宫
    public int[] listx;                        //five gems built this round
    public int[] listy;
    public int listpointer = 0;

    public int[] oldx;                         //all the gems built in the past
    public int[] oldy;                         
    public int oldpointer = 0;

    public int[] pathx = new int[8000];       //correct path
    public int[] pathy = new int[8000];
    public int pathpointer;

    public int[] frontierx = new int[5000];   //helper for finding path
    public int[] frontiery = new int[5000];
    public int frontierpointer;

    private int pathindicator;
    private int holdpath;

    public float keytime = 0.01F;
    public float creeptime = 0.2F;
    private float nextkey = 0.0F;
    private float nextcreep = 0.0F;
    public int monsternum = 0;    //本波怪物数量

    public int monstercount = 0;
    public Object[] cachemonster = new Object[35];
    public GameObject[][] monsterarray = new GameObject[37][];

    public GameObject basevector;

    public GameObject birthvector;
    public bool endflag;

    public int totalenergy;  //whole energy
    public int curenergy;    //current energy
    public int energylevel;  //the level of energy

    // Use this for initialization
    void Start()
    {
        creephp = new int[27] {
            0,3,8,20,25,70,200,400,510,720,1200,1300,1500,1800,2300,3400,4200,1000,1800,2000,7500,6000,8000,4000,8000,3000,9000
        };

        //creepspeed = new float[10] {
        //};

        //1--grey
        //2--red
        //3--blue
        //4--yellow
        //5--diamond
        //6--jadeite
        //7--opal

        //basic tower info

        toweratk = new int[8][];
        toweratk[1] = new int[8] { 0, 2, 4, 8, 16, 32, 48,90};  //海晶石
        toweratk[2] = new int[8] { 0, 4, 8, 12, 16, 20,24 ,80};  //red
        toweratk[3] = new int[8] { 0, 2, 4, 6, 8, 12, 14, 20};   //blue
        toweratk[4] = new int[8] { 0, 3, 6, 9, 12, 15,18,80 };   //yellow
        toweratk[5] = new int[8] { 0, 6, 12, 18, 30, 50,80,100 };   //diamond
        toweratk[6] = new int[8] { 0, 2, 4, 8, 16, 24,32,48 };    //jadeite
        toweratk[7] = new int[8] { 0, 1, 2, 3, 4, 5, 6,7};

        //craftlist
        craftlist = new int[10][];

        craftlist[1] = new int[5] { 0, 21, 31, 41, 1 };

        craftcounter = new int[10][];

        craftcounter[1] = new int[5] { 0,0,0,0,0};

        totalenergy = 0;
        curenergy = 0;
        energylevel = 1;
        vyes = false;
        yyes = false;
        monstercount = 0;
        endflag = false;
        curgemcount = 5;
        pathindicator = 0;
        holdpath = 0;
        keytime = 0.05F;
        wavenumber = 1;
        creeptime = 1F;
        Time.timeScale = 0.5F;
        listx = new int[6];
        listy = new int[6];
        oldx = new int[500];
        oldy = new int[500];
        oldpointer = 0;
        birthvector = GameObject.Find("birthpoint");
        basevector = GameObject.Find("basepoint");

        for (int i = 0; i <= 36; i++)
        {
            mazegem[i] = new GameObject[37];
            mazedir[i] = new int[37];
            mazerep[i] = new int[37];
            mazelv[i] = new int[37];
            mazefuture[i] = new int[37];
            monsterarray[i] = new GameObject[10];
            for (int j = 0; j <= 36; j++)
            {
                mazerep[i][j] = 0;
                mazelv[i][j] = 0;
                mazefuture[i][j] = 0;
            }
        }

        //预载入所有怪物
        for (int i = 1; i <= 5; i++)
        {
            //Debug.Log("JellyMonsters/Prefabs/Jelly" + i.ToString());
            cachemonster[i] = Resources.Load("JellyMonsters/Prefabs/Jelly" + i.ToString());
        }


        //预载入所有宝石
        //1--grey
        //2--red
        //3--blue
        //4--yellow
        //5--diamond
        //6--jadeite
        //7--opal
        
        basename = new string[8] {"Empty",
        "Grey Gem",
            "Red Gem",
            "Blue Gem",
            "Yellow Gem",
            "Diamond",
            "Jadeite",
            "Opal"
        };

        basedescription = new string[8] {" Don't bother. Nothing is here.",
            " Dim but deadly fast XD. \n Ability: Fast attack Speed",
            " Shining red, could cause small shockwave \n when attacks. \n Ability: Splash",
            " Frozen touches. \n Ability: Slow",
            " Glorious gold. \n Ability: Multiple attack",
            " Unbelievable solid. \n Ability: High attack dmg",
            " Like snakes. \n Ability: Poison touch",
            " Gentle feeSl. \n Ability: Attack speed aura" };
        //baseimage = new Sprite[8];
        //baseimage[0] = Resources.Load("gemimage/0") as Sprite;
        //towerimage.sprite = baseimage[0];
        for (int i = 1; i <= 7; i++)
        {
            //gems[i] = new Object[7];
            //for (int j = 1; j < 7; j++)
            //{
            gems[i] = Resources.Load("basegem/" + i.ToString());
            //baseimage[i] = Resources.Load("gemimage/" + i.ToString()) as Sprite;

            //}
        }

        //预载入子弹
        projectile = Resources.Load("projectile");

        wallunit = Resources.Load("wallunit");
        curgamestate = "preparing";

        //create some walls
        for (int i = 0; i < 4; i++)
        {
            createwall(i, 18);
            createwall(i + 33, 18);
            createwall(18, i);
            createwall(18, i + 32);
        }
        createwall(18, 36);
        waveinfo.text = "Next wave: " + wavenumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //UI button update

        if ((curgamestate=="preparing") || (curgamestate == "thinking"))
        {
            buildbt.GetComponent<CanvasRenderer>().SetAlpha(1);
        }
        else
        {
            buildbt.GetComponent<CanvasRenderer>().SetAlpha(0);
        }

        if ((mazerep[curx][cury]==-1) && (curgamestate != "attacking") )
        {
            eliminatebt.GetComponent<CanvasRenderer>().SetAlpha(1);
            if (Input.GetKey("x"))
            {
                Destroy(mazegem[curx][cury]);
                mazegem[curx][cury] = null;
                mazedir[curx][cury] = 0;
                mazerep[curx][cury] = 0;
                mazelv[curx][cury] = 0;
            }
        }
        else
        {
            eliminatebt.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
        vyes = false;
        yyes = false;
        if (curgamestate == "thinking") {
            
            bool checkhere = false;
            for (int i = 0; i < listpointer; i++) {
                if ((listx[i]== curx) && (listy[i]==cury))
                {
                    checkhere = true;
                }
            }

            if (checkhere)
            {
                

                float counter = 0f;
                for (int i = 0; i < listpointer; i++)
                {
                    if (mazerep[listx[i]][listy[i]] == mazerep[curx][cury]) {
                        counter += Mathf.Pow(2,(mazelv[listx[i]][listy[i]] - mazelv[curx][cury]));
                        
                    }
                }
                //counter = Mathf.Log(counter, 2);
                if ((counter == 8) || (counter ==4) || (counter == 5)) {
                    //could do Y operation
                    //Debug.Log(counter);
                    yyes = true;
                    vyes = true;
                }
                if ((counter==2)||(counter==3))
                {
                    //could do V operation
                    vyes = true;
                }

            }
        }

        if ((curgamestate == "thinking") && vyes)
        {
            up1bt.GetComponent<CanvasRenderer>().SetAlpha(1);
            if (Input.GetKey("v")) {
                for (int i = 0; i < listpointer; i++)
                {
                    if (!(listx[i] == curx && listy[i] == cury))
                    {
                        //replace this gem with wall
                        GameObject tempwall = Instantiate(wallunit, mazegem[listx[i]][listy[i]].transform.position, transform.rotation) as GameObject;
                        Object.Destroy(mazegem[listx[i]][listy[i]], 0);
                        mazegem[listx[i]][listy[i]] = tempwall;
                        mazerep[listx[i]][listy[i]] = -1;
                        mazelv[listx[i]][listy[i]] = 0;
                        //mazedir[curx][cury] = -1;
                    }
                }
                //upgrade current gem one level
                towercontrol temptower = mazegem[curx][cury].GetComponent<towercontrol>();
                mazelv[curx][cury] += 1;
                temptower.atkdmg = toweratk[mazerep[curx][cury]][mazelv[curx][cury]];
                listpointer = 0;
                curgamestate = "attacking";
                //计算怪物行进路线
                pathindicator = 1;
                holdpath = 1;
                //刷怪
                waveinfo.text = "Now defending: wave " + wavenumber.ToString();
                //update old lists of gems
                oldpointer += 1;
                oldx[oldpointer] = curx;
                oldy[oldpointer] = cury;
                checkmazefuture(mazelv[curx][cury]+mazerep[curx][cury]*10);
            }
        }
        else {
            up1bt.GetComponent<CanvasRenderer>().SetAlpha(0);
        }

        if ((curgamestate == "thinking") && yyes)
        {
            up2bt.GetComponent<CanvasRenderer>().SetAlpha(1);
            if (Input.GetKey("y"))
            {
                for (int i = 0; i < listpointer; i++)
                {
                    if (!(listx[i] == curx && listy[i] == cury))
                    {
                        //replace this gem with wall
                        GameObject tempwall = Instantiate(wallunit, mazegem[listx[i]][listy[i]].transform.position, transform.rotation) as GameObject;
                        Object.Destroy(mazegem[listx[i]][listy[i]], 0);
                        mazegem[listx[i]][listy[i]] = tempwall;
                        mazerep[listx[i]][listy[i]] = -1;
                        mazelv[listx[i]][listy[i]] = 0;
                        //mazedir[curx][cury] = -1;
                    }
                }
                //upgrade current gem one level
                towercontrol temptower = mazegem[curx][cury].GetComponent<towercontrol>();
                mazelv[curx][cury] += 2;
                temptower.atkdmg = toweratk[mazerep[curx][cury]][mazelv[curx][cury]];
                listpointer = 0;
                curgamestate = "attacking";
                //计算怪物行进路线
                pathindicator = 1;
                holdpath = 1;
                //刷怪
                waveinfo.text = "Now defending: wave " + wavenumber.ToString();
                //update old lists of gems
                oldpointer += 1;
                oldx[oldpointer] = curx;
                oldy[oldpointer] = cury;
                checkmazefuture(mazelv[curx][cury] + mazerep[curx][cury] * 10);
            }
        }
        else
        {
            up2bt.GetComponent<CanvasRenderer>().SetAlpha(0);
        }

        if ((curgamestate == "attacking")&&(mazefuture[curx][cury]!=0)) {
            craftbt.GetComponent<CanvasRenderer>().SetAlpha(1);

            if (Input.GetKey("c"))
            {
                
                
                GameObject tempgem = Instantiate(crafttower[mazefuture[curx][cury]], mazegem[curx][cury].transform.position, transform.rotation) as GameObject;
                Destroy(mazegem[curx][cury]);
                mazegem[curx][cury] = tempgem;
                mazefuture[curx][cury] = 0;
                mazerep[curx][cury] = 11;
                mazelv[curx][cury] = 1;
            }

        }
        else
        {
            craftbt.GetComponent<CanvasRenderer>().SetAlpha(0);
        }



        energylevel = Mathf.FloorToInt(totalenergy / 100F) + 1;
        energyshow.text = "Energy:" + curenergy.ToString() + "/" + totalenergy.ToString();
        if (energylevel > 5)
        {
            energylevel = 5;
        }

        if (curgamestate == "attacking" && monstercount <= 0 && endflag && monsternum <= 0)
        {
            //start new round
            endflag = false;
            pathindicator = 0;
            holdpath = 0;
            wavenumber += 1;
            curgamestate = "preparing";
            curgemcount = 5;
            waveinfo.text = "Next wave: " + wavenumber.ToString();
        }
        switch (pathindicator)
        {
            case 1:
                pathindicator = 0;
                pathpointer = 0;
                pathstartpoint = 1;
                cleanmaze();
                //generate first path
                mazedir[4][4] = 5;
                frontierpointer = 1;
                frontierx[1] = 4;
                frontiery[1] = 4;
                StartCoroutine(bfs(4, 4, 4, 18, 1));
                break;
            case 2:
                pathstartpoint = pathpointer + 1;
                cleanmaze();
                mazedir[4][18] = 5;
                frontierpointer = 1;
                frontierx[1] = 4;
                frontiery[1] = 18;
                StartCoroutine(bfs(4, 18, 32, 18, 2));
                break;
            case 3:
                pathstartpoint = pathpointer + 1;
                cleanmaze();
                mazedir[32][18] = 5;
                frontierpointer = 1;
                frontierx[1] = 32;
                frontiery[1] = 18;
                StartCoroutine(bfs(32, 18, 32, 4, 3));
                break;
            case 4:
                pathstartpoint = pathpointer + 1;
                cleanmaze();
                mazedir[32][4] = 5;
                frontierpointer = 1;
                frontierx[1] = 32;
                frontiery[1] = 4;
                StartCoroutine(bfs(32, 4, 18, 4, 4));
                break;
            case 5:
                pathstartpoint = pathpointer + 1;
                cleanmaze();
                mazedir[18][4] = 5;
                frontierpointer = 1;
                frontierx[1] = 18;
                frontiery[1] = 4;
                StartCoroutine(bfs(18, 4, 18, 31, 5));
                break;

            case 6:
                pathstartpoint = pathpointer + 1;
                cleanmaze();
                mazedir[18][31] = 5;
                frontierpointer = 1;
                frontierx[1] = 18;
                frontiery[1] = 31;
                StartCoroutine(bfs(18, 31, 32, 31, 6));
                break;

            case 7:
                //testprintpath();
                if (wavenumber % 10 == 0)
                {
                    //boss wave, just one creep spawned
                    monsternum = 1;
                }
                else
                {
                    monsternum = 10;
                }
                monstercount = 0;
                pathindicator += 1;
                break;
            default:; break;

        }
        if ((monsternum > 0) && (Time.time > nextcreep))
        {
            //刷一个怪
            nextcreep = Time.time + creeptime;
            monsterarray[wavenumber][10 - monsternum] = Instantiate(cachemonster[1], birthvector.transform.position, birthvector.transform.rotation) as GameObject;
            monsterarray[wavenumber][10 - monsternum].tag = "enemy";
            monsterbehavior tempmonster = monsterarray[wavenumber][10 - monsternum].GetComponent<monsterbehavior>();
            tempmonster.health = creephp[wavenumber];
            tempmonster.fullhealth= creephp[wavenumber];
            monsternum -= 1;
            monstercount += 1;
        }
        if ((Input.GetKey("up") || Input.GetKey("w")) && cury > 0 && Time.time > nextkey)
        {
            nextkey = Time.time + keytime;
            cury -= 1;
            this.transform.position += Vector3.forward * 2.0F;

        }

        if ((Input.GetKey("down") || Input.GetKey("s")) && cury < 36 && Time.time > nextkey)
        {
            nextkey = Time.time + keytime;
            cury += 1;
            this.transform.position += Vector3.back * 2.0F;

        }

        if ((Input.GetKey("left") || Input.GetKey("a")) && curx > 0 && Time.time > nextkey)
        {
            nextkey = Time.time + keytime;
            curx -= 1;
            this.transform.position += Vector3.left * 2.0F;

        }
        if ((Input.GetKey("right") || Input.GetKey("d")) && curx < 36 && Time.time > nextkey)
        {
            nextkey = Time.time + keytime;
            curx += 1;
            this.transform.position += Vector3.right * 2.0F;

        }
        if (Input.GetKeyDown("b") && curgamestate == "thinking")
        {
            //Debug.Log("here!");
            //消除4个宝石
            if (!(mazegem[curx][cury] == null))
            {
                for (int i = 0; i < listpointer; i++)
                {
                    if (!(listx[i] == curx && listy[i] == cury))
                    {
                        //replace this gem with wall
                        GameObject tempwall = Instantiate(wallunit, mazegem[listx[i]][listy[i]].transform.position, transform.rotation) as GameObject;
                        Object.Destroy(mazegem[listx[i]][listy[i]], 0);
                        mazegem[listx[i]][listy[i]] = tempwall;
                        mazerep[listx[i]][listy[i]] = -1;
                        mazelv[listx[i]][listy[i]] = 0;
                        //mazedir[curx][cury] = -1;
                    }
                }
                listpointer = 0;
                curgamestate = "attacking";
                //计算怪物行进路线
                pathindicator = 1;
                holdpath = 1;
                //刷怪
                waveinfo.text = "Now defending: wave " + wavenumber.ToString();
                //update old lists of gems
                oldpointer += 1;
                oldx[oldpointer] = curx;
                oldy[oldpointer] = cury;
                checkmazefuture(mazelv[curx][cury] + mazerep[curx][cury] * 10);

            }
        }

        if (Input.GetKeyDown("b") && curgamestate == "preparing")
        {
            if ((mazegem[curx][cury] == null) && checkpathcorner())
            {
                //造塔
                
                int tempgemnum = Mathf.FloorToInt(Random.Range(1.0f, 7.999f));
                
                mazegem[curx][cury] = Instantiate(gems[tempgemnum], this.transform.position, this.transform.rotation) as GameObject;
                mazerep[curx][cury] = tempgemnum;

                int tempgemlevelpossibility = Mathf.RoundToInt(Random.Range(0.6f, 10.4f));

                int tempgemlevel;

                switch (energylevel)
                {
                    case 1: tempgemlevel = 1; break;
                    case 2:
                        if (tempgemlevelpossibility <= 8)
                        {
                            tempgemlevel = 1;
                        }
                        else
                        {
                            tempgemlevel = 2;
                        }
                        break;
                    case 3:
                        if (tempgemlevelpossibility <= 6)
                        {
                            tempgemlevel = 1;
                        }
                        else
                        {
                            if (tempgemlevelpossibility <= 8)
                            {
                                tempgemlevel = 2;
                            }
                            else
                            {
                                tempgemlevel = 3;
                            }
                        }
                        break;
                    case 4:
                        if (tempgemlevelpossibility <= 4)
                        {
                            tempgemlevel = 1;
                        }
                        else
                        {
                            if (tempgemlevelpossibility <= 7)
                            {
                                tempgemlevel = 2;
                            }
                            else
                            {
                                if (tempgemlevelpossibility <= 9)
                                {
                                    tempgemlevel = 3;
                                }
                                else
                                {
                                    tempgemlevel = 4;
                                }
                            }
                        }
                        break;
                    case 5:
                        if (tempgemlevelpossibility <= 3)
                        {
                            tempgemlevel = 1;
                        }
                        else
                        {
                            if (tempgemlevelpossibility <= 5)
                            {
                                tempgemlevel = 2;
                            }
                            else
                            {
                                if (tempgemlevelpossibility <= 7)
                                {
                                    tempgemlevel = 3;
                                }
                                else
                                {
                                    if (tempgemlevelpossibility <= 9)
                                    {
                                        tempgemlevel = 4;
                                    }
                                    else
                                    {
                                        tempgemlevel = 5;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        Debug.Log("u r screwed in energylv stuff");
                        Debug.Log(tempgemlevelpossibility);
                        tempgemlevel = 1;
                        break;
                }

                towercontrol temptower = mazegem[curx][cury].GetComponent<towercontrol>();
                temptower.atkdmg = toweratk[tempgemnum][tempgemlevel];
                if (tempgemnum == 1)
                {
                    temptower.shotspeed = 0.1f;
                }
                else {
                    temptower.shotspeed = 0.5f;
                }

                mazelv[curx][cury] = tempgemlevel;
                mazedir[curx][cury] = -1;

                listx[listpointer] = curx;
                listy[listpointer] = cury;
                listpointer += 1;

                curgemcount -= 1;
                if (curgemcount == 0)
                {
                    //finish building state
                    curgamestate = "thinking";
                }
            }
        }

        //UI update stuff

        switch (mazerep[curx][cury])
        {
            case -1:
                towername.text = "Wall";
                towerlevel.text = "";
                towerimage.GetComponent<Image>().sprite = baseimage[8];
                
                towerdescription.text = "An unless wall just to block those stupid creeps.";
                break;
            default:
                towername.text = basename[mazerep[curx][cury]];
                towerlevel.text = mazelv[curx][cury].ToString();
                towerdescription.text = basedescription[mazerep[curx][cury]];
                //Debug.Log(mazerep[curx][cury]);
                towerimage.GetComponent<Image>().sprite = baseimage[mazerep[curx][cury]];
                
                break;
        }



    }


    private void checkmazefuture(int curtowertoken)
    {
        //check if any upgraded tower could be fulfilled
        for (int i = 1; i <= 1; i++)
        {
            //int tempcount = 0;
            bool craftyes=true;
            for (int j = 1; j <= 3; j++)
            {
                if (craftlist[i][j] == curtowertoken)
                {
                    craftcounter[i][j] += 1;
                }
                if (craftcounter[i][j] == 0) {
                    craftyes = false;
                }
            }
            if (craftyes) {
                //change all the future value
                //todo
                mazefuture[curx][cury] = craftlist[i][4];
            }

        }
        //if any change their mazefuture value
    }


    private bool checkpathcorner()
    {
        if (((curx == 4) && (cury == 18)) || ((curx == 4) && (cury == 4)) || ((curx == 32) && (cury == 18)) || ((curx == 32) && (cury == 4)) || ((curx == 18) && (cury == 4)) || ((curx == 18) && (cury == 31)) || ((curx == 32) && (cury == 31)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private void testprintpath()
    {
        Debug.Log("pathlength");
        Debug.Log(pathpointer);
        for (int i = 1; i <= pathpointer; i++)
        {
            Instantiate(wallunit, basevector.transform.position + Vector3.right * pathx[i] * 2 + Vector3.back * pathy[i] * 2, basevector.transform.rotation);

            Debug.Log("x " + pathx[i].ToString() + " y " + pathy[i].ToString());
        }
    }

    private void cleanmaze()
    {
        for (int i = 0; i <= 36; i++)
        {
            for (int j = 0; j <= 36; j++)
            {
                if (mazedir[i][j] == -1)
                {
                    mazedir[i][j] = -1;
                }
                else
                {
                    mazedir[i][j] = 0;
                }
            }
        }

    }

    public void createwall(int wx, int wy)
    {
        GameObject tempwall = Instantiate(wallunit, basevector.transform.position + Vector3.right * wx * 2 + Vector3.back * wy * 2, transform.rotation) as GameObject;
        mazegem[wx][wy] = tempwall;
        mazedir[wx][wy] = -1;
        mazerep[wx][wy] = -1;
        mazelv[wx][wy] = 0;
    }

    private void checkvalid(int ma, int mb, int dir)
    {
        if ((ma >= 0) && (ma <= 36) && (mb >= 0) && (mb <= 36))
        {
            if (mazedir[ma][mb] == 0)
            {
                mazedir[ma][mb] = dir;
                frontierpointer += 1;
                frontierx[frontierpointer] = ma;
                frontiery[frontierpointer] = mb;


            }
        }
        return;
    }

    private void pa()
    {
        for (int i = 0; i <= 36; i++)
        {
            string temps = i.ToString();
            for (int j = 0; j <= 36; j++)
            {
                temps = temps + " " + mazedir[i][j].ToString();
            }
            Debug.Log(temps);
        }
    }

    IEnumerator bfs(int nowx, int nowy, int destx, int desty, int timechecked)
    {
        while (frontierpointer > 0)
        {

            //pop出现在的坐标
            int nx = frontierx[1];
            int ny = frontiery[1];

            //如果是终点
            if ((nx == destx) && (ny == desty))
            {
                int rx = nx;
                int ry = ny;
                //pa();
                while (!((rx == nowx) && (ry == nowy)))
                {
                    //Debug.Log("path");
                    //Debug.Log(rx);
                    //Debug.Log(ry);
                    //Debug.Log(mazedir[rx][ry]);
                    switch (mazedir[rx][ry])
                    {
                        case 1: rx += 1; break;
                        case 2: rx -= 1; break;
                        case 3: ry += 1; break;
                        case 4: ry -= 1; break;
                        default: Debug.Log("u r screwed!"); break;
                    }

                    for (int j = pathpointer; j >= pathstartpoint; j--)
                    {
                        pathx[j + 1] = pathx[j];
                        pathy[j + 1] = pathy[j];

                    }
                    pathpointer += 1;
                    pathx[pathstartpoint] = rx;
                    pathy[pathstartpoint] = ry;


                }

                holdpath += 1;
                pathindicator = holdpath;
                frontierpointer = 0;
                //yield return null;
            }

            //消除现有坐标
            for (int j = 1; j < frontierpointer; j++)
            {
                frontierx[j] = frontierx[j + 1];
                frontiery[j] = frontiery[j + 1];
            }
            frontierpointer -= 1;

            //this is a valid grid
            checkvalid(nx - 1, ny, 1);
            checkvalid(nx + 1, ny, 2);
            checkvalid(nx, ny - 1, 3);
            checkvalid(nx, ny + 1, 4);

            //yield return null;
        }

        yield return null;
    }


}
