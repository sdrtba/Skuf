using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Canvas HUD;
    private bool _isActive;
    private int id;

    private void Start() => SceneManager.LoadScene(1);

    private void FixedUpdate()
    {
        id = SceneManager.GetActiveScene().buildIndex;
        if (id == 5 || id == 6 || id == 7)
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
