using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZavodHandler : MonoBehaviour
{
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private int moneyImpact = 30;
    [SerializeField] private int hungerImpact = 25;

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
    private RectTransform sliderRectTransform;
    private bool _sliderMoveRight = true;
    private float _random;

    [SerializeField] private GameObject[] itemSprites;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text neededIdText;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private GameObject line;
    [SerializeField] private float lineSpeed;
    [SerializeField] private int winScore;
    private int _score = 0;
    private bool _canMoveLine = false;
    private int _neededId;

    private void Start()
    {
        prevBtn.enabled = false;
        sliderRectTransform = slider.GetComponent<RectTransform>();

        SetNeededId();
        ChangeRange();
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

        if (_canMoveLine)
        {
            line.transform.position -= new Vector3(Time.deltaTime * lineSpeed, 0, 0);
        }
    }

    private IEnumerator MoveLine()
    {
        _canMoveLine = true;
        yield return new WaitForSeconds(1);
        _canMoveLine = false;
    }

    private void SetNeededId()
    {
        _neededId = Random.Range(0, maxSize + 1);
        neededIdText.text = "Need: " + _neededId;
    }

    public void BtnPressed()
    {
        if (slider.value >= _random && slider.value <= _random + successRangeValue && _id == _neededId)
        {
            _score += 1;
            scoreText.text = "Score: " + _score + "/" + winScore;
            Instantiate(itemSprites[_id], spawnPoint.position, Quaternion.identity, line.transform);

            if (_score >= winScore)
            {
                doneCanvas.SetActive(true);
                SkufHandler.instance.ChangeHunger(-hungerImpact);
                SkufHandler.instance.ChangeMoney(moneyImpact);
            }
        }
        else Instantiate(itemSprites[itemSprites.Length - 1], spawnPoint.position, Quaternion.identity, line.transform);
        SetNeededId();
        ChangeRange();
        StartCoroutine(MoveLine());
    }

    private void ChangeRange()
    {
        _random = Random.Range(0, 1 - successRangeValue);
        RectTransform successRangeRectTransform = successRange.GetComponent<RectTransform>();

        successRangeRectTransform.sizeDelta = new Vector2(sliderRectTransform.sizeDelta.x * successRangeValue, successRangeRectTransform.sizeDelta.y);
        successRangeRectTransform.anchoredPosition = new Vector3(_random * sliderRectTransform.sizeDelta.x, 0, 0);
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
