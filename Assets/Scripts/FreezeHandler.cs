using UnityEngine;
using UnityEngine.UI;

public class FreezeHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip[] drinkClip;
    [Range(0, 1)][SerializeField] private float drinkClipVolume;
    [SerializeField] private AudioClip eatClip;
    [Range(0, 1)][SerializeField] private float eatClipVolume;
    [SerializeField] private AudioClip openClip;
    [Range(0, 1)][SerializeField] private float openClipVolume;
    [SerializeField] private AudioClip deniedClip;
    [Range(0, 1)][SerializeField] private float deniedClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private int bearImpact;
    [Range(0, 100)][SerializeField] private int foodImpact;
    [Range(0, 100)][SerializeField] private int foodExtraImpact;

    [Header("System")]
    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject food;
    [SerializeField] private Text bearText;
    [SerializeField] private Text foodText;
    
    private AudioSource _drinkAudioSourceInstance;
    private AudioSource _eatAudioSourceInstance;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        SoundManager.instance.PlayAudioClip(openClip, transform, openClipVolume);
        ChangeFreeze(); 
    }

    private void ChangeFreeze()
    {
        if (SkufHandler.instance.bearCount == 0) bear.SetActive(false);
        else bearText.text = "x" + SkufHandler.instance.bearCount;

        if (SkufHandler.instance.foodCount == 0) food.SetActive(false);
        else foodText.text = "x" + SkufHandler.instance.foodCount;
    }

    public void TakeBear()
    {
        if (SkufHandler.instance.score < SkufHandler.instance.maxScore)
        {
            if (_drinkAudioSourceInstance == null)
            {
                _drinkAudioSourceInstance = SoundManager.instance.PlayAudioClip(drinkClip, transform, drinkClipVolume, false);
                Destroy(_drinkAudioSourceInstance, _drinkAudioSourceInstance.clip.length);
            }

            SkufHandler.instance.bearCount--;
            SkufHandler.instance.ChangeScore(bearImpact);
            ChangeFreeze();
        }
        else
        {
            SoundManager.instance.PlayAudioClip(deniedClip, transform, deniedClipVolume);
        }
    }

    public void TakeFood()
    {
        if (SkufHandler.instance.hunger < SkufHandler.instance.maxHunger)
        {
            if (SkufHandler.instance.hunger > SkufHandler.instance.maxHunger * 0.66) SkufHandler.instance.ChangeScore(foodExtraImpact);

            if (_eatAudioSourceInstance == null)
            {
                _eatAudioSourceInstance = SoundManager.instance.PlayAudioClip(eatClip, transform, eatClipVolume, false);
                Destroy(_eatAudioSourceInstance, _eatAudioSourceInstance.clip.length);
            }

            SkufHandler.instance.foodCount--;
            SkufHandler.instance.ChangeHunger(foodImpact);
            ChangeFreeze();
        }
        else
        {
            SoundManager.instance.PlayAudioClip(deniedClip, transform, deniedClipVolume);
        }
    }
}
