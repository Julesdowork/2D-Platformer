using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string name;
    public Transform enemy;
    public int count;
    public float rate;
}

public class WaveSpawner : MonoBehaviour {

    public Wave[] waves;
    public float timeBetweenWaves = 5f;
    public enum SpawnState { Spawning, Waiting, Counting };
    public Transform[] spawnPoints;
    public SpawnState State
    {
        get { return state; }
    }
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    private int nextWave = 0;
    private SpawnState state = SpawnState.Counting;
    private float searchCountdown = 1f;
    private float waveCountdown;

    // Use this for initialization
    void Start () {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }
		if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
	}

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }

        return true;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed!");
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        nextWave++;
        if (nextWave >= waves.Length)
        {
            nextWave = 0;
            Debug.Log("All Waves Complete! Looping...");
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning wave: " + wave.name);
        state = SpawnState.Spawning;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemy(Transform enemy)
    {
        Debug.Log("Spawning enemy: " + enemy.name);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
