using UnityEngine;

namespace MatchingGame
{
    [CreateAssetMenu(menuName = "MatchingGame/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Board")]
        public int columns = 4;
        public int rows = 3;
        public float cardSpacing;

        [Header("Cards data")]
        public CardData[] allCardsData;
        public float cardFlipDuration;

        [Header("SFX")]
        public AudioClip flipSfx, matchSfx, mismatchSfx, winSfx, looseSfx;
    }
}