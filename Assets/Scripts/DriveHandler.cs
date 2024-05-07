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
    [SerializeField] private float time = 10; //?
    [SerializeField] private int salary = 15; //?
    [SerializeField] private int hungerImpact = 10; //?

    private bool _isDrive = false;
    private const float _speed = 12 * 10;
    private const float _amplitude = 0.05f;
    private const float _curDriverPos = -2.2f;
    private float _curBackPos;

    private float _lifeTime;
    private float _gameTime;


    private void Start()
    {
        _curBackPos = backGround.transform.position.x;
        slider.maxValue = time;
    }

    /*
        private void Update()
        {
            if (_isDrive)
            {
                _gameTime += Time.fixedDeltaTime / 25;
                if (_gameTime >= 1)
                {
                    _lifeTime += 1;
                    _gameTime = 0;
                }
                if (_lifeTime >= time)
                {
                    DoneCanvas.SetActive(true);
                    _isDrive = false;
                    SkufHandler.instance.ChangeMoney(salary);
                    SkufHandler.instance.ChangeHunger(hungerImpact);
                }

                driver.transform.position = new Vector3(driver.transform.position.x, _curDriverPos + Mathf.Sin(Time.fixedTime * _speed) * _amplitude, driver.transform.position.z);
                backGround.transform.position = new Vector3(_curBackPos, backGround.transform.position.y, backGround.transform.position.z);
                _curBackPos -= _speed / 10000;
                slider.value = _lifeTime;
            }
        }*/

    public void Drive()
    {
        if (SkufHandler.instance.hunger > 0)
        {
            driveBtn.SetActive(false);
            backBtn.SetActive(false);
            _lifeTime = 0;
            _isDrive = true;

            var animation = driver.gameObject.GetComponent<Animation>();
            animation.Play();
            backGround.GetComponent<Animation>().Play();
        }
    }
}
