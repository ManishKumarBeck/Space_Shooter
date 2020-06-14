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
    [SerializeField]
    private bool _isDiagonalMovementEnable = false;
    [SerializeField]
    private int _angle = 45;
   

    private bool _isDestroyed = false;
    private GameObject _target;

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

         _angle = AssignAngle();
        

    }

    void Update()
    {
        if (_isDiagonalMovementEnable == false)
        {
            CalculateMovement();
        }
        if(_isDiagonalMovementEnable == true)
        {            
            DiagonalMovement(_angle);
        }
        if(Time.time > _canFire && _isDestroyed == false)
        {
            _fireRate = Random.Range(3f, 8f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            if (lasers == null)
            {
                Debug.Log("Laser is null");
            }
            for (int i = 0; i < lasers.Length; i++)
            {
                if (_isDiagonalMovementEnable == false)
                {
                    lasers[i].AssignEnemyLaser();
                    Debug.Log("Down Laser");
                }
                if (_isDiagonalMovementEnable == true)
                {
                    lasers[i].AssignDiagonalEnemyLaser();
                    Debug.Log("Diagonal Laser");
                }

            }            

        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        CheckRange();

        if (transform.position.y < -4.5 && _isDestroyed == true)
        {
            Debug.Log("It's called");
            Destroy(this.gameObject);
        }
    }

    void DiagonalMovement(int angle)
    {
      
        transform.rotation = Quaternion.AngleAxis(angle,new Vector3(0,0,1));
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        CheckRange();
       
    }

    void CheckRange()
    {
        if (transform.position.y < -5.5f)
        {
            if (_isDiagonalMovementEnable == false)
            {
                Respawn();
            }

            else
            {
                Destroy(this.gameObject);
            }
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

    private int AssignAngle()
    {

        if (transform.position.x < 0)
        {
            _angle = _angle * 1;
        }
        if (transform.position.x > 0)
        {
            _angle = _angle * -1;
        }


        return _angle;
    }

    public bool DestructionCheck()
    {
        _isDestroyed = true;
        return true;
    }

    
}
