using UnityEngine;
using UnityEngine.UI;

public class ZavodHandler : MonoBehaviour
{
    [SerializeField] private GameObject imageItems;
    [SerializeField] private Transform[] imageItemsPoints;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private float offset;
    [SerializeField] private float speed;
    private bool _moveLeft = false;
    private bool _moveRight = false;
    private int maxSize = 3;
    private int _id = 0;


    [SerializeField] private GameObject successRange;
    [SerializeField] private Slider slider;
    [SerializeField] private float sliderSpeed;
    [SerializeField] private float successRangeValue;
    private bool _sliderMoveRight = true;
    private float _random;

    private void Start()
    {
        prevBtn.enabled = false;
        _random = Random.Range(0, 1 - successRangeValue);
        successRange.transform.localPosition = new Vector3(_random*345, 0, 0);
    }

    private void Update()
    {
        if (_moveRight)
        {
            imageItems.transform.position = Vector3.Lerp(imageItems.transform.position, imageItemsPoints[_id].position, speed * Time.deltaTime);
            if (imageItems.transform.position.x + offset >= imageItemsPoints[_id].position.x) _moveRight = false;
        }
        else if (_moveLeft)
        {
            imageItems.transform.position = Vector3.Lerp(imageItems.transform.position, imageItemsPoints[_id].position, speed * Time.deltaTime);
            if (imageItems.transform.position.x - offset <= imageItemsPoints[_id].position.x) _moveLeft = false;
        }

        if (slider.value < 1 && _sliderMoveRight)
        {
            slider.value += sliderSpeed * Time.deltaTime;
        }
        else
        {
            _sliderMoveRight = false;
        }

        if (slider.value > 0 && !_sliderMoveRight)
        {
            slider.value -= sliderSpeed * Time.deltaTime;
        }
        else
        {
            _sliderMoveRight = true;
        }
    }

    public void Next()
    {
        _moveLeft = true;


        _id += 1;
        CheckButton();

    }

    public void Prev()
    {
        _moveRight = true;


        _id -= 1;
        CheckButton();
    }
    public void BtnPressed()
    {
        if (slider.value >= _random && slider.value <= _random + successRangeValue)
        {
            Debug.Log("Ok");
            _random = Random.Range(0, 1 - successRangeValue);
            successRange.transform.localPosition = new Vector3(_random * 345, 0, 0);
        }
        else
        {
            Debug.Log("False");
        }

        

    }

    private void CheckButton()
    {
        if (_id == maxSize) nextBtn.enabled = false;
        else if (_id == 0) prevBtn.enabled = false;
        else
        {
            nextBtn.enabled = true;
            prevBtn.enabled = true;
        }
    }

    
}
