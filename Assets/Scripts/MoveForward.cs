using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

    public float speed = 20;
 
	// Use this for initialization
	void Start () {

        GetComponent<Rigidbody>().velocity = transform.up * speed;
	}

}
