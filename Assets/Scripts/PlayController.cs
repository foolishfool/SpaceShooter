using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour {

    public float speed = 10.0f;

    //the range of player can move
    private float xMin = -4.1f;
    private float xMax = 4.1f;
    private float yMin = -1.6f;
    private float yMax = 8f;



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
