using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum Arrow{
    Left,
    Right,
    Up,
    Down,
    None
}

public class SellerHandler : MonoBehaviour
{
    [SerializeField] private Text indexText;
    private List<Arrow> _arrowList;
    private Arrow _curArrow;
    private float _index;

    private void CreateClient()
    {
        _arrowList = new List<Arrow>();
        for (int i = 0; i < Random.Range(3, 10); i++)
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
        }
        _curArrow = _arrowList[0];
        Debug.Log(_curArrow);

    }

    private void ChangeItem()
    {
        _arrowList.RemoveAt(0);

        if (_arrowList.Count == 0) {
            _index += 1;
            indexText.text = $"{_index}/10";

            if (_index >= 10)
            {
                Debug.Log("Ok");
                _curArrow = Arrow.None;
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
        CreateClient();
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.LeftArrow) && _curArrow == Arrow.Left)
        {
            ChangeItem();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _curArrow == Arrow.Right)
        {
            ChangeItem();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && _curArrow == Arrow.Up)
        {
            ChangeItem();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _curArrow == Arrow.Down)
        {
            ChangeItem();
        }
    }
} 
