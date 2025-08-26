using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MatchingGame
{
    public class HUDController : MonoBehaviour
    {

        public static UnityAction<int, int, bool> OnGameStartAction;
        public static UnityAction OnGoHomeAction;

        [Header("Main Menu")]
        [SerializeField]
        GameObject mainMenuHUD;
        [SerializeField]
        TMP_InputField columnInputField;
        [SerializeField]
        TMP_InputField rowsInputField;
        [SerializeField]
        Button startBtn;
        [SerializeField]
        TextMeshProUGUI errorText;
        [SerializeField]
        Toggle negativeScoreToggle;

        private string emptyFieldText = "One or more fields are empty or wrong. Please check and try again";

        [Header("InGame")]
        [SerializeField]
        GameObject inGameHUD;
        [SerializeField]
        GameObject playAgainPanel;
        [SerializeField]
        Button playAgainBtn;
        [SerializeField]
        Button homeBtn;
        [SerializeField]
        TextMeshProUGUI matchText;
        [SerializeField]
        TextMeshProUGUI movesText;
        string matchTextFormat = "Score : {0}";
        string moveTextFormat = "Turns : {0}";

        private void Awake()
        {
            startBtn.onClick.AddListener(OnStartGameBtnClicked);
            playAgainBtn.onClick.AddListener(OnStartGameBtnClicked);
            homeBtn.onClick.AddListener(OnHomeBtnClicked);
        }

        void OnStartGameBtnClicked()
        {
            if(string.IsNullOrEmpty(columnInputField.text) || string.IsNullOrEmpty(rowsInputField.text) )
            {
                errorText.text = emptyFieldText;
                return;
            }

            int rows = int.Parse(columnInputField.text);
            int column = int.Parse(rowsInputField.text);

            if (rows <= 1 || column <= 1)
            {
                errorText.text = emptyFieldText;
                return;
            }

            errorText.text = "";
            OnGameStartAction?.Invoke(rows, column, negativeScoreToggle.isOn);
        }

        private void OnHomeBtnClicked()
        {
            OnGoHomeAction?.Invoke();
        }

        public void ShowMainMenu()
        {
            mainMenuHUD.SetActive(true);
            inGameHUD.SetActive(false);
        }

        public void ShowInGameHUD()
        {
            mainMenuHUD.SetActive(false);
            inGameHUD.SetActive(true);
        }

        public void ShowHidePlayAgainPanel(bool show)
        {
            playAgainPanel.SetActive(show);
        }

        public void ShowMoves(int moves)
        {
            movesText.text = string.Format(moveTextFormat, moves.ToString("D2"));
        }
        public void ShowScore(int score)
        {
            matchText.text = string.Format(matchTextFormat, score.ToString("D2"));
        }
    }
}