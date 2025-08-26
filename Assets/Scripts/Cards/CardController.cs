using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button cardBtn;

    public string CardId { get; private set; }
    public CardView View { get; private set; }

    private GameManager game;
    public bool IsFaceUp { get; private set; }

    private void Awake()
    {
        cardBtn.onClick.AddListener(OnCardClicked);

        View = GetComponent<CardView>();
    }

    /// <summary>
    /// Initialize the card with data
    /// </summary>
    /// <param name="gm"></param>
    /// <param name="id"></param>
    /// <param name="face"></param>
    /// <param name="back"></param>
    public void Init(GameManager gm, string id, Sprite face)
    {
        game = gm;
        CardId = id;
        View.SetFace(face);
        View.Flip(false, 0f);
    }

    private void OnCardClicked()
    {
        View.Flip(true, 0.5f);
    }
}
