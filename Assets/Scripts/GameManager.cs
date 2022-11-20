using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int wave = 0;
    public int spawnAmmount = 25;
    public int spawnAdd = 5;
    public float spawnTime = 30f;

    public bool useJob = false;

    public Spawner spawner;
    public GameObject farmer;
    public GameObject knight;
    public GameObject crusader;
    public GameObject player;

    public TextMeshProUGUI textScore;

    private void Start()
    {
        StartCoroutine(coroutineSpawn());
    }
    public void AddScore(int value = 100)
    {
        score += value;
    }
    void Update()
    {
        textScore.text = score.ToString();
        if (player == null)
        {
            StopCoroutine("coroutineSpawn");
            StartCoroutine(gameOver());
        }
    }

    public IEnumerator coroutineSpawn()
    {
        while (true)
        {
            wave++;

            int spawns = spawnAmmount + (spawnAdd * wave);

            for (int i = 0; i < spawns; i++)
            {
                int type = Random.Range(0, 100);

                if (type <= 10)
                {
                    spawner.spawn(farmer, true);
                }
                else if(type > 10 && type <= 75)
                {
                    spawner.spawn(knight);
                }
                else
                {
                    spawner.spawn(crusader);
                }
                yield return new WaitForSeconds(.1f);
            }

            yield return new WaitForSeconds(spawnTime);
            if(spawnTime > 10)
            {
                spawnTime -= 2;
            }
        }
    }
    IEnumerator gameOver()
    {
        Score.totalScore = score;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");
    }
}
