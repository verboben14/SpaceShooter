using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenEnemySpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    public GameObject EnemyPrefab { get { return enemyPrefab; } }
    public float TimeBetweenSpawns { get { return timeBetweenEnemySpawns; } }
    public float SpawnRandomFactor { get { return spawnRandomFactor; } }
    public int NumberOfEnemies { get { return numberOfEnemies; } }
    public float MoveSpeed { get { return moveSpeed; } }

    public List<Transform> GetPathWaypoints()
    {
        var pathWaypoints = new List<Transform>();

        foreach (Transform pathWaypoint in pathPrefab.transform)
        {
            pathWaypoints.Add(pathWaypoint);
        }

        return pathWaypoints;
    }
}
