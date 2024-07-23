using UnityEngine;
using UnityEngine.UI;

public class FreezeHandler : MonoBehaviour
{
    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject food;
    [SerializeField] private Text bearText;
    [SerializeField] private Text foodText;
    [Range(0, 100)][SerializeField] private int bearImpact;
    [Range(0, 100)][SerializeField] private int foodImpact;
    [Range(0, 100)][SerializeField] private int foodExtraImpact;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
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

            SkufHandler.instance.foodCount--;
            SkufHandler.instance.ChangeHunger(foodImpact);
            ChangeFreeze();
        }
    }
}
