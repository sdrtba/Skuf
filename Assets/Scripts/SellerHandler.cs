using System.Collections;
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
    [SerializeField] private RectTransform tape;
    [SerializeField] private GameObject itemsParent;
    [SerializeField] private GameObject[] curItems;
    [SerializeField] private GameObject[] itemObjects;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private Sprite[] arrowSprites;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text indexText;
    [SerializeField] private int hungerImpact = 20; //?
    [SerializeField] private int moneyImpact = 25; //?
    private Vector3 _defParentPosition;
    private Arrow _curArrow;
    private float _index;
    private int _itemsCount = 5;
    private bool _isDone = false;

    private Arrow GetCurArrow()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0:
                arrowObject.GetComponent<Image>().sprite = arrowSprites[0];
                arrowObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 90);
                return Arrow.Left;
            case 1:
                arrowObject.GetComponent<Image>().sprite = arrowSprites[1];
                arrowObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, -90);
                return Arrow.Right;
            case 2:
                arrowObject.GetComponent<Image>().sprite = arrowSprites[2];
                arrowObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                return Arrow.Up;
            case 3:
                arrowObject.GetComponent<Image>().sprite = arrowSprites[3];
                arrowObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 180);
                return Arrow.Down;
        }
        return Arrow.None;
    }

    private void CreateClient()
    {
        _curArrow = GetCurArrow();
        itemsParent.transform.position = _defParentPosition;
        _isDone = false;

        for (int i = 0; i < _itemsCount; i++)
        {
            int r = Random.Range(0, itemObjects.Length);
            curItems[i].GetComponent<Image>().sprite = itemObjects[r].GetComponent<Image>().sprite;
            curItems[i].GetComponent<RectTransform>().sizeDelta = itemObjects[r].GetComponent<RectTransform>().sizeDelta;
        }
    }

    private void ArrowClick()
    {
        StartCoroutine(Move());
        _curArrow = GetCurArrow();

        if (itemsParent.transform.position.x <= -15)
        {
            _isDone = true;
            StopAllCoroutines();
        }

        if (_isDone)
        {
            _index += 1;
            indexText.text = $"{_index}/5";

            if (_index >= 5)
            {
                _curArrow = Arrow.None;

                doneCanvas.SetActive(true);

                SkufHandler.instance.ChangeHunger(-hungerImpact);
                SkufHandler.instance.ChangeMoney(moneyImpact);
            }
            else
            {
                CreateClient();
            }
        }
    }

    private IEnumerator Move()
    {
        float newPos = itemsParent.transform.position.x - 1f;
        while (itemsParent.transform.position.x >= newPos)
        {
            itemsParent.transform.position -= new Vector3(0.1f, 0, 0);

            tape.sizeDelta += new Vector2(10f, 0);

            yield return new WaitForSeconds(0.02f);
        }
    }

    private void Start()
    {
        _defParentPosition = itemsParent.transform.position;
        CreateClient();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && _curArrow == Arrow.Left)
        {
            ArrowClick();
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && _curArrow == Arrow.Right)
        {
            ArrowClick();
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _curArrow == Arrow.Up)
        {
            ArrowClick();
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && _curArrow == Arrow.Down)
        {
            ArrowClick();
        }
    }
}
