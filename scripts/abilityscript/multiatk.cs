using UnityEngine;
using System.Collections;

public class multiatk : MonoBehaviour {

    // Use this for initialization
    public int multilevel;

    void Start () {
        //for test
        multilevel = 5;
        //

        towercontrol temptower = this.GetComponent<towercontrol>();
        temptower.multishot = true;         //set the multi ability on
        temptower.multimax = multilevel;    //set how many targets could be shot at the same time
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
