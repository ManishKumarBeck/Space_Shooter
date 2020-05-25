using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Sprite[] _liveSprites;

    //private bool _isAmmoEmpty = false;

    private GameManager _gameManager;
    private AudioSource _ammoIndicatorAudio;


    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _ammoIndicatorAudio = _ammoText.GetComponent<AudioSource>();


        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

        if(_ammoIndicatorAudio == null)
        {
            Debug.LogError("Ammo Indiactor Audio Source is NULL");
        }
    }



    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
    }

    public void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }
    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void AmmoIndiactor(bool ammoEmpty)
    {
        if (ammoEmpty == true)
        {
            _ammoIndicatorAudio.Play();
            StartCoroutine(AmmoFlicker(2.0f));
        }
    }

    IEnumerator AmmoFlicker(float waitTime)
    {
        while (Time.time > waitTime)
        {
            waitTime += Time.time;
            _ammoText.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _ammoText.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
