using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BirdHandler : MonoBehaviour
{
    [SerializeField] GameObject bird;
    [Range(0, 100)][SerializeField] private int regenTime;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int scoreImpact;
    [SerializeField] private AudioClip deadClip;
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)][SerializeField] private float clipVolume;
    private bool _canSay = true;
    private AudioSource _audioSource;
    private Image _image;
    private Button _button;


    private void Start()
    {
        _image = bird.GetComponent<Image>();
        _button = bird.GetComponent<Button>();

        if (!SkufHandler.instance.isBirdActive)
        {
            _image.enabled = false;
            _button.enabled = false;
        }

        SkufHandler.instance.SetHUDVisibility(true);
        if (SkufHandler.instance.hunger > 0) bird.GetComponent<Button>().interactable = false;

        _audioSource = SoundManager.instance.PlayAudioClip(clip, transform, clipVolume, false);
        _audioSource.Stop();
    }

    private void Update()
    {
        if (_canSay && _image.enabled == true)
        {
            StartCoroutine(Say());
        }
    }


    public void Eat()
    {
        bird.GetComponent<Image>().enabled = false;
        bird.GetComponent<Button>().enabled = false;

        _audioSource.Stop();
        SoundManager.instance.PlayAudioClip(deadClip, transform, clipVolume);

        SkufHandler.instance.ChangeHunger(hungerImpact);
        SkufHandler.instance.ChangeScore(scoreImpact);
        SkufHandler.instance.StartCoroutine(SkufHandler.instance.EatBird(regenTime));
    }

    private IEnumerator Say()
    {
        _canSay = false;
        int rand = Random.Range(1, 5);
        _audioSource.Play();
        yield return new WaitForSeconds(rand + _audioSource.clip.length);
        _canSay = true;
    }
}
