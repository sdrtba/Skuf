using UnityEngine;

public class TVHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip tvClip;
    [Range(0, 1)][SerializeField] private float tvClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private float increaseTime;
    [Range(0, 100)][SerializeField] private int hungerByScoreImpact;

    private bool _isActive = false;
    private AudioSource _tvAudioSource;
    private float _defSpeed;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        _defSpeed = increaseTime;

        _tvAudioSource = SoundManager.instance.PlayAudioClip(tvClip, transform, tvClipVolume, false);
        _tvAudioSource.loop = true;
        _tvAudioSource.Stop();
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;
        if (_isActive )
        {
            _tvAudioSource.Play();
        }
        else
        {
            _tvAudioSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (_isActive && SkufHandler.instance.hunger >= SkufHandler.instance.maxHunger*0.66)
        {
            increaseTime -= Time.deltaTime;
            if (increaseTime < 0)
            {
                SkufHandler.instance.ChangeHunger(-hungerByScoreImpact);
                SkufHandler.instance.ChangeScore(1);
                increaseTime = _defSpeed;
                YG.YandexGame.SaveProgress();
            }
        }
    }
}
