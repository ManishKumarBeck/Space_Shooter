﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 40f;
    [SerializeField]
    private GameObject _explosionPrefab;
    

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.forward *  _rotationSpeed *Time.deltaTime);       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag ==  "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject,0.2f);
        }
    }
}
