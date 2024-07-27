using UnityEngine;
using UnityEngine.UI;

public class TanksHandler : MonoBehaviour
{
    [SerializeField] private AudioClip enemyDeadClip;
    [SerializeField] private AudioClip windClip;
    [SerializeField] private AudioClip tanksClip;
    [SerializeField] private AudioClip[] deadClip;
    [SerializeField] private AudioClip[] shotClip;
    [Range(0f, 1f)][SerializeField] private float clipVolume;



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


        loseText.text = loseText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", loseScoreImpact.ToString());
        winText.text = winText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", scoreImpact.ToString());

        _aimRb = aim.GetComponent<Rigidbody2D>();

        AudioSource windAudioSource = SoundManager.instance.PlayAudioClip(tanksClip, transform, clipVolume, false);
        windAudioSource.loop = true;
        AudioSource tanksAudioSource = SoundManager.instance.PlayAudioClip(windClip, transform, clipVolume, false);
        tanksAudioSource.loop = true;
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
        SoundManager.instance.PlayAudioClip(shotClip, transform, clipVolume);
        hpSlider.value -= damage;
        if (hpSlider.value <= 0)
        {
            _active = false;
            loseCanvas.SetActive(true);
            SoundManager.instance.PlayAudioClip(deadClip, transform, clipVolume);
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeScore(-loseScoreImpact);

            return true;
        }
        return false;
    }

    public bool IncreaseScore()
    {
        SoundManager.instance.PlayAudioClip(enemyDeadClip, transform, clipVolume);
        SoundManager.instance.PlayAudioClip(deadClip, transform, clipVolume);
        score += 1;
        scoreText.text = $"{score}/{winScore}";
        if (score >= winScore) {
            _active = false;
            winCanvas.SetActive(true);
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeScore(scoreImpact);

            return true;
        }
        return false;
    }
}
