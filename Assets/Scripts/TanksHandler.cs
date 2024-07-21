using UnityEngine;
using UnityEngine.UI;

public class TanksHandler : MonoBehaviour
{
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject aim;
    [SerializeField] private float damage;
    [SerializeField] private int winScore;
    [SerializeField] private float aimSpeed;
    private int score = 0;
    private Rigidbody2D _aimRb;
    private float _horizontal;
    private float _vertical;
    private bool _active = true;

    private void Start()
    {
        _aimRb = aim.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (_active)
            _aimRb.AddForce(new Vector3(_horizontal * aimSpeed * Time.deltaTime, _vertical * aimSpeed * Time.deltaTime, 0));
    }

    public bool GetDamage()
    {
        hpSlider.value -= damage;
        if (hpSlider.value <= 0)
        {
            _active = false;
            loseCanvas.SetActive(true);
            return true;
        }
        return false;
    }

    public bool IncreaseScore()
    {
        score += 1;
        if (score >= winScore) {
            _active = false;
            winCanvas.SetActive(true);
            return true;
        }
        return false;
    }
}
