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

        void Start()
        {
            NewGame();
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

        // Called from CardController.OnMouseUpAsButton (or via IPointerClickHandler in a UI version)
        public void OnCardSelected(CardController card)
        {
            if (card == null) return;
            if (State != GameState.Idle) return;
            if (card == first || card == second) return; 

            // if card already matched/disabled, ignore
            var col = card.GetComponent<Collider2D>();
            if (col != null && !col.enabled) return;

            State = GameState.Busy;

            // flip up
            card.View.Flip(true, config.cardFlipDuration);
            if (audioMgr) audioMgr.PlaySfx(config ? config.flipSfx : null);

            if (first == null)
            {
                first = card;
                State = GameState.Idle;
                return;
            }

            // second selection
            second = card;
            scoreMgr.AddMove();

            StartCoroutine(CheckMatch_CR());
        }

        private IEnumerator CheckMatch_CR()
        {
            // wait until flip animation mostly done
            yield return new WaitForSeconds(config.cardFlipDuration);

            bool matched = first.CardId == second.CardId;

            if (matched)
            {
                if (audioMgr) audioMgr.PlaySfx(config ? config.matchSfx : null);

                // disable both so they cannot be clicked again
                var c1 = first.GetComponent<Collider2D>();
                var c2 = second.GetComponent<Collider2D>();
                if (c1) c1.enabled = false;
                if (c2) c2.enabled = false;


                // scoring
                int add = config.baseScorePerMatch;
                scoreMgr.AddScore(add);

                first = null;
                second = null;
                State = GameState.Idle;

                if (scoreMgr.CurrentMatches * 2 >= cards.Count)
                {
                    Win();
                }
            }
            else
            {
                if (audioMgr) audioMgr.PlaySfx(config ? config.mismatchSfx : null);

                // small reveal delay so the player can see both faces
                yield return new WaitForSeconds(config.cardFlipDuration);

                // flip back down
                first.View.Flip(false, config.cardFlipDuration);
                second.View.Flip(false, config.cardFlipDuration);

                // wait until flips complete before unlocking
                yield return new WaitForSeconds(config.cardFlipDuration);

                first = null;
                second = null;
                State = GameState.Idle;
            }
        }

        private void Win()
        {
            State = GameState.Win;
            if (audioMgr) audioMgr.PlaySfx(config ? config.winSfx : null);
            if (hudMgr) hudMgr.ShowWinDialog();
        }

        private void Lose()
        {
            State = GameState.Lose;
            if (hudMgr) hudMgr.ShowLoseDialog();
        }

        public void Restart()
        {
            if (State == GameState.Busy) return; // avoid reset mid-animation
            NewGame();
        }
    }
}