using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
    public static TokenSpawner instance {  get; private set; }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    public GameObject tokenPrefab;

    public void SpawnTokens(float minPos, float maxPos)
    {
        int random = Random.Range(0, 101);
        // 10% chance of spawning a token.
        if(random <= 10)
        {
            float tokenLocation = Random.Range(minPos, maxPos);
            int randomLane = Random.Range(0, 3);
            Instantiate(tokenPrefab, new Vector3(PlayerMovement.instance.Lanes[randomLane].position.x, 1.8f, tokenLocation), Quaternion.identity);
        }
    }
}
