using System;
using UnityEngine;
using UnityEngine.UI;

public class BirdHandler : MonoBehaviour
{
    [SerializeField] GameObject bird;
    [Range(0, 100)][SerializeField] private int regenTime;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int scoreImpact;
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)][SerializeField] private float clipVolume;


    private void Start()
    {
        if (!SkufHandler.instance.isBirdActive)
        {
            bird.GetComponent<Image>().enabled = false;
            bird.GetComponent<Button>().enabled = false;
        }

        SkufHandler.instance.SetHUDVisibility(true);
        if (SkufHandler.instance.hunger > 0) bird.GetComponent<Button>().interactable = false;
    }


    public void Eat()
    {
        bird.GetComponent<Image>().enabled = false;
        bird.GetComponent<Button>().enabled = false;

        SoundManager.instance.PlayAudioClip(clip, transform, clipVolume);

        SkufHandler.instance.ChangeHunger(hungerImpact);
        SkufHandler.instance.ChangeScore(scoreImpact);
        SkufHandler.instance.StartCoroutine(SkufHandler.instance.EatBird(regenTime));
    }
}
