using System;
using UnityEngine;
using UnityEngine.UI;


public class TanksHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip enemyDeadClip;
    [Range(0, 1)][SerializeField] private float enemyDeadClipVolume;
    [SerializeField] private AudioClip enemyShotClip;
    [Range(0, 1)][SerializeField] private float enemyShotClipVolume;
    [SerializeField] private AudioClip deadClip;
    [Range(0, 1)][SerializeField] private float deadClipVolume;
    [SerializeField] private AudioClip windClip;
    [Range(0, 1)][SerializeField] private float windClipVolume;
    [SerializeField] private AudioClip tanksClip;
    [Range(0, 1)][SerializeField] private float tanksClipVolume;

    [Header("Coefficients")]
    [Range(0, 5)][SerializeField] public int maxTankPerTime;
    [Range(0, 100)][SerializeField] private float aimSpeed;
    [Range(0, 1)][SerializeField] private float damage;
    [Range(0, 100)][SerializeField] private int winScore;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int scoreImpact;
    [Range(0, 100)][SerializeField] private int loseScoreImpact;

    [Header("System")]
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject aim;
    [SerializeField] private Text loseText;
    [SerializeField] private Text winText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Slider hpSlider;

    [NonSerialized] public int tanksCount = 0;
    private bool _isActive = true;
    private int score = 0;
    private AudioSource _tanksAudioSource;
    private Rigidbody2D _aimRb;
    private float _horizontal;
    private float _vertical;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);

        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);
        else
        {
            _aimRb = aim.GetComponent<Rigidbody2D>();

            AudioSource windAudioSource = SoundManager.instance.PlayAudioClip(windClip, transform, windClipVolume, false);
            windAudioSource.loop = true;

            _tanksAudioSource = SoundManager.instance.PlayAudioClip(tanksClip, transform, tanksClipVolume, false);
            _tanksAudioSource.loop = true;
        }
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal") * 100;
        _vertical = Input.GetAxisRaw("Vertical") * 100;

        if (_isActive)
            _aimRb.AddForce(new Vector3(_horizontal, _vertical, 0).normalized * aimSpeed);
    }

    public bool GetDamage()
    {
        SoundManager.instance.PlayAudioClip(enemyShotClip, transform, enemyShotClipVolume);
        hpSlider.value -= damage;
        if (hpSlider.value <= 0)
        {
            _isActive = false;
            _tanksAudioSource.Stop();

            SoundManager.instance.PlayAudioClip(deadClip, transform, deadClipVolume);
            SkufHandler.instance.SetText(loseText, hungerImpact.ToString(), loseScoreImpact.ToString());
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeScore(-loseScoreImpact);
            loseCanvas.SetActive(true);

            return true;
        }
        return false;
    }

    public bool IncreaseScore()
    {
        score += 1;
        scoreText.text = $"{score}/{winScore}";
        SoundManager.instance.PlayAudioClip(enemyDeadClip, transform, enemyDeadClipVolume);

        if (score >= winScore) {
            _isActive = false;

            SkufHandler.instance.SetText(winText, hungerImpact.ToString(), scoreImpact.ToString());
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeScore(scoreImpact);
            winCanvas.SetActive(true);

            return true;
        }
        return false;
    }
}
