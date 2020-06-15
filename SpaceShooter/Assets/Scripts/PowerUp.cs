using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _pickupClip;
    [SerializeField]
    private float _clipVolume = 40f;
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if(transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                AudioSource.PlayClipAtPoint(_pickupClip, transform.position, _clipVolume);
                switch (_powerupID)
                {
                    case 0: player.TripleShotActive();
                        break;
                    case 1: player.SpeedBoost();
                        break;
                    case 2: player.ShieldActive();
                        break;
                    case 3: player.AmmoReload();
                        break;
                    case 4: player.HealthPickup();
                         break;
                    case 5: player.HunterDroneActive();
                        break;
                    case 6: player.SpeedReduce();
                        break;
                    default:
                        Debug.Log("default");
                        break;
                }

                
            }
            Destroy(this.gameObject);
        }
    }
}
