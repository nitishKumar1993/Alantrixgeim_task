using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace MatchingGame
{
    public class ScoreManager : MonoBehaviour
    {
        public HUDController hudMgr;

        public int currentMatches = 0;
        public int currentScore= 0;
        public int currentMoves = 0;

        public int CurrentMatches { get => currentMatches;}

        public void ResetScore()
        {
            currentMatches = 0;
            currentScore = 0;
            hudMgr.ShowScore(currentMatches);
        }

        public void ResetMoves()
        {
            currentMoves = 0;
            hudMgr.ShowMoves(currentMoves);
        }

        public void AddScore(int score)
        {
            Debug.Log(score);
            if (score > 0)
                currentMatches += score;

            currentScore += score;

            currentScore = Mathf.Max(0, currentScore);

            hudMgr.ShowScore(currentScore);
        }

        public void AddMove()
        {
            currentMoves++;
            hudMgr.ShowMoves(currentMoves);
        }
    }
}
