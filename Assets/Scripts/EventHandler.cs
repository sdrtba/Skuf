using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    public void OpenScene(int sceneId) => SceneManager.LoadScene(sceneId);
}
