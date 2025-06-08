using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] enemy;
    public Transform[] spawnPos;
    
    
    public void StartSpawn(int spawnCount, float duration)
    {
        StartCoroutine(SpawnEnemy(spawnCount, duration));
    }
    private IEnumerator SpawnEnemy(int spawnCount, float duration)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int j = Random.Range(0, spawnPos.Length);
            int a = Random.Range(0, enemy.Length);
            Instantiate(enemy[a], spawnPos[j].position, Quaternion.identity);
            yield return new WaitForSeconds(duration);
        }
    }
}
