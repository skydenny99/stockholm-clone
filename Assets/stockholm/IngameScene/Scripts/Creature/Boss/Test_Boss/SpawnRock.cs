using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRock : MonoBehaviour, BossPattern
{

    public string patternName = "Spawn Rock";
    public GameObject rock;
    public float spawnHeight = 10f;
    public float spawnTime = 3f;
    public float spawnDelay = 1f;
    public float spawnInterval = 0.1f;

    private bool patternStarted_ = false;
    private GameObject player_;
    private Coroutine spawnRockCoroutine_;
    private int spawnCount = 0;

    public string getPatternName()
    {
        return patternName;
    }

    public void run()
    {
        patternStarted_ = true;
        spawnRockCoroutine_ = StartCoroutine(spawnCoroutine());
    }

    void Start()
    {
        spawnCount = (int)(spawnTime / spawnInterval);
        player_ = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
    }

   

    void spawnRock()
    {
        Vector3 spawnPos = new Vector3(player_.transform.position.x, spawnHeight);
        GameObject g = Instantiate(rock, spawnPos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform.root);
        Destroy(g, 3);
        //TODO : Projectile 상속 후 구현
    }

    public IEnumerator spawnCoroutine()
    {
        if(patternStarted_)
        {
            patternStarted_ = false;
            yield return new WaitForSeconds(spawnDelay);
        }
        for(int i = 0; i < spawnCount; i++)
        {
            spawnRock();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
}
