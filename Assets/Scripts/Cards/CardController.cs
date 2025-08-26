using UnityEngine;
using UnityEngine.UI;

namespace MatchingGame
{
    public class CardController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button cardBtn;

        public string CardId { get; private set; }
        public CardView View { get; private set; }

        private GameManager gameManager;
        public bool IsFaceUp { get; private set; }

        private void Awake()
        {
            cardBtn.onClick.AddListener(OnCardClicked);

            View = GetComponent<CardView>();
        }

        private void OnCardClicked()
        {
            gameManager.OnCardSelected(this);
        }

        public void Init(GameManager gm, string id, Sprite face)
        {
            gameManager = gm;
            CardId = id;
            View.SetFace(face);
        }

        public void ShowFace()
        {
            View.ShowFace();
        }

        public void HideFace()
        {
            View.HideFace();
        }
    }
}
