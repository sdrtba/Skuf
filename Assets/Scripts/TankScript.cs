using UnityEngine;
using UnityEngine.UI;

public class TankScript : MonoBehaviour
{
    [SerializeField] private TanksHandler tanksHandler;

    [SerializeField] private GameObject aim;

    [SerializeField] private Transform newPos;
    private Vector3 defaultPos;
    [SerializeField] private Sprite deadSprite;
    private Sprite defaultSprite;
    [SerializeField] private GameObject shot;
    [SerializeField] private float speed;
    [SerializeField] private float cooldown;
    [SerializeField] private int maxDelay;
    private float offset = 0.4f;
    private float _timer;
    private bool _isUping = true;
    private bool _isShooting = false;
    private bool _isDowing = false;
    private float _time = 0;
    private bool _canShoot = false;
    private bool _active = true;

    private void Start()
    {
        defaultPos = GetComponent<Transform>().position;
        defaultSprite = GetComponent<Image>().sprite;
        _timer = Random.Range(0, maxDelay);
    }

    private void Update()
    {
        if (_active) _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            if (_isUping)
            {
                transform.position = Vector3.Lerp(transform.position, newPos.position, speed * Time.deltaTime);
                if (transform.position.y >= newPos.position.y - offset/5)
                {
                    _isUping = false;
                    _isDowing = false;
                    _isShooting = true;
                }
            }
            else if (_isShooting)
            {
                _time += Time.deltaTime;
                if (_time > cooldown)
                {
                    Instantiate(shot, transform.position + new Vector3(0, offset, 0), Quaternion.identity);
                    _time = 0;

                    if (tanksHandler.GetDamage()) Stop();
                }
            }
            else if (_isDowing)
            {
                transform.position = Vector3.Lerp(transform.position, defaultPos, speed * Time.deltaTime);
                if (transform.position.y <= defaultPos.y + offset/5)
                {
                    _isDowing = false;
                    _isShooting = false;
                    _isUping = true;
                    _timer = Random.Range(0, maxDelay);
                    ChangeSprite();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _canShoot)
        {
            Die();
            if (tanksHandler.IncreaseScore()) Stop();
        }
    }

    private void Stop()
    {
        _timer = 1;
        _active = false;
    }

    private void Die()
    {
        _isUping = false;
        _isShooting = false;
        _isDowing = true;
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        Image Image = GetComponent<Image>();
        if (Image.sprite == defaultSprite)
        {
            Image.sprite = deadSprite;
        }
        else
        {
            Image.sprite = defaultSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("aim"))
        {
            _canShoot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("aim"))
        {
            _canShoot = false;
        }
    }
}
