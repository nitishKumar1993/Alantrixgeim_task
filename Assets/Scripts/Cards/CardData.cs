using UnityEngine;

namespace MatchingGame
{
    [CreateAssetMenu(menuName = "MatchingGame/Card Data", fileName = "CardData")]
    public class CardData : ScriptableObject
    {
        public string cardId;
        public Sprite cardSprite;
    }
}