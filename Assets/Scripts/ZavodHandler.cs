using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZavodHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip backClip;
    [Range(0, 1)][SerializeField] private float backClipVolume;
    [SerializeField] private AudioClip leverClip;
    [Range(0, 1)][SerializeField] private float leverClipVolume;
    [SerializeField] private AudioClip pressClip;
    [Range(0, 1)][SerializeField] private float pressClipVolume;
    [SerializeField] private AudioClip changeClip;
    [Range(0, 1)][SerializeField] private float changeClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private int failImpact;
    [Range(0, 100)][SerializeField] private int moneyImpact;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0f, 100f)][SerializeField] private float pressSpeed;
    [Range(0f, 100f)][SerializeField] private float prefabsSpeed;
    [Range(0f, 100f)][SerializeField] private float itemsSpeed;
    [Range(0f, 100f)][SerializeField] private float sliderSpeed;
    [Range(0f, 1f)][SerializeField] private float successRangeValue;

    [Header("System")]
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text doneText;
    [SerializeField] private Text indexText;
    [SerializeField] private RectTransform press;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private RectTransform prefabsParentTransform;
    [SerializeField] private RectTransform waitingTransform;
    [SerializeField] private GameObject itemsParent;
    [SerializeField] private Transform[] imageItemsPoints;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject successRange;

    private List<int> _idList = new List<int>();
    private bool _sliderMoveRight = true;
    private bool _itemsMoveRight = false;
    private bool _itemsMoveLeft = false;
    private bool _canPressBtn = true;
    private bool _canPress = false;
    private float _offset = 0.1f;
    private int _index = 0;
    private int _id = 0;
    private RectTransform _itemTransform;
    private RectTransform _sliderRectTransform;
    private float _random;
    private float _defY;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);

        _itemTransform = prefabs[_index].GetComponent<RectTransform>();
        _sliderRectTransform = slider.GetComponent<RectTransform>();
        prevBtn.interactable = false;
        _defY = press.position.y;

        AudioSource windAudioSource = SoundManager.instance.PlayAudioClip(backClip, transform, backClipVolume, false);
        windAudioSource.loop = true;

        int rand;
        for (int i = 0; i < prefabs.Length; i++)
        {
            rand = Random.Range(0, items.Length);
            _idList.Add(rand);
            Instantiate(items[rand], prefabs[i].transform).GetComponent<Image>().raycastTarget = false;
        }

        ChangeRange();
        StartCoroutine(MoveLine());
    }

    private void Update()
    {
        if (_itemsMoveRight)
        {
            itemsParent.transform.position = Vector3.Lerp(itemsParent.transform.position, imageItemsPoints[_id].position, itemsSpeed * Time.deltaTime);
            if (itemsParent.transform.position.x + _offset >= imageItemsPoints[_id].position.x)
            {
                _itemsMoveRight = false;
                _canPressBtn = true;
            }
        }
        else if (_itemsMoveLeft)
        {
            itemsParent.transform.position = Vector3.Lerp(itemsParent.transform.position, imageItemsPoints[_id].position, itemsSpeed * Time.deltaTime);
            if (itemsParent.transform.position.x - _offset <= imageItemsPoints[_id].position.x)
            { 
                _itemsMoveLeft = false;
                _canPressBtn = true;
            }
        }

        if (slider.value < 1 && _sliderMoveRight) slider.value += sliderSpeed * Time.deltaTime;
        else _sliderMoveRight = false;
        if (slider.value > 0 && !_sliderMoveRight) slider.value -= sliderSpeed * Time.deltaTime;
        else _sliderMoveRight = true;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Prev();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }

    private IEnumerator MoveLine()
    {
        while (_itemTransform.position.x > waitingTransform.position.x)
        {
            prefabsParentTransform.position -= new Vector3(prefabsSpeed * Time.deltaTime, 0f, 0f);
            yield return null;
        }
        _canPress = true;
    }

    private IEnumerator Press(bool success)
    {
        AudioSource audioSource = SoundManager.instance.PlayAudioClip(pressClip, transform, pressClipVolume, false);
        _canPress = false;
        while (press.position.y > waitingTransform.position.y)
        {
            press.position -= new Vector3(0, pressSpeed * Time.deltaTime, 0);
            yield return null;
        }

        Destroy(_itemTransform.GetChild(0).gameObject);
        if (success) Instantiate(objects[_idList[0]], _itemTransform);
        else Instantiate(objects[objects.Length-1], _itemTransform);
        _idList.RemoveAt(0);

        _index += 1;
        indexText.text = _index + "/" + prefabs.Length;

        audioSource.Stop();

        while (press.position.y < _defY)
        {
            press.position += new Vector3(0, pressSpeed * Time.deltaTime, 0);
            yield return null;
        }

        if (_index >= prefabs.Length)
        {
            doneText.text = doneText.text.Replace("{0}", hungerImpact.ToString()).Replace("{1}", moneyImpact.ToString());
            doneCanvas.SetActive(true);
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeMoney(moneyImpact);
        }
        else
        {
            _itemTransform = prefabs[_index].GetComponent<RectTransform>();
            StartCoroutine(MoveLine());
        }
    }

    public void LeverPressed()
    {
        SoundManager.instance.PlayAudioClip(leverClip, transform, leverClipVolume);
        if (_canPress)
        {
            if (slider.value >= _random && slider.value <= _random + successRangeValue && _id == _idList[0])
            {
                StartCoroutine(Press(true));
            }
            else
            {
                StartCoroutine(Press(false));
                moneyImpact -= failImpact;
            }

        }
        
        ChangeRange();
    }

    private void ChangeRange()
    {
        _random = Random.Range(0, 1 - successRangeValue);
        RectTransform successRangeRectTransform = successRange.GetComponent<RectTransform>();

        successRangeRectTransform.sizeDelta = new Vector2(_sliderRectTransform.sizeDelta.x * successRangeValue, successRangeRectTransform.sizeDelta.y);
        successRangeRectTransform.anchoredPosition = new Vector3(_random * _sliderRectTransform.sizeDelta.x, 0, 0);
    }

    public void Next()
    {
        if (_canPressBtn)
        {
            _canPressBtn = false;
            _itemsMoveLeft = true;

            _id += 1;
            CheckButton();

        }
        
    }

    public void Prev()
    {
        if (_canPressBtn)
        {
            _canPressBtn = false;
            _itemsMoveRight = true;

            _id -= 1;
            CheckButton();
        }
        
    }

    private void CheckButton()
    {
        SoundManager.instance.PlayAudioClip(changeClip, transform, changeClipVolume);
        if (_id == 3) nextBtn.interactable = false;
        else if (_id == 0) prevBtn.interactable = false;
        else
        {
            nextBtn.interactable = true;
            prevBtn.interactable = true;
        }
    }
}
