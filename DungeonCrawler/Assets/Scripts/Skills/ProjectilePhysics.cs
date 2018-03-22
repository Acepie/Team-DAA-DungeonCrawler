using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour {

    public Vector3 startPos;
    public float maxTravelDistance;
    public AbstractCreature skillUser;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
     void Update()
    {
        if (Vector3.Distance(transform.position, startPos) > maxTravelDistance)
        {
            Destroy(this.gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if ((other.gameObject.CompareTag("Enemy")  || other.gameObject.CompareTag("Player")) && other.gameObject.tag != skillUser.tag)
        {
            other.gameObject.GetComponent<AbstractCreature>().UnderAttack(1);        
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);

        }
    }
}
