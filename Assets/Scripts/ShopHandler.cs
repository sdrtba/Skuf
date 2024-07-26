using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private AudioClip buyClip;
    [SerializeField] private AudioClip deniedClip;
    [Range(0f, 1f)][SerializeField] private float clipVolume;

    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject food;
    [Range(0, 100)][SerializeField] private int bearPrice;
    [Range(0, 100)][SerializeField] private int foodPrice;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
    }

    public void BuyBear()
    {
        if (SkufHandler.instance.CanBuy(bearPrice))
        {
            SoundManager.instance.PlayAudioClip(buyClip, transform, clipVolume);

            SkufHandler.instance.ChangeMoney(-bearPrice);
            SkufHandler.instance.bearCount++;
        }
        else
        {
            SoundManager.instance.PlayAudioClip(deniedClip, transform, clipVolume);
        }
    }

    public void BuyFood()
    {
        if (SkufHandler.instance.CanBuy(foodPrice))
        {
            SoundManager.instance.PlayAudioClip(buyClip, transform, clipVolume);

            SkufHandler.instance.ChangeMoney(-foodPrice);
            SkufHandler.instance.foodCount++;
        }
        else
        {
            SoundManager.instance.PlayAudioClip(deniedClip, transform, clipVolume);
        }
    }
}
