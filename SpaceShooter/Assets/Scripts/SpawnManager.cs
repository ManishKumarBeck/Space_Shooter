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
    [SerializeField]
    private float _spawnSpeed = 4f;
    [SerializeField]
    private float _gameTime; 

    private bool _stopSpawning = false;

  
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(PowerUpRoutine());
        
    }

    private void Update()
    {
        _gameTime = Time.time;  //just for testing purpose on inspector
    }

    IEnumerator SpawnEnemyRoutine()
    {        
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            UpdateSpawnSpeed();
            int randomEnemy = Random.Range(0, 2);
            float randomX = Random.Range(-9f, 9f);

            Vector3 posToSpawn = new Vector3(randomX, 8f, 0f);
            
            GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], posToSpawn,Quaternion.identity);            
            newEnemy.transform.parent = _enemyContainer.transform;
            

            yield return new WaitForSeconds(_spawnSpeed);
        }        
    }

    IEnumerator PowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9f, 9f);
            Vector3 posToSpawn = new Vector3(randomX, 8f, 0f);
            int randomPowerup = Random.Range(0, 7);
            Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 9f));
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    void UpdateSpawnSpeed()
    {
        if(_gameTime >= 30)
        {
            _spawnSpeed = 3f;           
        }

        if(_gameTime >= 50)
        {
            _spawnSpeed = 2f;
        }

        if(_gameTime >= 70)
        {
            _spawnSpeed = 1f;
        }
    }
}
