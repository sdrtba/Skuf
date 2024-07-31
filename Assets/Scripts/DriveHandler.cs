using UnityEngine;
using UnityEngine.UI;

public class DriveHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip backClip;
    [Range(0, 1)][SerializeField] private float backClipVolume;
    [SerializeField] private AudioClip driveClip;
    [Range(0, 1)][SerializeField] private float driveClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private int moneyImpact;
    [Range(0, 100)][SerializeField] private int hungerImpact;

    [Header("System")]
    [SerializeField] private GameObject[] unactiveObjects;
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject driver;
    [SerializeField] private Text doneText;
    [SerializeField] private Slider slider;

    private bool _isDrive = false;
    private Animator _driveAnimator;
    private Animation _backgroundAnimation;
    private AudioSource _audioSource;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);

        _driveAnimator = driver.gameObject.GetComponent<Animator>();
        _backgroundAnimation = backGround.GetComponent<Animation>();

        _audioSource = SoundManager.instance.PlayAudioClip(driveClip, transform, driveClipVolume, false);
        _audioSource.loop = true;
        _audioSource.Stop();

        AudioSource audioSource = SoundManager.instance.PlayAudioClip(backClip, transform, backClipVolume, false);
        audioSource.loop = true;
    }

    private void FixedUpdate()
    {
        if (_isDrive)
        {
            float state = _driveAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            slider.value = state;
            if (state >= 1)
            {
                _isDrive = false;

                SkufHandler.instance.SetText(doneText, hungerImpact.ToString(), moneyImpact.ToString());

                _driveAnimator.SetBool("isDrive", false);
                _backgroundAnimation.Stop();
                doneCanvas.SetActive(true);

                Debug.Log(doneText.text);
                doneText.text = doneText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", moneyImpact.ToString());
                Debug.Log(doneText.text);

                SkufHandler.instance.ChangeMoney(moneyImpact);
                SkufHandler.instance.ChangeHunger(-hungerImpact);

                _audioSource.Stop();
            }
        }
    }

    

    public void Drive()
    {
        foreach (GameObject go in unactiveObjects)
        {
            go.SetActive(false);
        }
        _isDrive = true;

        _driveAnimator.SetBool("isDrive", true);
        _backgroundAnimation.Play();

        _audioSource.Play();
    }
}
