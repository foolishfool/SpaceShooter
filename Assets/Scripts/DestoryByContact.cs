using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByContact : MonoBehaviour
{
    //explosion of effects
    public GameObject playerExplosion;
    public GameObject explosion;

    private GameController gamecontroller;

    public int score;

    private void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("GameController");

        if ( gameControllerObj!= null)
        {
            gamecontroller = gameControllerObj.GetComponent<GameController>();
        }
        if (gamecontroller == null)
        {
            Debug.Log("Cannot find GameController");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
            gamecontroller.GameOver();
        }
        //destroy player
        Destroy(other.gameObject);
        //destory asteroids
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
        gamecontroller.addScore(score);
    }
}
