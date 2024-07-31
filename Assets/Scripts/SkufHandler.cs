using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class SkufHandler : MonoBehaviour
{
    [NonSerialized] public static SkufHandler instance;

    [Header("Coefficients")]
    [SerializeField] public int maxScore;
    [SerializeField] public int maxHunger;

    [Header("System")]
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Text moneyText;

    [NonSerialized] public int score;
    [NonSerialized] public int hunger;
    [NonSerialized] public int money;
    [NonSerialized] public int bearCount;
    [NonSerialized] public int foodCount;

    [NonSerialized] public bool isBirdActive = true;


    private void OnEnable()
    {
        YandexGame.GetDataEvent += InitializeManager;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= InitializeManager;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (YandexGame.SDKEnabled == true) InitializeManager();
    }

    private void InitializeManager()
    {
        score = YandexGame.savesData.score;
        scoreSlider.value = score;

        hunger = YandexGame.savesData.hunger;
        hungerSlider.value = hunger;

        bearCount = YandexGame.savesData.bearCount;
        foodCount = YandexGame.savesData.foodCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) ChangeScore(40);
        else if (Input.GetKeyDown(KeyCode.J)) ChangeScore(-40);
        else if (Input.GetKeyDown(KeyCode.O)) ChangeHunger(10);
        else if (Input.GetKeyDown(KeyCode.K)) ChangeHunger(-10);
        else if (Input.GetKeyDown(KeyCode.P)) ChangeMoney(13);
        else if (Input.GetKeyDown(KeyCode.L)) ChangeMoney(-13);
        else if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }

    public void SetText(Text text, string firstValue, string secondValue)
    {
        LanguageYG lang = text.gameObject.GetComponent<LanguageYG>();

        if (YandexGame.lang == "ru")
        {
            text.text = lang.ru;
        }
        else if (YandexGame.lang == "tr")
        {
            text.text = lang.tr;
        }
        else
        {
            text.text = lang.en;
        }
        lang.enabled = false;

        text.text = text.text.Replace("{0}", firstValue).Replace("{1}", secondValue);
    }

    public void SetHUDVisibility(bool isEnable)
    {
        HUDCanvas.enabled = isEnable;
    }

    public void ChangeScore(int count)
    {
        if (score + count <= 0) { score = 0; }
        else if (score + count >= maxScore) { score = maxScore; }
        else { score += count; }

        scoreSlider.value = score;
    }

    public void ChangeHunger(int count)
    {
        if (hunger + count <= 0) { hunger = 0; }
        else if (hunger + count >= maxHunger) { hunger = maxHunger; }
        else { hunger += count; }

        hungerSlider.value = hunger;
    }

    public bool CanBuy(int count)
    {
        if (money - count < 0) { return false; }
        else { return true; }
    }

    public void ChangeMoney(int count)
    {

        money += count;
        moneyText.text = money.ToString();
    }

    public IEnumerator EatBird(int regenTime)
    {
        isBirdActive = false;
        yield return new WaitForSeconds(regenTime);
        isBirdActive = true;
    }
}
