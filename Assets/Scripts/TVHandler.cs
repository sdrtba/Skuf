using UnityEngine;

public class TVHandler : MonoBehaviour
{
    [SerializeField] private GameObject skuf;
    [SerializeField] private float scoreImpact = 0.5f; //?
    [SerializeField] private float hungerImpact = 1f; //?
    private float _localHunger = 0;
    private float _localScore = 0;
    private bool _isActive = false;

    public void ToggleActive() => _isActive = !_isActive;

    private void FixedUpdate()
    {
        if (_isActive && SkufHandler.instance.hunger >= 100)
        {
            _localHunger += hungerImpact;
            _localScore += scoreImpact;
            if (_localHunger > 1.5)
            {
                SkufHandler.instance.ChangeHunger(-(int)_localHunger);
                _localHunger = 0;
            }
            if (_localScore > 1.5)
            {
                SkufHandler.instance.ChangeScore((int)_localScore);
                _localScore = 0;
            }
        }
    }
}
