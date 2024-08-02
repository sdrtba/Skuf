using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class EventHandler : MonoBehaviour
{
    private bool _wasAd = false;

    public void OpenScene(int sceneId)
    {
        YandexGame.FullscreenShow();
        if (!_wasAd) SceneManager.LoadScene(sceneId);
    }

    private void OnEnable()
    {
        YandexGame.OpenFullAdEvent += OpenAd;
        YandexGame.CloseFullAdEvent += CloseAd;
    }

    private void OnDisable()
    {
        YandexGame.OpenFullAdEvent -= OpenAd;
        YandexGame.CloseFullAdEvent -= CloseAd;
    }

    private void OpenAd()
    {
        _wasAd = true;
    }

    private void CloseAd()
    {
        _wasAd = false;
    }

}
