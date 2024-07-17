using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanksHandler : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject aim;
    [SerializeField] private float damage;
    [SerializeField] private int winScore;
    private int score = 0;
    private Rigidbody2D _aimRb;
    private float _horizontal;
    private float _vertical;
    private float _aimSpeed = 200;

    private void Start()
    {
        _aimRb = aim.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        _aimRb.AddForce(new Vector3(_horizontal * _aimSpeed * Time.deltaTime, _vertical * _aimSpeed * Time.deltaTime, 0));
    }

    public bool GetDamage()
    {
        hpSlider.value -= damage;
        if (hpSlider.value <= 0)
        {
            Debug.Log("lose");
            return true;
        }
        return false;
    }

    public bool IncreaseScore()
    {
        score += 1;
        if (score >= winScore) {
            Debug.Log("win");
            return true;
        }
        return false;
    }
}
