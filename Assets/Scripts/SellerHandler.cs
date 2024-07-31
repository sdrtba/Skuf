using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Arrow
{
    Left,
    Right,
    Up,
    Down,
    None
}

public class SellerHandler : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip peepClip;
    [Range(0, 1)][SerializeField] private float peepClipVolume;
    [SerializeField] private AudioClip clientClip;
    [Range(0, 1)][SerializeField] private float clientClipVolume;
    [SerializeField] private AudioClip backClip;
    [Range(0, 1)][SerializeField] private float backClipVolume;

    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private float objectsParentSpeed;
    [Range(0, 100)][SerializeField] private int stepsCount;
    [Range(0, 100)][SerializeField] private int winScore;
    [Range(0, 100)][SerializeField] private int hungerImpact;
    [Range(0, 100)][SerializeField] private int moneyImpact;

    [Header("System")]
    [SerializeField] private GameObject hungerCanvas;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text doneText;
    [SerializeField] private Text scoreText;
    [Space]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private RectTransform objectsParent;
    [SerializeField] private RectTransform endTransform;
    [Space]
    [SerializeField] private Sprite[] arrowSprites;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private RectTransform line;

    private List<GameObject> _objectsList = new List<GameObject>(); 
    private int _score = 0;
    private Vector3 _defParentPosition;
    private Transform _arrowTransform;
    private Image _arrowImage;
    private Arrow _curArrow;
    private bool _canMove;
    private int _curStep;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);
        if (SkufHandler.instance.hunger <= 0) hungerCanvas.SetActive(true);
        else
        {
            SkufHandler.instance.SetText(doneText, hungerImpact.ToString(), moneyImpact.ToString());

            _defParentPosition = objectsParent.transform.position;
            _arrowImage = arrowObject.GetComponent<Image>();
            _arrowTransform = arrowObject.GetComponent<Transform>();

            AudioSource backAudioSource = SoundManager.instance.PlayAudioClip(backClip, transform, backClipVolume, false);
            backAudioSource.loop = true;

            GenerateObjects();
        }
    }

    private void Update()
    {
        if (_canMove)
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && _curArrow == Arrow.Left)
            {
                CallMove();
            }
            else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && _curArrow == Arrow.Right)
            {
                CallMove();
            }
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _curArrow == Arrow.Up)
            {
                CallMove();
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && _curArrow == Arrow.Down)
            {
                CallMove();
            }
        }
    }

    private void GenerateObjects()
    {
        _curStep = 0;
        _canMove = true;
        objectsParent.transform.position = _defParentPosition;
        _curArrow = GetCurArrow();

        if (_objectsList.Count != 0)
        {
            _objectsList.ForEach(ob => { Destroy(ob); });
            _objectsList.Clear();
        }

        int rand;
        for (int i = 0; i < objects.Length; i++)
        {
            rand = Random.Range(0, prefabs.Length);
            GameObject newObject = Instantiate(prefabs[rand], objects[i].transform);
            _objectsList.Add(newObject);
        }
    }

    private Arrow GetCurArrow()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0:
                _arrowImage.sprite = arrowSprites[0];
                _arrowTransform.rotation = Quaternion.Euler(0, 0, 90);
                return Arrow.Left;
            case 1:
                _arrowImage.sprite = arrowSprites[1];
                _arrowTransform.rotation = Quaternion.Euler(0, 0, -90);
                return Arrow.Right;
            case 2:
                _arrowImage.sprite = arrowSprites[2];
                _arrowTransform.rotation = Quaternion.Euler(0, 0, 0);
                return Arrow.Up;
            case 3:
                _arrowImage.sprite = arrowSprites[3];
                _arrowTransform.rotation = Quaternion.Euler(0, 0, 180);
                return Arrow.Down;
        }
        return Arrow.None;
    }

    private void CallMove()
    {
        _canMove = false;
        _curStep += 1;
        _curArrow = GetCurArrow();

        if (_curStep == stepsCount + 1)
        {
            _score += 1;
            scoreText.text = $"{_score}/{winScore}";
            SoundManager.instance.PlayAudioClip(clientClip, transform, clientClipVolume);

            if (_score == winScore)
            {
                doneCanvas.SetActive(true);

                SkufHandler.instance.ChangeHunger(-hungerImpact);
                SkufHandler.instance.ChangeMoney(moneyImpact);

                YG.YandexGame.SaveProgress();
            }
            else
            {
                GenerateObjects();
            }
        }
        else
        {
            SoundManager.instance.PlayAudioClip(peepClip, transform, peepClipVolume);
            float destination = (endTransform.position.x - _defParentPosition.x) / stepsCount * _curStep + _defParentPosition.x;
            StartCoroutine(Move(destination));
        }
    }

    private IEnumerator Move(float destination)
    {
        while (objectsParent.position.x > destination)
        {
            objectsParent.Translate(Vector2.left * objectsParentSpeed * Time.deltaTime);
            line.sizeDelta += new Vector2(objectsParentSpeed/13, 0);
            yield return null;
        }

        _canMove = true;
    }
}
