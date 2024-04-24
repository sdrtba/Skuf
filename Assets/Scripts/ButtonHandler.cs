using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject pushUpForm;

    public void ToggleActive()
    {
        pushUpForm.SetActive(!pushUpForm.activeSelf);
    }
}
