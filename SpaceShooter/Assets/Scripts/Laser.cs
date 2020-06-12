using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private bool _isEnemyLaser = false;
    [SerializeField]
    private bool _isDiagonalLaser = false;

    void Update()
    {
        if(_isEnemyLaser == true || _isDiagonalLaser == true)
        {
            MoveDown();
        }
        
        else
        {
            MoveUp();            
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        Destroy();
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime );
        Destroy();
    }

    void Destroy()
    {
        if(transform.position.y > 8f || transform.position.y < -8f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void AssignDiagonalEnemyLaser()
    {
        _isDiagonalLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" )
        {
            if (_isEnemyLaser == true || _isDiagonalLaser == true)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }
                Destroy(this.gameObject);
            }
        }
    }
}
