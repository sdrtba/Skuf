using UnityEngine;

public class ShopHandler : MonoBehaviour
{
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
            SkufHandler.instance.ChangeMoney(-bearPrice);
            SkufHandler.instance.bearCount++;
        }
    }

    public void BuyFood()
    {
        if (SkufHandler.instance.CanBuy(foodPrice))
        {
            SkufHandler.instance.ChangeMoney(-foodPrice);
            SkufHandler.instance.foodCount++;
        }
    }
}
