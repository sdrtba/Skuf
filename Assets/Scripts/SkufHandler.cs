using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkufHandler : MonoBehaviour
{
    public static SkufHandler instance = null;

    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Text moneyText;

    public int score;
    public int hunger;
    public int money;
    public int bearCount;
    public int foodCount;

    public int maxScore = 600;
    public int maxHunger = 150;
    public int minScore = 0;
    public int minHunger = 0;

    public bool isBirdActive = true;
    private int _birdRegen = 10; //?
    private int _birdHungerImpact = 20; //?
    private int _birdScoreImpact = 10; //?



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
        if (Input.GetKeyDown(KeyCode.Q)) ChangeScore(40);
        else if (Input.GetKeyDown(KeyCode.W)) ChangeHunger(10);
        else if (Input.GetKeyDown(KeyCode.A)) ChangeScore(-40);
        else if (Input.GetKeyDown(KeyCode.S)) ChangeHunger(-10);
        else if (Input.GetKeyDown(KeyCode.E)) ChangeMoney(13);
        else if (Input.GetKeyDown(KeyCode.D)) ChangeMoney(-13);
    }

    public void ChangeScore(int count)
    {  
        if (score + count <= minScore) { score = minScore; }
        else if (score + count >= maxScore) { score = maxScore; }
        else
        {
            score += count;
            scoreSlider.value = score;
        }
    }

    public void ChangeHunger(int count)
    {
        if (hunger + count <= minHunger) { hunger = minHunger; }
        else if (hunger + count >= maxHunger) { hunger = maxHunger; }
        else
        {
            hunger += count;
            hungerSlider.value = hunger;
        }
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

    public void EatBird() => StartCoroutine(EatBirdC());

    private IEnumerator EatBirdC()
    {
        isBirdActive = false;
        ChangeHunger(_birdHungerImpact);
        ChangeScore(_birdScoreImpact);
        yield return new WaitForSeconds(_birdRegen);

        isBirdActive = true;
    }
}
