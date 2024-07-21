using UnityEngine;
using UnityEngine.UI;

public class DriveHandler : MonoBehaviour
{
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text doneText;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject driver;
    [SerializeField] private GameObject[] unactiveObjects;
    [SerializeField] private Slider slider;
    [SerializeField] private int moneyImpact;
    [SerializeField] private int hungerImpact;

    private bool _isDrive = false;
    private Animator driveAnimator;
    private Animation backgroundAnimation;


    private void Start()
    {
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);

        doneText.text = doneText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", moneyImpact.ToString());
        driveAnimator = driver.gameObject.GetComponent<Animator>();
        backgroundAnimation = backGround.GetComponent<Animation>();
    }

    private void FixedUpdate()
    {
        if (_isDrive)
        {
            float state = driveAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            slider.value = state;
            if (state >= 1)
            {
                _isDrive = false;

                driveAnimator.SetBool("isDrive", false);
                backgroundAnimation.Stop();
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

        driveAnimator.SetBool("isDrive", true);
        backgroundAnimation.Play();
    }
}
