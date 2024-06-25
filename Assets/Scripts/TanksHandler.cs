using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TanksHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] tanks;
    [SerializeField] private GameObject cursor;
    [SerializeField] private Slider hpSlider;
    private float _horizontal;
    private float _vertical;
    private float _speed = 5;
    private int _damage;

    private void Start()
    {
        foreach (var t in tanks) { t.SetActive(false); }

        int r = Random.Range(0, tanks.Length);
        var tank = tanks[r];
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        cursor.transform.Translate(new Vector3(_horizontal * _speed * Time.deltaTime, _vertical * _speed * Time.deltaTime, 0));
    }

    private IEnumerator TakeTank()
    {
        int r = Random.Range(0, tanks.Length);
        var tank = tanks[r];

        tank.SetActive(true);
        Vector3 pos = tank.transform.position;
        tank.transform.position = Vector3.Lerp(pos, pos + new Vector3(0, 1), Time.deltaTime);
        yield return new WaitForSeconds(1);


    }
}
