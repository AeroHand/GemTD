using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    public GameObject targetenemy;
    private monsterbehavior tempscript;

    public AudioClip piu;
    public AudioClip pa;
    public AudioSource noise1;
    public Object fireex;

    public float prospeed;

    public int atkdmg;

    public int frozen;  //freeze ability

    public int poison;  //poision ability
    public float poisontime;

    public bool fast = false;
    public Object fastparticle;
    public Object freezeparticle;
    public Object poisonparticle;
    //public Object fastparticle;


    // Use this for initialization
    void Start () {
        //atkdmg = 10;
        //frozen = 0;
        prospeed = 0.2F;

        tempscript = targetenemy.GetComponent<monsterbehavior>();
        fireex=Resources.Load("PyroParticles/Prefab/Prefab/FireExplosion");

        noise1 = GetComponent<AudioSource>();

        noise1.PlayOneShot(piu, 1F);

    }

    // Update is called once per frame
    void Update () {
        if (!(targetenemy == null))
        {
            if (Vector3.Distance(targetenemy.transform.position, this.transform.position) > 0.7)
            {

                transform.position = Vector3.MoveTowards(transform.position, targetenemy.transform.position, prospeed);

                

            }
            else
            {

                noise1.PlayOneShot(pa, 1F);
                Destroy(this.gameObject);
                tempscript.health -= atkdmg;
                if (frozen > 0)
                {
                    tempscript.freeze[frozen] = true;
                    tempscript.freezetimer[frozen] = 3F;
                    Destroy(Instantiate(freezeparticle, this.transform.position, this.transform.rotation), 0.5f);
                }
                if (poison >0) {
                    Destroy(Instantiate(poisonparticle, this.transform.position, this.transform.rotation), 0.5f);
                }

                if (fast) {
                    Destroy(Instantiate(fastparticle, this.transform.position, this.transform.rotation), 0.1f);
                }

                for (int i = 1; i <= poison; i++)
                {
                    //add one stack of poison modifier
                    tempscript.poisonpointer += 1;
                    Debug.Log(tempscript.poisonpointer);
                    tempscript.poisontimer[tempscript.poisonpointer] = poisontime;
                }

                if (Random.Range(1f, 5f) > 4.2F)
                {
                    //Instantiate(fireex, transform.position, transform.rotation);
                }
            }
        }
        else
        {
            Destroy(this.gameObject);

        }
	}
}
