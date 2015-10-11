using UnityEngine;
using System.Collections;

public class freeze : MonoBehaviour {

    public int flevel;
	// Use this for initialization
	void Start () {
        //for test
        flevel =5;
        //

        towercontrol temptower = this.GetComponent<towercontrol>();
        temptower.freeze = flevel;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
