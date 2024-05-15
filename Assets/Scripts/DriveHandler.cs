using UnityEngine;
using UnityEngine.UI;

public class DriveHandler : MonoBehaviour
{
    [SerializeField] private GameObject DoneCanvas;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject driver;
    [SerializeField] private GameObject backBtn;
    [SerializeField] private GameObject driveBtn;
    [SerializeField] private Slider slider;
    [SerializeField] private int salary = 15; //?
    [SerializeField] private int hungerImpact = 10; //?

    private bool _isDrive = false;
    private Animator driveAnimator;
    private Animation backgroundAnimation;


    private void Start()
    {
        driveAnimator = driver.gameObject.GetComponent<Animator>();
        backgroundAnimation = backGround.GetComponent<Animation>();
    }

    private void FixedUpdate()
    {
        if (_isDrive)
        {
            float state = driveAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            Debug.Log(state);
            slider.value = state;
            if (state >= 1)
            {
                _isDrive = false;

                driveAnimator.SetBool("isDrive", false);
                backgroundAnimation.Stop();
                DoneCanvas.SetActive(true);

                SkufHandler.instance.ChangeMoney(salary);
                SkufHandler.instance.ChangeHunger(hungerImpact);
            }
        }
    }

    public void Drive()
    {
        if (SkufHandler.instance.hunger > 0)
        {
            driveBtn.SetActive(false);
            backBtn.SetActive(false);
            _isDrive = true;

            driveAnimator.SetBool("isDrive", true);
            backgroundAnimation.Play();
        }
    }
}
