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
    [SerializeField] private AudioClip screenClip;
    [Range(0, 1)][SerializeField] private float screenClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int moneyImpact;
    [Range(0, 100)][SerializeField] private int failImpact;
    [Range(0, 100)][SerializeField] private float screenItemsSpeed;
    [Range(0, 100)][SerializeField] private float objectsSpeed;
    [Range(0, 100)][SerializeField] private float pressSpeed;
    [Range(0, 100)][SerializeField] private float sliderSpeed;
    [Range(0, 100)][SerializeField] private float successRangeValue;

    [Header("System")]
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text doneText;
    [SerializeField] private Text scoreText;
    [Space]
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button leverBtn;
    [Space]
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private RectTransform[] objects;
    [Space]
    [SerializeField] private RectTransform[] screenItemsPosition;
    [SerializeField] private RectTransform screenItemsParent;
    [SerializeField] private RectTransform objectsParent;
    [SerializeField] private RectTransform waitingTransform;
    [SerializeField] private RectTransform press;
    [Space]
    [SerializeField] private GameObject successRange;
    [SerializeField] private Slider slider;
    
    private List<RectTransform> _objectsList = new List<RectTransform>();
    private List<int> _idList = new List<int>();
    private bool _canPressScreenBtn = true;
    private bool _canPress = false;
    private int _curId = 0;
    private RectTransform _sliderRectTransform;
    private Sprite _defLeverSprite;
    private Image _leverImage;
    private float _random;
    private float _defY;
    private int _score;


    private void Start()
    {
        AudioSource backAudioSource = SoundManager.instance.PlayAudioClip(backClip, transform, backClipVolume, false);
        backAudioSource.loop = true;

        SkufHandler.instance.SetHUDVisibility(false);
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);
        else
        {
            SkufHandler.instance.SetText(doneText, hungerImpact.ToString(), moneyImpact.ToString());

            _sliderRectTransform = slider.GetComponent<RectTransform>();
            _leverImage = leverBtn.GetComponent<Image>();
            _defLeverSprite = _leverImage.sprite;

            ChangeRange();
            CheckButton();
            CreateObjects();

            StartCoroutine(MoveObjects());
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && prevBtn.interactable) Prev();
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && nextBtn.interactable) Next();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            leverBtn.onClick.Invoke();
            _leverImage.sprite = leverBtn.spriteState.pressedSprite;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) _leverImage.sprite = _defLeverSprite;

        float x = Time.time % sliderSpeed;
        slider.value = (-Mathf.Abs(2 * x / sliderSpeed - 1) + 1);
    }

    private IEnumerator StartPress(bool success)
    {
        AudioSource pressAudioSource = SoundManager.instance.PlayAudioClip(pressClip, transform, pressClipVolume, false);
        while (press.anchoredPosition.y > waitingTransform.anchoredPosition.y)
        {
            press.Translate(Vector2.down * pressSpeed * Time.deltaTime);
            yield return null;
        }

        if (success) Instantiate(items[_curId], _objectsList[0].parent);
        else Instantiate(items[items.Length-1], _objectsList[0].parent);
        Destroy(_objectsList[0].gameObject);
        _objectsList.RemoveAt(0);
        _idList.RemoveAt(0);

        while (press.anchoredPosition.y < _defY)
        {
            press.Translate(Vector2.up * pressSpeed * Time.deltaTime);
            yield return null;
        }

        pressAudioSource.Stop();

        if (_objectsList.Count > 0)
        {
            StartCoroutine(MoveObjects());
        }
        else
        {
            doneCanvas.SetActive(true);
            
            SkufHandler.instance.ChangeHunger(-hungerImpact);
            SkufHandler.instance.ChangeMoney(moneyImpact);
        }
    }

    private IEnumerator MoveObjects()
    {

        while (_objectsList[0].position.x > waitingTransform.position.x)
        {
            objectsParent.Translate(Vector2.left * objectsSpeed * Time.deltaTime);
            yield return null;
        }

        _canPress = true;
    }    

    private IEnumerator MoveScreenItemsLeft()
    {
        while (screenItemsParent.anchoredPosition.x > screenItemsPosition[_curId].anchoredPosition.x)
        {
            screenItemsParent.Translate(Vector2.left * screenItemsSpeed * Time.deltaTime);
            yield return null;
        }

        _canPressScreenBtn = true;
    }

    private IEnumerator MoveScreenItemsRight()
    {
        while (screenItemsParent.anchoredPosition.x < screenItemsPosition[_curId].anchoredPosition.x)
        {
            screenItemsParent.Translate(Vector2.right * screenItemsSpeed * Time.deltaTime);
            yield return null;
        }

        _canPressScreenBtn = true;
    }

    private void CreateObjects()
    {
        int rand;
        for (int i = 0; i < objects.Length; i++)
        {
            rand = Random.Range(0, prefabs.Length);
            GameObject newObject = Instantiate(prefabs[rand], objects[i]);
            _objectsList.Add(newObject.GetComponent<RectTransform>());
            _idList.Add(rand);
        }
    }

    private void ChangeRange()
    {
        _random = Random.Range(0, 1 - successRangeValue);
        RectTransform successRangeRectTransform = successRange.GetComponent<RectTransform>();

        successRangeRectTransform.sizeDelta = new Vector2(_sliderRectTransform.sizeDelta.x * successRangeValue, successRangeRectTransform.sizeDelta.y);
        successRangeRectTransform.anchoredPosition = new Vector3(_random * _sliderRectTransform.sizeDelta.x, 0, 0);
    }

    private void CheckButton()
    {
        SoundManager.instance.PlayAudioClip(screenClip, transform, screenClipVolume);

        if (_curId == 3) nextBtn.interactable = false;
        else if (_curId == 0) prevBtn.interactable = false;
        else
        {
            nextBtn.interactable = true;
            prevBtn.interactable = true;
        }
    }

    public void LeverPressed()
    {
        SoundManager.instance.PlayAudioClip(leverClip, transform, leverClipVolume);

        if (_canPress)
        {
            _score += 1;
            scoreText.text = $"{_score}/{objects.Length}";
            _canPress = false;

            if (slider.value >= _random && slider.value <= _random + successRangeValue && _curId == _idList[0])
            {
                StartCoroutine(StartPress(true));
            }
            else
            {
                moneyImpact -= failImpact;
                StartCoroutine(StartPress(false));
            }
        }

        ChangeRange();
    }

    public void Next()
    {
        if (_canPressScreenBtn)
        {
            _curId += 1;
            _canPressScreenBtn = false;

            CheckButton();
            StartCoroutine(MoveScreenItemsLeft());
        }
    }

    public void Prev()
    {
        if (_canPressScreenBtn)
        {
            _curId -= 1;
            _canPressScreenBtn = false;

            CheckButton();
            StartCoroutine(MoveScreenItemsRight());
        }
    }
}
