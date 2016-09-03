using UnityEngine;
using System.Collections;

public class lost : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("r"))
        {
            Application.LoadLevel("test1");
        }
    }
}
