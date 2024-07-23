using UnityEngine;
using UnityEngine.UI;

public class BirdHandler : MonoBehaviour
{
    [SerializeField] GameObject bird;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        if (!SkufHandler.instance.isBirdActive) bird.SetActive(false);
        if (SkufHandler.instance.hunger > 0) bird.GetComponent<Button>().interactable = false;
    }


    public void Eat()
    {
        Destroy(bird);
        SkufHandler.instance.EatBird();
    }
}
