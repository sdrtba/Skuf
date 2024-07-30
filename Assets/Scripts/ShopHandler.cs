using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private AudioClip backClip;
    [Range(0f, 1f)][SerializeField] private float backClipVolume;
    [SerializeField] private AudioClip openClip;
    [Range(0f, 1f)][SerializeField] private float openClipVolume;
    [SerializeField] private AudioClip buyClip;
    [Range(0f, 1f)][SerializeField] private float buyClipVolume;
    [SerializeField] private AudioClip deniedClip;
    [Range(0f, 1f)][SerializeField] private float deniedClipVolume;

    [Range(0, 100)][SerializeField] private int bearPrice;
    [Range(0, 100)][SerializeField] private int foodPrice;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        SoundManager.instance.PlayAudioClip(openClip, transform, openClipVolume);

        AudioSource audioSource = SoundManager.instance.PlayAudioClip(backClip, transform, backClipVolume, false);
        audioSource.loop = true;
    }

    public void BuyBear()
    {
        if (SkufHandler.instance.CanBuy(bearPrice))
        {
            SoundManager.instance.PlayAudioClip(buyClip, transform, buyClipVolume);

            SkufHandler.instance.ChangeMoney(-bearPrice);
            SkufHandler.instance.bearCount++;
        }
        else
        {
            SoundManager.instance.PlayAudioClip(deniedClip, transform, deniedClipVolume);
        }
    }

    public void BuyFood()
    {
        if (SkufHandler.instance.CanBuy(foodPrice))
        {
            SoundManager.instance.PlayAudioClip(buyClip, transform, buyClipVolume);

            SkufHandler.instance.ChangeMoney(-foodPrice);
            SkufHandler.instance.foodCount++;
        }
        else
        {
            SoundManager.instance.PlayAudioClip(deniedClip, transform, deniedClipVolume);
        }
    }
}
