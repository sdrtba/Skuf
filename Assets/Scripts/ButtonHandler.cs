using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite[] highlitedSprites;
    [SerializeField] private GameObject pushUpForm;
    [SerializeField] private GameObject skuf;

    private bool _isActive = false;
    private SpriteState _defSS;
    private Sprite _defSprite;


    private void Start()
    {
        if (sprites?.Length > 0)
        {
            _defSprite = GetComponent<Image>().sprite;
            _defSS = GetComponent<Button>().spriteState;
        }
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;

        if (skuf != null) skuf.SetActive(!skuf.activeSelf);
        if (pushUpForm != null) pushUpForm.SetActive(!pushUpForm.activeSelf);
        if (buttons?.Length > 0)
        {
            foreach (Button btn in buttons)
            {
                btn.interactable = !btn.interactable;
            }
        }
        if (sprites?.Length > 0)
        {
            if (_isActive)
            {
                int index = Random.Range(0, sprites.Length);
                gameObject.GetComponent<Image>().sprite = sprites[index];

                SpriteState _ss = new SpriteState();
                _ss.highlightedSprite = highlitedSprites[index];
                GetComponent<Button>().spriteState = _ss;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = _defSprite;
                GetComponent<Button>().spriteState = _defSS;
            }
        }
    }
}
