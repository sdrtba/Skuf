using UnityEngine;
using UnityEngine.UI;

public class TankScript : MonoBehaviour
{
    [Header("Coefficients")]
    [Range(0f, 100f)][SerializeField] private float speed;
    [Range(0f, 100f)][SerializeField] private float cooldown;
    [Range(0, 100)][SerializeField] private int maxDelay;

    [Header("System")]
    [SerializeField] private TanksHandler tanksHandler;
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform newPos;
    [SerializeField] private Sprite deadSprite;

    public static bool _tankActive = true;
    private bool _isShooting = false;
    private bool _isDowing = false;
    private bool _canShoot = false;
    private bool _isShoted = false;
    private bool _isUping = true;
    private float offset = 0.4f;
    private float _time = 0;
    private Sprite defaultSprite;
    private Vector3 defaultPos;
    private float _timer;


    private void Start()
    {
        _tankActive = true;
        defaultPos = GetComponent<Transform>().position;
        defaultSprite = GetComponent<Image>().sprite;
        _timer = Random.Range(0, maxDelay);
    }

    private void Update()
    {
        if (_tankActive)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (_isUping)
                {
                    transform.position = Vector3.Lerp(transform.position, newPos.position, speed * Time.deltaTime);
                    if (transform.position.y >= newPos.position.y - offset / 5)
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
                    if (transform.position.y <= defaultPos.y + offset / 5)
                    {
                        _isDowing = false;
                        _isShooting = false;
                        _isUping = true;
                        _isShoted = false;
                        _timer = Random.Range(0, maxDelay);
                        ChangeSprite();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canShoot && !_isShoted)
            {
                Die();
                if (tanksHandler.IncreaseScore()) Stop();
            }
        }
    }

    private void Stop()
    {
        _tankActive = false;
    }

    private void Die()
    {
        _isShoted = true;
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
