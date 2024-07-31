using UnityEngine;
using UnityEngine.UI;

public class TanksHandler : MonoBehaviour
{
    [SerializeField] private AudioClip enemyDeadClip;
    [Range(0f, 1f)][SerializeField] private float enemyDeadClipVolume;
    [SerializeField] private AudioClip enemyShotClip;
    [Range(0f, 1f)][SerializeField] private float enemyShotClipVolume;

    [SerializeField] private AudioClip deadClip;
    [Range(0f, 1f)][SerializeField] private float deadClipVolume;

    [SerializeField] private AudioClip windClip;
    [Range(0f, 1f)][SerializeField] private float windClipVolume;
    [SerializeField] private AudioClip tanksClip;
    [Range(0f, 1f)][SerializeField] private float tanksClipVolume;
    private AudioSource _tanksAudioSource;


    [SerializeField] private Text loseText;
    [SerializeField] private Text winText;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject aim;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text scoreText;
    [Range(0f, 100f)][SerializeField] private float damage;
    [Range(0f, 1000f)][SerializeField] private float aimSpeed;
    [Range(0, 100)][SerializeField] private int winScore;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int scoreImpact;
    [Range(0, 100)][SerializeField] private int loseScoreImpact;
    private Rigidbody2D _aimRb;
    private float _horizontal;
    private float _vertical;
    private bool _active = true;
    private int score = 0;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);

        if (SkufHandler.instance.hunger <= 0)
        {
            TankScript._tankActive = false;
            hungerCanvas.SetActive(true);
        }
        

        _aimRb = aim.GetComponent<Rigidbody2D>();

        AudioSource windAudioSource = SoundManager.instance.PlayAudioClip(tanksClip, transform, tanksClipVolume, false);
        windAudioSource.loop = true;
        _tanksAudioSource = SoundManager.instance.PlayAudioClip(windClip, transform, windClipVolume, false);
        _tanksAudioSource.loop = true;
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (_active)
            _aimRb.AddForce(new Vector3(_horizontal * aimSpeed * Time.deltaTime, _vertical * aimSpeed * Time.deltaTime, 0));
    }

    public bool GetDamage()
    {
        SoundManager.instance.PlayAudioClip(enemyShotClip, transform, enemyShotClipVolume);
        hpSlider.value -= damage;
        if (hpSlider.value <= 0)
        {
            _active = false;

            loseText.text = loseText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", loseScoreImpact.ToString());

            loseCanvas.SetActive(true);
            SoundManager.instance.PlayAudioClip(deadClip, transform, deadClipVolume);
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeScore(-loseScoreImpact);

            _tanksAudioSource.Stop();

            return true;
        }
        return false;
    }

    public bool IncreaseScore()
    {
        SoundManager.instance.PlayAudioClip(enemyDeadClip, transform, enemyDeadClipVolume);
        score += 1;
        scoreText.text = $"{score}/{winScore}";
        if (score >= winScore) {
            _active = false;

            winText.text = winText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", scoreImpact.ToString());

            winCanvas.SetActive(true);
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeScore(scoreImpact);

            return true;
        }
        return false;
    }
}
