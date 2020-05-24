using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private int _pointsOfEnemy = 10;
    [SerializeField]
    private GameObject _laserPrefab;
    
    private bool _isDestroyed = false;

    private float _fireRate = 4.0f;
    private float _canFire = -1;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }


    }

    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 8f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            if(lasers == null)
            {
                Debug.Log("Laser is null");
            }
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            Respawn();
        }

        if (transform.position.y < -4.5 && _isDestroyed == true)
        {
            Debug.Log("It's called");
            Destroy(this.gameObject);
        }
    }


    void Respawn()
    {
        transform.position = new Vector3(Random.Range(-9.4f, 9.4f), 8f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Laser")
        {            
            Destroy(other.gameObject);
            _player.AddScore(_pointsOfEnemy);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.2f;
            DestructionCheck();
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.20f);
        }

        if (other.tag == "Player")
        {            
            if(_player != null)
            {
                _player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.2f;
            DestructionCheck();
            _audioSource.Play();
            
            Destroy(this.gameObject, 2.20f);
        }
    }

    private void DestructionCheck()
    {
        _isDestroyed = true;
    }
}
