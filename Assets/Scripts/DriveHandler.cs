using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DriveHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip streetClip;
    [Range(0, 1)][SerializeField] private float streetClipVolume;
    [SerializeField] private AudioClip driveClip;
    [Range(0, 1)][SerializeField] private float driveClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private int deliveryTime;
    [Range(0, 100)][SerializeField] private float frequenz;
    [Range(0, 100)][SerializeField] private float amplitude;
    [Range(0, 100)][SerializeField] private float backGroundSpeed;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int moneyImpact;

    [Header("System")]
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Slider slider;
    [SerializeField] private Text doneText;
    [Space]
    [SerializeField] private RectTransform driver;
    [SerializeField] private RectTransform backGround;
    [SerializeField] private float backGroundEndX;
    [SerializeField] private float backGroundStartX;

    private bool _isDrive = false;
    private AudioSource _driveAudioSource;
    private float _defDriverY;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);
        else {
            SkufHandler.instance.SetText(doneText, hungerImpact.ToString(), moneyImpact.ToString());

            _defDriverY = driver.anchoredPosition.y;
            slider.maxValue = deliveryTime;

            _driveAudioSource = SoundManager.instance.PlayAudioClip(driveClip, transform, driveClipVolume, false);
            _driveAudioSource.loop = true;
            _driveAudioSource.Stop();

            AudioSource streetAudioSource = SoundManager.instance.PlayAudioClip(streetClip, transform, streetClipVolume, false);
            streetAudioSource.loop = true;
        }
    }

    public void Drive()
    {
        _isDrive = true;
        _driveAudioSource.Play();
        StartCoroutine(Move());
        StartCoroutine(Timer());
    }

    public IEnumerator Move()
    {
        while (_isDrive)
        {
            slider.value += Time.deltaTime;

            driver.anchoredPosition = new Vector2(driver.anchoredPosition.x, Mathf.Sin(Time.time * frequenz) * amplitude + _defDriverY);

            backGround.Translate(Vector2.left * backGroundSpeed * Time.deltaTime);
            if (backGround.anchoredPosition.x <= backGroundEndX)
            {
                backGround.anchoredPosition = new Vector2(backGroundStartX, backGround.anchoredPosition.y);
            }

            yield return null;
        }
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(deliveryTime);

        _isDrive = false;
        _driveAudioSource.Stop();
        doneCanvas.SetActive(true);

        SkufHandler.instance.ChangeMoney(moneyImpact);
        SkufHandler.instance.ChangeHunger(-hungerImpact);

        YG.YandexGame.SaveProgress();
    }
}
