using UnityEngine;
using System.Collections;

public class maincharacontrol : MonoBehaviour
{

    public string curgamestate;                 //状态机
    public int wavenumber = 1;                      //波数
    public int curgemcount;                     //本波宝石数量
    public int pathstartpoint;
    public Object[][] gems = new Object[7][];     //可出现宝石种类
    public Object wallunit;                     //墙
    public int curx = 0;                        //x，y 现有的xy
    public int cury = 0;
    public GameObject[][] mazegem = new GameObject[37][];
    public Object projectile;      //子弹

    public int[][] mazedir = new int[37][];
    //迷宫
    public int[] listx;
    public int[] listy;
    public int listpointer = 0;

    public int[] pathx = new int[8000];
    public int[] pathy = new int[8000];
    public int pathpointer;

    public int[] frontierx = new int[5000];
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

    // Use this for initialization
    void Start()
    {
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
        birthvector = GameObject.Find("birthpoint");
        basevector = GameObject.Find("basepoint");

        for (int i = 0; i <= 36; i++)
        {
            mazegem[i] = new GameObject[37];
            mazedir[i] = new int[37];
            monsterarray[i] = new GameObject[10];
        }

        //预载入所有怪物
        for (int i = 1; i <= 5; i++)
        {
            //Debug.Log("JellyMonsters/Prefabs/Jelly" + i.ToString());
            cachemonster[i] = Resources.Load("JellyMonsters/Prefabs/Jelly" + i.ToString());
        }
        //预载入所有宝石
        for (int i = 1; i <= 1; i++)
        {
            gems[i] = new Object[7];
            for (int j = 1; j < 7; j++)
            {
                gems[i][j] = Resources.Load("gem/grey" + j.ToString());

            }
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
    }

    // Update is called once per frame
    void Update()
    {
        if (curgamestate == "attacking" && monstercount <= 0 && endflag && monsternum<=0)
        {
            //start new round
            endflag = false;
            pathindicator = 0;
            holdpath = 0;
            wavenumber += 1;
            curgamestate = "preparing";
            curgemcount = 5;
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
                //holdpath += 1;
                //pathindicator = holdpath;
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
                monsternum = 10;
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
                        GameObject tempwall = Instantiate(wallunit, mazegem[listx[i]][listy[i]].transform.position, mazegem[listx[i]][listy[i]].transform.rotation) as GameObject;
                        Object.Destroy(mazegem[listx[i]][listy[i]], 0);
                        mazegem[curx][cury] = tempwall;
                        mazedir[curx][cury] = -1;
                    }
                }
                listpointer = 0;
                curgamestate = "attacking";
                //计算怪物行进路线
                pathindicator = 1;
                holdpath = 1;
                //刷怪



            }
        }

        if (Input.GetKeyDown("b") && curgamestate == "preparing")
        {
            if ((mazegem[curx][cury] == null) && checkpathcorner())
            {
                //造塔
                int tempgem = 1;
                int tempgemnum = Mathf.RoundToInt(Random.Range(1.0f, 5.0f));

                mazegem[curx][cury] = Instantiate(gems[tempgem][tempgemnum], this.transform.position, this.transform.rotation) as GameObject;

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
