using UnityEngine;

public class TVHandler : MonoBehaviour
{
    [SerializeField] private AudioClip tvClip;
    [Range(0f, 1f)][SerializeField] private float tvClipVolume;
    [Range(0f, 100f)][SerializeField] private float speed;
    [Range(0, 100)][SerializeField] private int hungerByScoreImpact;
    private AudioSource audioSource;
    private bool _isActive = false;
    private float _defSpeed;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        _defSpeed = speed;

        audioSource = SoundManager.instance.PlayAudioClip(tvClip, transform, tvClipVolume, false);
        audioSource.loop = true;
        audioSource.Stop();
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;
        if (_isActive )
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (_isActive && SkufHandler.instance.hunger >= SkufHandler.instance.maxHunger*0.66)
        {
            speed -= Time.deltaTime;
            if (speed < 0)
            {
                SkufHandler.instance.ChangeHunger(-hungerByScoreImpact);
                SkufHandler.instance.ChangeScore(1);
                speed = _defSpeed;
            }
        }
    }
}
