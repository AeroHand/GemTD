using UnityEngine;
using System.Collections;

public class gemshow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f);
    }
}
