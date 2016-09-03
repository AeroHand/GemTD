using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class final : MonoBehaviour {

    public Text life;
    private int wholehealth;
    public Object baozha;
    maincharacontrol c2 = null;
    // Use this for initialization
    void Start () {

        wholehealth = 20;
        life.text = "Your health: "+wholehealth.ToString();
        c2 = GameObject.Find("avatar").GetComponent<maincharacontrol>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {



        if (other.gameObject.tag == "enemy")
        {
            wholehealth--;
            life.text = "Your health: " + wholehealth.ToString();
            Destroy(Instantiate(baozha, other.transform.position, this.transform.rotation),2);
            Destroy(other.gameObject);
            c2.monstercount -= 1;
            c2.endflag = true;
            if (wholehealth<=0)
            {
                Application.LoadLevel("losts");
            }
        }

    }
}
