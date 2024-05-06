using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BirdHandler : MonoBehaviour
{
    private void Start()
    {
        
        if (!SkufHandler.instance.isBirdActive) gameObject.SetActive(false);
        if (SkufHandler.instance.hunger > 0) gameObject.GetComponent<Button>().interactable = false;
    }


    public void Eat()
    {
        Destroy(gameObject);
        SkufHandler.instance.EatBird();
    }
}
