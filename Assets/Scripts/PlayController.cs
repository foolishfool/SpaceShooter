﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour {
    //speed of spaceship
    public float speed = 10.0f;

    //the range of player can move
    private float xMin = -4.1f;
    private float xMax = 4.1f;
    private float yMin = -1.6f;
    private float yMax = 14f;

    public GameObject bullet;
    //the position of generating bullet
    public Transform spawnPos;
   
    //the rate of emit bullet , one second 4 bullets
    [Header("bullets interval time") ]
    public float fireRate  = 0.25f;
    //next time of emitting bullet
    private float nextFire;

    public AudioClip fireAudio;
  

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bullet, spawnPos.position, spawnPos.rotation);
            GetComponent<AudioSource>().PlayOneShot(fireAudio);
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //arrow of buttons in keyboard
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, v, 0f);
        GetComponent<Rigidbody>().velocity = speed*move;

        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, xMin, xMax),
            Mathf.Clamp(GetComponent<Rigidbody>().position.y, yMin, yMax),
            0
            );

    }
}
