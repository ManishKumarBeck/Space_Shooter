using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _originalSpeed = 5f;
    [SerializeField]
    private float _increasedSpeed = 7.5f;
    [SerializeField]
    private float _increasedSpeedTimer = 500f;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _hunterDronePrefab;
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject _thrusters;
    [SerializeField]
    private GameObject[] _engineDamage;   
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    [SerializeField]
    private AudioClip _laserSound;    
    



    private int _shieldStrength = 3;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isAmmoCollected = false;
    private bool _isHunterDroneActive = false;

    private float _canFire = -1f;
    private float _speedMultiplier = 2f;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioScource;
    private CameraShake _cameraShake;

    Renderer _shieldRendered;
    Renderer _thrusterRenderer;




    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioScource = GetComponent<AudioSource>();
        _shieldRendered = _shieldVisualiser.GetComponent<Renderer>();
        _thrusterRenderer = _thrusters.GetComponent<Renderer>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_shieldRendered == null)
        {
            Debug.LogError("Shield Rendere not found");
        }
        if(_thrusterRenderer == null)
        {
            Debug.LogError("Thruster Rendere not found");
        }
        if(_cameraShake == null)
        {
            Debug.LogError("Camera shake is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_audioScource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
           _audioScource.clip = _laserSound;
        }

        _uiManager.UpdateAmmo(_ammoCount);
    }

    void Update()
    {


        ClaculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedBoostActive == false)
        {
            if (_increasedSpeedTimer > 1)
            {
                _speed = _increasedSpeed;
                _uiManager.ThrusterVisualisation(_increasedSpeedTimer);
                _thrusterRenderer.material.color = Color.blue;
            }
            else
            {
                _thrusterRenderer.material.color = Color.white;
            }
        }
        else if (_isSpeedBoostActive == true)
        {
            _thrusterRenderer.material.color = Color.green;
        }
        else
        {
            _speed = _originalSpeed;
            _thrusterRenderer.material.color = Color.white;
        }

        
    }

    void ClaculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);


        float clamp = Mathf.Clamp(transform.position.y, -3.9f, 0f);
        transform.position = new Vector3(transform.position.x, clamp, 0);


        if (transform.position.x > 11.2f)
        {
            transform.position = new Vector3(-11.2f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.2f)
        {
            transform.position = new Vector3(11.2f, transform.position.y, 0);
        }
    }

    private void FixedUpdate()
    {
        if (_speed == _increasedSpeed)
        {
            if (_increasedSpeedTimer >= 2)
            {
                _increasedSpeedTimer -= 2;
            }
            else
            {
                _speed = _originalSpeed;
            }
        }

        else if (_increasedSpeedTimer < 500 && !Input.GetKey(KeyCode.LeftShift))
        {
            _increasedSpeedTimer += 1;
        }
        _uiManager.ThrusterVisualisation(_increasedSpeedTimer);
    }
   

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
            
        }
        else if(_isHunterDroneActive == true)
        {
            Instantiate(_hunterDronePrefab, transform.position, Quaternion.identity);
        }   
        else if (_ammoCount > 0)
        {
            _ammoCount--;
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }
        else
        {
            Debug.Log("Ammo not present");
            _uiManager.AmmoIndiactor(true);
        }

        if (_ammoCount > 0)
        {
            _audioScource.Play();
        }
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {

            _shieldStrength--;

            if (_shieldStrength == 2)
            {
                _shieldRendered.material.color = Color.yellow;
                return;
            }
            if (_shieldStrength == 1)
            {
                _shieldRendered.material.color = Color.red;
                return;
            }
            else
            {
                _shieldVisualiser.SetActive(false);
                _isShieldActive = false;
                return;
            }
        }


        _lives--;
        _uiManager.UpdateLives(_lives);
        StartCoroutine(_cameraShake.Shake(0.15f, 0.4f));
      
        EngineDamage();

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOverSequence();

            Destroy(this.gameObject);
        }
    }

    public void EngineDamage()
    {
        if(_lives == 3)
        {
            _engineDamage[0].SetActive(false);
            _engineDamage[1].SetActive(false);
        }
        if (_lives == 2)
        {
            _engineDamage[0].SetActive(true);
            _engineDamage[1].SetActive(false);
        }
        if (_lives == 1)
        {
            _engineDamage[1].SetActive(true);
        }

    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void HunterDroneActive()
    {
        _isHunterDroneActive = true;
        _isTripleShotActive = false;
        StartCoroutine(HomingMissilePowerDownRoutine());
    }

    IEnumerator HomingMissilePowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isHunterDroneActive = false;
    }
    public void SpeedBoost()
    {
        _originalSpeed *= _speedMultiplier;
        _speed = _originalSpeed;        
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _originalSpeed /= _speedMultiplier;
        _speed = _originalSpeed;
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualiser.SetActive(true);
        _shieldStrength = 3;
        _shieldRendered.material.color = Color.white;

    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoReload()
    {
        _isAmmoCollected = true;
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
        Debug.Log("Ammo collected");
    }

    public void HealthPickup()
    {
        if(_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            EngineDamage();
        }
    }

}
