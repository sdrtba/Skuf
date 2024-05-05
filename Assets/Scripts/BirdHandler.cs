using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BirdHandler : MonoBehaviour
{
    private bool _isActive = true;

    private void OnMouseEnter()
    {
        if (SkufHandler.instance.hunger == 0)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else gameObject.GetComponent<Button>().interactable = false;
    }
}
