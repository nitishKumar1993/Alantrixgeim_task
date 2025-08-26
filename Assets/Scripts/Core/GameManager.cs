using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchingGame
{
    public enum GameState { Idle, Busy, Win, Lose }

    public class GameManager : MonoBehaviour
    {
        [Header("Configs")]
        public GameConfig config;

        [Header("Scene Refs")]
        public GridGenerator grid;
        public ScoreManager scoreMgr;
        public HUDController hudMgr;
        public AudioManager audioMgr;
        public RectTransform boardRoot;

        public int columns, rows;
        
        private List<CardController> cards = new List<CardController>();
        private CardController first;
        private CardController second;

        public GameState State { get; private set; } = GameState.Idle;

        private void Awake()
        {
            HUDController.OnGameStartAction += OnStartNewGameAction;
            HUDController.OnGoHomeAction += OnHomeBtnAction;
        }

        void Start()
        {
            hudMgr.ShowMainMenu();
        }

        private void OnStartNewGameAction(int _rows, int _columns)
        {
            columns = _columns;
            rows = _rows;

            if (hudMgr) hudMgr.ShowInGameHUD();
            if (hudMgr) hudMgr.ShowHidePlayAgainPanel(false);

            NewGame();
        }

        private void OnHomeBtnAction()
        {
            hudMgr.ShowMainMenu();
            StopAllCoroutines();
        }

        public void NewGame()
        {
            ClearBoard();
            first = null;
            second = null;

            scoreMgr.ResetScore();
            scoreMgr.ResetMoves();

            cards = grid.SpawnGrid(this, config, columns, rows);

            State = GameState.Busy;

            StartCoroutine(ShowAllCardsOnNewGame());
        }

        private IEnumerator ShowAllCardsOnNewGame()
        {
            cards.ForEach(obj => obj.ShowFace());

            yield return new WaitForSeconds(config.newGameCardShowDuration);

            cards.ForEach(obj => obj.HideFace());

            State = GameState.Idle;
        }

        private void ClearBoard()
        {
            if (!grid || !grid.gridRoot) return;

            for (int i = grid.gridRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(grid.gridRoot.GetChild(i).gameObject);
            }
            cards.Clear();
        }

        public void OnCardSelected(CardController card)
        {
            if (card == null) return;
            if (State != GameState.Idle) return;
            if (card == first || card == second) return; 

            var col = card.GetComponent<Collider2D>();
            if (col != null && !col.enabled) return;

            State = GameState.Busy;

            card.View.Flip(true, config.cardFlipDuration);
            if (audioMgr) audioMgr.PlaySfx(config ? config.flipSfx : null);

            if (first == null)
            {
                first = card;
                State = GameState.Idle;
                return;
            }

            second = card;
            scoreMgr.AddMove();

            StartCoroutine(CheckMatch_CR());

            first = null;
            second = null;
        }

        private IEnumerator CheckMatch_CR()
        {
            State = GameState.Idle;

            CardController firstCard = first;
            CardController secondCard = second;

            // wait until flip animation mostly done
            yield return new WaitForSeconds(config.cardFlipDuration);

            bool matched = firstCard.CardId == secondCard.CardId;

            if (matched)
            {
                if (audioMgr) audioMgr.PlaySfx(config ? config.matchSfx : null);

                // disable both so they cannot be clicked again
                var c1 = firstCard.GetComponent<Collider2D>();
                var c2 = secondCard.GetComponent<Collider2D>();
                if (c1) c1.enabled = false;
                if (c2) c2.enabled = false;

                int add = config.baseScorePerMatch;
                scoreMgr.AddScore(add);

                firstCard.transform.DOShakeScale(0.5f, 0.2f);
                secondCard.transform.DOShakeScale(0.5f, 0.2f);

                if (scoreMgr.CurrentMatches * 2 >= cards.Count)
                {
                    Win();
                }
            }
            else
            {
                if (audioMgr) audioMgr.PlaySfx(config ? config.mismatchSfx : null);

                firstCard.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
                secondCard.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);

                yield return new WaitForSeconds(0.5f);

                // flip back down
                firstCard.View.Flip(false, config.cardFlipDuration);
                secondCard.View.Flip(false, config.cardFlipDuration);

                // wait until flips complete before unlocking
                yield return new WaitForSeconds(config.cardFlipDuration);
            }
        }

        private void Win()
        {
            Debug.Log("Game Win");

            State = GameState.Win;
            if (audioMgr) audioMgr.PlaySfx(config ? config.winSfx : null);
            if (hudMgr) hudMgr.ShowHidePlayAgainPanel(true);
        }

        private void Lose()
        {
            Debug.Log("Game Lose");

            State = GameState.Lose;
        }

        public void Restart()
        {
            if (State == GameState.Busy) return; // avoid reset mid-animation
            NewGame();
        }
    }
}