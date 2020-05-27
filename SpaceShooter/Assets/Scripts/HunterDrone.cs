using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterDrone : MonoBehaviour
{

    [SerializeField]    
    private float rotationSpeed = 200f;
    [SerializeField]
    private float _speed = 5f;
   
    private float startTime;

    
    private Transform _home;
    private Transform _target;
    private Transform _enemyTarget;
    private Rigidbody2D _rigidBody;
    
      
    private void Start()
    {               
        _rigidBody = GetComponent<Rigidbody2D>();
        _home = GameObject.FindGameObjectWithTag("Player").transform;
        if(_home == null)
        {
            Debug.LogError("Home is NULL");
        }

        startTime = Time.time;
    }

    private void Update()
    {
        
        _enemyTarget = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (_enemyTarget == null)
        {
            _target = _home;
        }
        else
        {
            _target = _enemyTarget;
        }
      
        if(_target == _home)
        {
            if(_enemyTarget != null)
            {
                _target = _enemyTarget;
            }
            else
            {
                _target = _home;
            }
        }
        CheckTime();

    }

    void FixedUpdate()
    {
        if(_target == null)
        {
            CheckTime();
            _target = _home;
            return;
        }
        Vector2 direction = (Vector2)_target.position - _rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rigidBody.angularVelocity = -rotationSpeed * rotateAmount;
        _rigidBody.velocity = transform.up * _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy" )
        {
            Destroy(this.gameObject);
        }
    }

    void CheckTime()
    {
        if((Time.time-startTime) > 10f)
        {
        
            Destroy(this.gameObject);
        }
        else
        {
            return;
        }
    }
    
   
}
