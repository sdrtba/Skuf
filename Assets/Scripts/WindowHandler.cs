using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowHandler : MonoBehaviour
{
    [SerializeField] private AudioClip birdClip;
    [Range(0f, 1f)][SerializeField] private float birdClipVolume;
    [SerializeField] private AudioClip deadClip;
    [Range(0f, 1f)][SerializeField] private float deadClipVolume;
    [SerializeField] private AudioClip streetClip;
    [Range(0f, 1f)][SerializeField] private float streetClipVolume;

    [SerializeField] GameObject bird;
    [Range(0, 100)][SerializeField] private int regenTime;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int scoreImpact;
    private bool _canSay = true;
    private AudioSource _birdAudioSource;
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

        _birdAudioSource = SoundManager.instance.PlayAudioClip(birdClip, transform, birdClipVolume, false);

        AudioSource streetAudioSource = SoundManager.instance.PlayAudioClip(streetClip, transform, streetClipVolume, false);
        streetAudioSource.loop = true;
    }

    private void Update()
    {
        if (_canSay && _image.enabled)
        {
            StartCoroutine(Say());
        }
    }


    public void Eat()
    {
        bird.GetComponent<Image>().enabled = false;
        bird.GetComponent<Button>().enabled = false;

        _birdAudioSource.Stop();
        SoundManager.instance.PlayAudioClip(deadClip, transform, deadClipVolume);

        SkufHandler.instance.ChangeHunger(hungerImpact);
        SkufHandler.instance.ChangeScore(scoreImpact);
        SkufHandler.instance.StartCoroutine(SkufHandler.instance.EatBird(regenTime));
    }

    private IEnumerator Say()
    {
        _canSay = false;
        int rand = Random.Range(1, 5);
        _birdAudioSource.Play();
        yield return new WaitForSeconds(rand + _birdAudioSource.clip.length);
        _canSay = true;
    }
}
