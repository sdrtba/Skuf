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
    [SerializeField] private GameObject itemsParent;
    [SerializeField] private GameObject[] curItems;
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private GameObject doneCanvas;
    [SerializeField] private Text indexText;
    [SerializeField] private int hungerImpact = 20; //?
    [SerializeField] private int moneyImpact = 25; //?
    private Transform _defItemsTransform;
    private List<Arrow> _arrowList;
    private Arrow _curArrow;
    private float _index;
    private int _itemsCount = 5;

    private void CreateClient()
    {
        _arrowList = new List<Arrow>();
        for (int i = 0; i < _itemsCount; i++)
        {
            int r = Random.Range(0, 4);
            switch (r)
            {
                case 0:
                    _arrowList.Add(Arrow.Left);
                    break;
                case 1:
                    _arrowList.Add(Arrow.Right);
                    break;
                case 2:
                    _arrowList.Add(Arrow.Up);
                    break;
                case 3:
                    _arrowList.Add(Arrow.Down);
                    break;
            }

            curItems[i].GetComponent<Image>().sprite = itemSprites[Random.Range(0, itemSprites.Length)];
        }
        _curArrow = _arrowList[0];
        Debug.Log(_curArrow);
    }

    private void ChangeItem()
    {
        _arrowList.RemoveAt(0);
        itemsParent.transform.position -= new Vector3(0.5f, 0, 0);

        if (_arrowList.Count == 0)
        {
            itemsParent.transform.position = _defItemsTransform.position;

            _index += 1;
            indexText.text = $"{_index}/10";

            if (_index >= 10)
            {
                Debug.Log("Ok");
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
        else
        {
            _curArrow = _arrowList[0];
            Debug.Log(_curArrow);
        }
    }

    private void Start()
    {
        _defItemsTransform = itemsParent.transform;
        CreateClient();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && _curArrow == Arrow.Left)
        {
            ChangeItem();
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && _curArrow == Arrow.Right)
        {
            ChangeItem();
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _curArrow == Arrow.Up)
        {
            ChangeItem();
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && _curArrow == Arrow.Down)
        {
            ChangeItem();
        }
    }
}
