using UnityEngine;
using UnityEngine.UI;

public class FreezeHandler : MonoBehaviour
{
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip eatClip;
    [SerializeField] private AudioClip[] drinkClip;
    [Range(0f, 1f)][SerializeField] private float clipVolume;

    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject food;
    [SerializeField] private Text bearText;
    [SerializeField] private Text foodText;
    [Range(0, 100)][SerializeField] private int bearImpact;
    [Range(0, 100)][SerializeField] private int foodImpact;
    [Range(0, 100)][SerializeField] private int foodExtraImpact;

    private AudioSource _drinkAudioSourceInstance;
    private AudioSource _eatAudioSourceInstance;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        SoundManager.instance.PlayAudioClip(openClip, transform, clipVolume);
        ChangeFreeze(); 
    }

    private void ChangeFreeze()
    {
        if (SkufHandler.instance.bearCount == 0) bear.SetActive(false);
        if (SkufHandler.instance.foodCount == 0) food.SetActive(false);
        bearText.text = "x" + SkufHandler.instance.bearCount;
        foodText.text = "x" + SkufHandler.instance.foodCount;
    }

    public void TakeBear()
    {
        if (SkufHandler.instance.score != SkufHandler.instance.maxScore)
        {
            if (_drinkAudioSourceInstance == null)
            {
                _drinkAudioSourceInstance = SoundManager.instance.PlayAudioClip(drinkClip, transform, clipVolume, false);
                Destroy(_drinkAudioSourceInstance, _drinkAudioSourceInstance.clip.length);
            }

            SkufHandler.instance.bearCount--;
            SkufHandler.instance.ChangeScore(bearImpact);
            ChangeFreeze();
        }
    }

    public void TakeFood()
    {
        if (SkufHandler.instance.hunger != SkufHandler.instance.maxHunger)
        {
            if (SkufHandler.instance.hunger > SkufHandler.instance.maxHunger * 0.66) SkufHandler.instance.ChangeScore(foodExtraImpact);

            if (_eatAudioSourceInstance == null)
            {
                _eatAudioSourceInstance = SoundManager.instance.PlayAudioClip(eatClip, transform, clipVolume, false);
                Destroy(_eatAudioSourceInstance, _eatAudioSourceInstance.clip.length);
            }

            SkufHandler.instance.foodCount--;
            SkufHandler.instance.ChangeHunger(foodImpact);
            ChangeFreeze();
        }
    }
}
