using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkufHandler : MonoBehaviour
{
    public static SkufHandler instance = null;

    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Text moneyText;

    [NonSerialized]public int score;
    [NonSerialized] public int hunger;
    [NonSerialized] public int money;
    [NonSerialized] public int bearCount;
    [NonSerialized] public int foodCount;

    [NonSerialized] public int maxScore = 600;
    [NonSerialized] public int maxHunger = 150;
    [NonSerialized] public int minScore = 0;
    [NonSerialized] public int minHunger = 0;

    [NonSerialized] public bool isBirdActive = true;


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
        InitializeManager();
    }

    private void InitializeManager()
    {
        score = minScore;
        hunger = maxHunger;
        scoreSlider.value = score;
        hungerSlider.value = hunger;
        bearCount = 0;
        foodCount = 0;
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

    public void SetHUDVisibility(bool isEnable)
    {
        HUDCanvas.enabled = isEnable;
    }

    public void ChangeScore(int count)
    {
        if (score + count <= minScore) { score = minScore; }
        else if (score + count >= maxScore) { score = maxScore; }
        else { score += count; }

        scoreSlider.value = score;
    }

    public void ChangeHunger(int count)
    {
        if (hunger + count <= minHunger) { hunger = minHunger; }
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
