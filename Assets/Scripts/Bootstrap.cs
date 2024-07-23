using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Canvas HUD;
    private bool _isActive;
    private int _sceneId;

    private void Start() => SceneManager.LoadScene(1);

    private void FixedUpdate()
    {
        _sceneId = SceneManager.GetActiveScene().buildIndex;
        if (_sceneId == 5 || _sceneId == 6 || _sceneId == 7 || _sceneId == 8)
        {
            _isActive = false;
        }
        else
        {
            _isActive = true;
        }

        if (_isActive != HUD.enabled) HUD.enabled = !HUD.enabled;

    }
}
