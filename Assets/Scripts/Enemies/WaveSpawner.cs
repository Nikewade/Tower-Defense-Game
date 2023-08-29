using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public WaveData[] waves;
    public int curWave = 0;

    public int remainingEnemies;

    [Header("Components")]
    public Transform enemySpawnPos;
    public TextMeshProUGUI waveText;
    public GameObject nextWaveButton;

    private void OnEnable()
    {
        Enemy.OnDestroyed += OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnDestroyed -= OnEnemyDestroyed;
    }

    private void Start()
    {
        waveText.text = $"Wave: {curWave + 1}";
    }

    public void SpawnNextWave()
    {
        curWave++;

        if (curWave - 1 == waves.Length)
            return;

        waveText.text = $"Wave: {curWave}";

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        nextWaveButton.SetActive(false);
        WaveData wave = waves[curWave - 1];

        for (int x = 0; x < wave.enemySets.Length; x++)
        {
            for (int y = 0; y < wave.enemySets[x].spawnCount; y++)
            {
                remainingEnemies++;
            }
        }

        for (int x = 0; x < wave.enemySets.Length; x++)
        {
            // Spawn Delay
            yield return new WaitForSeconds(wave.enemySets[x].spawnDelay);

            for (int y = 0; y < wave.enemySets[x].spawnCount; y++)
            {
                SpawnEnemy(wave.enemySets[x].enemyPrefab);
                // Spawn Rate
                yield return new WaitForSeconds(wave.enemySets[x].spawnRate);
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, enemySpawnPos.position, Quaternion.identity);
    }

    public void OnEnemyDestroyed()
    {
        if(remainingEnemies == 0)
            nextWaveButton.SetActive(true);
    }
}
