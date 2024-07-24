using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZavodHandler : MonoBehaviour
{
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text doneText;
    [Range(0, 100)][SerializeField] private int moneyImpact;
    [Range(0, 100)][SerializeField] private int hungerImpact;

    [SerializeField] private GameObject imageItems;
    [SerializeField] private Transform[] imageItemsPoints;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [Range(0f, 100f)][SerializeField] private float offset;
    [Range(0f, 100f)][SerializeField] private float speed;
    private bool _moveLeft = false;
    private bool _moveRight = false;
    private int _maxSize = 3;
    private int _id = 0;


    [SerializeField] private GameObject successRange;
    [SerializeField] private Slider slider;
    [Range(0f, 100f)][SerializeField] private float sliderSpeed;
    [Range(0f, 100f)][SerializeField] private float successRangeValue;
    private RectTransform _sliderRectTransform;
    private bool _sliderMoveRight = true;
    private float _random;

    [SerializeField] private RectTransform lineObject;
    [SerializeField] private GameObject[] itemSprites;
    [SerializeField] private Text scoreText;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private GameObject line;
    [Range(0f, 100f)][SerializeField] private float lineSpeed;
    [Range(0, 100)][SerializeField] private int winScore;
    private int _score = 0;
    private bool _canMoveLine = false;
    private int _neededId;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);

        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);
        doneText.text = doneText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", moneyImpact.ToString());

        prevBtn.interactable = false;
        _sliderRectTransform = slider.GetComponent<RectTransform>();

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
            lineObject.sizeDelta += new Vector2(Time.deltaTime * lineSpeed * 108, 0);
        }
    }

    private IEnumerator MoveLine()
    {
        _canMoveLine = true;
        yield return new WaitForSeconds(1);
        _canMoveLine = false;
    }

    public void BtnPressed()
    {
        if (slider.value >= _random && slider.value <= _random + successRangeValue && _id == _neededId)
        {
            _score += 1;
            scoreText.text = _score + "/" + winScore;
            Instantiate(itemSprites[_id], spawnPoint.position, Quaternion.identity, line.transform);

            if (_score >= winScore)
            {
                doneCanvas.SetActive(true);
                SkufHandler.instance.ChangeHunger(-hungerImpact);
                SkufHandler.instance.ChangeMoney(moneyImpact);
            }
        }
        else Instantiate(itemSprites[itemSprites.Length - 1], spawnPoint.position, Quaternion.identity, line.transform);
        ChangeRange();
        StartCoroutine(MoveLine());
    }

    private void ChangeRange()
    {
        _neededId = Random.Range(0, _maxSize + 1);

        _random = Random.Range(0, 1 - successRangeValue);
        RectTransform successRangeRectTransform = successRange.GetComponent<RectTransform>();

        successRangeRectTransform.sizeDelta = new Vector2(_sliderRectTransform.sizeDelta.x * successRangeValue, successRangeRectTransform.sizeDelta.y);
        successRangeRectTransform.anchoredPosition = new Vector3(_random * _sliderRectTransform.sizeDelta.x, 0, 0);
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
        if (_id == _maxSize) nextBtn.interactable = false;
        else if (_id == 0) prevBtn.interactable = false;
        else
        {
            nextBtn.interactable = true;
            prevBtn.interactable = true;
        }
    }
}
