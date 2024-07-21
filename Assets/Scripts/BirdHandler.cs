using UnityEngine;
using UnityEngine.UI;

public class BirdHandler : MonoBehaviour
{
    [SerializeField] GameObject bird;

    private void Start()
    {
        if (!SkufHandler.instance.isBirdActive) bird.SetActive(false);
        if (SkufHandler.instance.hunger > 0) bird.GetComponent<Button>().interactable = false;
    }


    public void Eat()
    {
        Destroy(bird);
        SkufHandler.instance.EatBird();
    }
}
