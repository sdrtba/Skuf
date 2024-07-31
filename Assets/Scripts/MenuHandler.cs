using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MenuHandler : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private GameObject rateBtn;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private Slider musicSlider;


    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);

        audioMixer.GetFloat("soundFXVolume", out float soundFXValue);
        soundFXSlider.value = Mathf.Pow(10, soundFXValue / 20);

        audioMixer.GetFloat("musicVolume", out float musicValue);
        musicSlider.value = Mathf.Pow(10, musicValue / 20);

        if (!YandexGame.EnvironmentData.reviewCanShow)
        {
            rateBtn.SetActive(false);
        }
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    public void Play(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void Rate()
    {
        YandexGame.ReviewShow(true);
    }

    public void Delete()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}
