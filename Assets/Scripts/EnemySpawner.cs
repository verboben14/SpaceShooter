using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // configuration parameters
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWaveIndex = 0;
    [SerializeField] bool isLooping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (isLooping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWaveIndex; waveIndex < waveConfigs.Count; waveIndex++)
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig wave)
    {
        for (int currentEnemyIndex = 0; currentEnemyIndex < wave.NumberOfEnemies; currentEnemyIndex++)
        {
            var newEnemy = Instantiate(wave.EnemyPrefab, wave.GetPathWaypoints()[0].position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(wave);
            yield return new WaitForSeconds(wave.TimeBetweenSpawns);
        }
    }
}
