using UnityEngine;

public class TVHandler : MonoBehaviour
{
    [SerializeField] private GameObject skuf;
    [Range(0, 100)][SerializeField] private int hungerByScoreImpact;
    [Range(0f, 100f)][SerializeField] private float speed;
    private bool _isActive = false;
    private float _defSpeed;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(true);
        _defSpeed = speed;
    }

    public void ToggleActive() => _isActive = !_isActive;

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
