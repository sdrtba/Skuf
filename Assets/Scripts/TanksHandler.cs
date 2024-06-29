using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TanksHandler : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private Slider hpSlider;
    private Rigidbody2D _aimRb;
    private float _horizontal;
    private float _vertical;
    private float _speed = 200;

    private void Start()
    {
        _aimRb = cursor.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        _aimRb.AddForce(new Vector3(_horizontal * _speed * Time.deltaTime, _vertical * _speed * Time.deltaTime, 0));
    }
}
