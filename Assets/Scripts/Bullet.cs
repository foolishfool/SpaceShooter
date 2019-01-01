using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 20;
 
	// Use this for initialization
	void Start () {

        GetComponent<Rigidbody>().velocity = transform.up * speed;
	}

    private void Update()
    {
        if (gameObject.transform.position.y > 15.0f)
        {
            Destroy(gameObject);
        }
    }

}
