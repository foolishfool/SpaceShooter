using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByContact : MonoBehaviour
{
    //explosion of effects
    public GameObject playerExplosion;
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
        }
        //destroy player
        Destroy(other.gameObject);
        //destory asteroids
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
