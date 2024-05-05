using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance = null;

    [SerializeField] private GameObject bird;
    private bool _isBirdActive = true;

    private int _regenTime = 10;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void OpenScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void EatBird()
    {
        if (SkufHandler.instance.hunger == 0 && _isBirdActive)
        {
            StartCoroutine(EatBirdC());
        }
    }

    private IEnumerator EatBirdC()
    {
        var button = bird.GetComponent<Button>();
        var sprite = bird.GetComponent<Image>();

        _isBirdActive = false;
        button.interactable = false;
        sprite.color = new Color(255, 255, 255, 0);
        SkufHandler.instance.ChangeHunger(20);
        yield return new WaitForSeconds(_regenTime);

        _isBirdActive = true;
        sprite.color = new Color(255, 255, 255, 255);
    }
}
