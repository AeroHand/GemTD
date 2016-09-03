using UnityEngine;
using System.Collections;

public class choose : MonoBehaviour {

	// Use this for initialization
    private Vector3[] twopos;
    private int curstate;
    private float keytime;
	void Start () {
        curstate = 0;
        keytime = Time.time;
        twopos = new Vector3[2];
        twopos[0] = this.transform.position;
        twopos[1] = this.transform.position + Vector3.back * 18;
    }
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("down") || Input.GetKey("s")) && ((keytime+.2)<=Time.time)) {
            keytime = Time.time;
            curstate = 1 - curstate;
            this.transform.position = twopos[curstate];
        }

        if (Input.GetKey("space") || Input.GetKey("enter")) {
            if (curstate == 0)
            {
                Application.LoadLevel("test1");
            }
            else {
                Application.Quit();
            }
        } 

    }
}
