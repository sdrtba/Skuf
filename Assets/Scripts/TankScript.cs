using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TankScript : MonoBehaviour
{
    [Header("Coefficients")]
    [Range(0, 100)][SerializeField] private int maxDelay;
    [Range(0, 100)][SerializeField] private float speed;
    [Range(0, 100)][SerializeField] private float cooldown;


    [Header("System")]
    [SerializeField] private TanksHandler tanksHandler;
    [SerializeField] private RectTransform newPos;
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private GameObject shot;
    public static bool _tankActive = true;
    private bool _inTrigger = false;
    private bool _canShoot = false;
    private bool _isAlive = true;
    private RectTransform _rectTransform;
    private Sprite _defaultSprite;
    private Image _image;
    private float _defY;


    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defY = _rectTransform.anchoredPosition.y;

        _image = GetComponent<Image>();
        _defaultSprite = _image.sprite;

        StartCoroutine(StartLoop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _inTrigger && _isAlive)
        {
            Die();
        }

        if (!_tankActive)
        {
            GetComponent<TankScript>().enabled = false;
        }
    }

    private IEnumerator StartLoop()
    {
        float timer = Random.Range(2, maxDelay);
        yield return new WaitForSeconds(timer);
        if (tanksHandler.tanksCount < tanksHandler.maxTankPerTime){
            tanksHandler.tanksCount += 1;
            StartCoroutine(MoveUp());
        }
        else
        {
            StartCoroutine(StartLoop());
        }
    }

    private IEnumerator MoveUp()
    {
        if (_isAlive)
        {
            while (_rectTransform.anchoredPosition.y < newPos.anchoredPosition.y && _isAlive)
            {
                _rectTransform.Translate(Vector2.up * speed * Time.deltaTime);
                yield return null;
            }

            _canShoot = true;
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator MoveDown()
    {
        while (_rectTransform.anchoredPosition.y > _defY)
        {
            _rectTransform.Translate(Vector2.down * speed * Time.deltaTime);
            yield return null;
        }

        _isAlive = true;
        _image.sprite = _defaultSprite;
        
        StartCoroutine(StartLoop());
    }

    private IEnumerator Shoot()
    {
        if (_isAlive)
        {
            while (_canShoot && _isAlive)
            {
                Instantiate(shot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);

                if (tanksHandler.GetDamage())
                {
                    Stop();
                    yield break;
                }
                else yield return new WaitForSeconds(cooldown);
            }
        }
    }

    private void Die()
    {
        _isAlive = false;
        _canShoot = false;
        _image.sprite = deadSprite;
        tanksHandler.tanksCount -= 1;

        StartCoroutine(MoveDown());
        if (tanksHandler.IncreaseScore()) Stop();
        else StartCoroutine(MoveDown());
    }

    private void Stop()
    {
        _tankActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("aim")) _inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("aim")) _inTrigger = false;
    }
}
