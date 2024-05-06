using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject food;
    [SerializeField] private int bearPrice = 15; //?
    [SerializeField] private int foodPrice = 10; //?

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
