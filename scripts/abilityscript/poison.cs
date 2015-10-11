using UnityEngine;
using System.Collections;

public class poison : MonoBehaviour {

    public int plevel;
    public float ptime;
	// Use this for initialization
	void Start () {
        //for test
        plevel = 4;


        ptime = 5;
        towercontrol temptower = this.GetComponent<towercontrol>();
        temptower.poisonlevel = plevel;
        temptower.poisontime = ptime;                                                

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
