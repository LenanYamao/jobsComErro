using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gm;
    public List<Transform> spawnPoints;
    public List<Transform> farmerPoints;

    public Transform player;

    public void spawn(GameObject enemy, bool farmer = false)
    {
        int rand = Random.Range(0, spawnPoints.Count);

        var spawned = Instantiate(enemy, spawnPoints[rand].position, Quaternion.identity);
        if (!farmer)
        {
            spawned.GetComponent<AiEntity>().target = player;
            spawned.GetComponent<AiEntity>().gm = gm;
        }
        else
        {
            int target = Random.Range(0, farmerPoints.Count);
            spawned.GetComponent<AiEntity>().target = farmerPoints[target];
            spawned.GetComponent<AiEntity>().gm = gm;
        }

    }
}
