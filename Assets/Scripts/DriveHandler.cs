using UnityEngine;
using UnityEngine.UI;

public class DriveHandler : MonoBehaviour
{
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text doneText;
    [SerializeField] private GameObject[] unactiveObjects;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject driver;
    [SerializeField] private Slider slider;
    [Range(0, 100)][SerializeField] private int moneyImpact;
    [Range(0, 100)][SerializeField] private int hungerImpact;

    private bool _isDrive = false;
    private Animator _driveAnimator;
    private Animation _backgroundAnimation;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);

        doneText.text = doneText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", moneyImpact.ToString());
        _driveAnimator = driver.gameObject.GetComponent<Animator>();
        _backgroundAnimation = backGround.GetComponent<Animation>();
    }

    private void FixedUpdate()
    {
        if (_isDrive)
        {
            float state = _driveAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            slider.value = state;
            if (state >= 1)
            {
                _isDrive = false;

                _driveAnimator.SetBool("isDrive", false);
                _backgroundAnimation.Stop();
                doneCanvas.SetActive(true);

                SkufHandler.instance.ChangeMoney(moneyImpact);
                SkufHandler.instance.ChangeHunger(-hungerImpact);
            }
        }
    }

    public void Drive()
    {
        foreach (GameObject go in unactiveObjects)
        {
            go.SetActive(false);
        }
        _isDrive = true;

        _driveAnimator.SetBool("isDrive", true);
        _backgroundAnimation.Play();
    }
}
