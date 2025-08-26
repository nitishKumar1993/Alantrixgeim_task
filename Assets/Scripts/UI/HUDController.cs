using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatchingGame
{
    public class HUDController : MonoBehaviour
    {

        [SerializeField]
        TextMeshProUGUI matchText;

        [SerializeField]
        TextMeshProUGUI movesText;

        string matchTextFormat = "Matches : {0}";
        string moveTextFormat = "Turns : {0}";

        // Start is called before the first frame update
        void Start()
        {

        }

        public void ShowMoves(int moves)
        {
            movesText.text = string.Format(moveTextFormat, moves.ToString("D2"));
        }
        public void ShowScore(int score)
        {
            matchText.text = string.Format(matchTextFormat, score.ToString("D2"));
        }

        public void ShowWinDialog()
        {

        }

        public void ShowLoseDialog()
        {

        }
    }
}