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

    private int _maxScore = 600;
    private int _maxHunger = 150;

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

    public void ChangeScore(int count)
    {
        if (score + count <= _maxScore && score + count >= _minHunger)
        {
            score += count;
            scoreSlider.value = score;
        }
    }

    public void ChangeHunger(int count)
    {
        if (hunger + count <= _maxHunger && hunger + count >= _minHunger)
        {
            hunger += count;
            hungerSlider.value = hunger;
        }
    }

    public void ChangeMoney(int count)
    {
        if (money + count >= 0)
        {
            money += count;
            moneyText.text = money.ToString();
        }
    }
}
