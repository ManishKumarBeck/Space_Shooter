using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(PowerUpRoutine());
        
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            int randomEnemy = Random.Range(0, 2);
            float randomX = Random.Range(-9f, 9f);

            Vector3 posToSpawn = new Vector3(randomX, 8f, 0f);
            
            GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], posToSpawn,Quaternion.identity);            
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(4f);
        }        
    }

    IEnumerator PowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9f, 9f);
            Vector3 posToSpawn = new Vector3(randomX, 8f, 0f);
            int randomPowerup = Random.Range(0, 6);
            Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 9f));
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    
}
