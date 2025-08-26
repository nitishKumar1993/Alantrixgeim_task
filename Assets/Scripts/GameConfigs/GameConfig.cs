using UnityEngine;

namespace MatchingGame
{
    [CreateAssetMenu(menuName = "MatchingGame/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Game data")]
        public int baseScorePerMatch;

        [Header("Cards data")]
        public CardData[] allCardsData;
        public float cardFlipDuration;
        public float newGameCardShowDuration;

        [Header("SFX")]
        public AudioClip flipSfx, matchSfx, mismatchSfx, winSfx, looseSfx;
    }
}