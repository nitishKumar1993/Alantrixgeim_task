using System.Collections.Generic;
using UnityEngine;

namespace MatchingGame
{
    public static class DeckBuilder
    {
        public static List<(string id, Sprite face)> Build(GameConfig cfg, int neededPairs)
        {
            var pool = new List<CardData>(cfg.allCardsData);
            if (pool.Count < neededPairs) Debug.LogWarning("Not enough unique cards; will reuse.");

            var deck = new List<(string, Sprite)>();
            for (int i = 0; i < neededPairs; i++)
            {
                var data = pool[i % pool.Count];
                deck.Add((data.cardId, data.cardSprite));
                deck.Add((data.cardId, data.cardSprite));
            }

            for (int i = 0; i < deck.Count; i++)
            {
                int r = Random.Range(i, deck.Count);
                (deck[i], deck[r]) = (deck[r], deck[i]);
            }
            return deck;
        }
    }
}