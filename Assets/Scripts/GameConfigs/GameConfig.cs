using UnityEngine;

[CreateAssetMenu(menuName = "MatchingGame/Game Config", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Board")]
    public int columns = 4;
    public int rows = 3;

    [Header("Cards data")]
    public CardData[] allCardsData;

    [Header("SFX")]
    public AudioClip flipSfx, matchSfx, mismatchSfx, winSfx, looseSfx;
}