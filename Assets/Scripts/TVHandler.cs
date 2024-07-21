using UnityEngine;

public class TVHandler : MonoBehaviour
{
    [SerializeField] private GameObject skuf;
    [SerializeField] private int hungerByScoreImpact;
    [SerializeField] private float speed;
    private bool _isActive = false;
    private float _defSpeed;

    public void ToggleActive() => _isActive = !_isActive;

    private void Start() => _defSpeed = speed;

    private void FixedUpdate()
    {
        if (_isActive && SkufHandler.instance.hunger >= SkufHandler.instance.maxHunger*0.66)
        {
            speed -= Time.deltaTime;
            if (speed < 0)
            {
                SkufHandler.instance.ChangeHunger(-hungerByScoreImpact);
                SkufHandler.instance.ChangeScore(1);
                speed = _defSpeed;
            }
        }
    }
}
