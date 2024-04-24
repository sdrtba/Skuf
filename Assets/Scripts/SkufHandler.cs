using UnityEngine;
using UnityEngine.UI;

public class SkufHandler : MonoBehaviour
{
    public static SkufHandler instance = null;

    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Text moneyText;
    
    private int score;
    private int hunger;
    private int money;

    private int _maxScore = 600;
    private int _maxHunger = 100;

    private int _minScore = 0;
    private int _minHunger = 0;

    

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
        score = _minScore;
        hunger = _maxHunger;
        scoreSlider.value = score;
        hungerSlider.value = hunger;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeScore(40);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeHunger(10);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeScore(-40);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeHunger(-10);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeMoney(13);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeMoney(-13);
        }
    }

    private void ChangeScore(int count)
    {
        if (score + count <= 600 && score + count >= 0)
        {
            score += count;
            scoreSlider.value = score;
        }
    }

    private void ChangeHunger(int count)
    {
        if (hunger + count <= 100 && hunger + count >= 0)
        {
            hunger += count;
            hungerSlider.value = hunger;
        }
    }

    private void ChangeMoney(int count)
    {
        if (money + count >= 0)
        {
            money += count;
            moneyText.text = money.ToString();
        }
    }
}
