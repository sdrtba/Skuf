using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TankScript : MonoBehaviour
{
    [SerializeField] private Sprite deadSprite;
    private bool _isMoving = false;
    private bool _isShooting = true;
    private bool _isDead = false;
    private float offset = 1.5f;
    private float _speed = 0.0007f;
    private float _baseY;
    private float _downY;
    private int _cooldown;

    private void Start()
    {
        _baseY = transform.position.y;
        transform.position -= new Vector3(0, offset, 0);
        _downY = transform.position.y;

        int r = Random.Range(0, 5);
        if (r == 1 || r == 2) _isMoving = true;
    }

    private void Update()
    {
        if (_isMoving)
        {
            transform.position += new Vector3(0, _speed, 0);
        }
        else if (_isShooting)
        {
            StartCoroutine(Shoot());
        }
        if (_isDead)
        {
            transform.position = new Vector3(0, _speed, 0);
        }
        if (transform.position.y > _baseY)
        {
            _isMoving = false;
        }
        if (transform.position.y < _downY)
        {
            _isDead = false;
        }
    }

    private IEnumerator Shoot()
    {
        _isShooting = false;
        yield return new WaitForSeconds(_cooldown);
        Debug.Log("Shoot");
        _isShooting = true;
    }

    private void Die()
    {
        gameObject.GetComponent<Image>().sprite = deadSprite;
    }
}
