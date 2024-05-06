using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject pushUpForm;

    public void ToggleActive() => pushUpForm.SetActive(!pushUpForm.activeSelf);
}
