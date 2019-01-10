using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public float Xmin;
    public float Xmax;
    public float Y = 15f;
    //the number of hazard in each group
    public int waveNum = 10;
    //wait time for the first group of hazards
    private float waitTime = 4f;
    //interval time for each hazard in each hazards group
    private float spawnTime = 1f;
    //interval time for each group
    private float nextTime = 4f;

    private JumpingNumberTextComponent textjumperNumber;

    private int score = 0;
    private bool gameOver;

    public Text gameOverText;
    public Text helpText;

    void Start()
    {
        StartCoroutine(spawnWaves());
        gameOverText.text = "";
        helpText.text = "";
  

        GameObject textJumperObj = GameObject.FindWithTag("TextJumper");

        if (textJumperObj != null)
        {
            textjumperNumber = textJumperObj.GetComponent<JumpingNumberTextComponent>();
        }
        if (textjumperNumber == null)
        {
            Debug.Log("Cannot find JumpingNumberTextComponent");
        }
    }

    // Update is called once per frame
    void Update () {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

    }

    IEnumerator spawnWaves()
    {
        yield return new WaitForSeconds(waitTime);

        while (true)
        {
            for (int i = 0; i < waveNum; i++)
            {
                onSpawn();
                yield return new WaitForSeconds(spawnTime);
            }

            yield return new WaitForSeconds(nextTime);

            if (gameOver)
            {
                break;
            }
        }
    
    }

    void onSpawn()
    {
        GameObject go = hazards[Random.Range(0, hazards.Length - 1)];
        Vector3 p = new Vector3(Random.Range(Xmin, Xmax), Y, 0);
        Instantiate(go, p, Quaternion.identity);
    }

    public void addScore(int v)
    {
        int currentScore = score;
        score += v;
        //play animation
        textjumperNumber.Change(currentScore, score);
    }

    public void GameOver()
    {
        gameOver = true ;
        gameOverText.text = "GAME OVER !";
        helpText.text = "PRESS R TO RESTART";
    }
}
