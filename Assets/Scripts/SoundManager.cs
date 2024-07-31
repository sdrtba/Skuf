using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [NonSerialized] public static SoundManager instance;

    [Header("System")]
    [SerializeField] private AudioSource soundFXObject;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public AudioSource PlayAudioClip(AudioClip clip, Transform spawnTransform, float volume, bool destroy = true)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        if (destroy)
        {
            Destroy(audioSource.gameObject, clipLength);
            return null;
        }
        return audioSource;
    }

    public AudioSource PlayAudioClip(AudioClip[] clip, Transform spawnTransform, float volume, bool destroy = true)
    {
        int random = UnityEngine.Random.Range(0, clip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        if (destroy)
        {
            Destroy(audioSource.gameObject, clipLength);
            return null;
        }
        return audioSource;
    }
}
