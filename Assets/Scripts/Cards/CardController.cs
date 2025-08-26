using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public string CardId { get; private set; }
    public CardView View { get; private set; }

    private GameManager game;

    // Start is called before the first frame update
    void Start()
    {
        View = GetComponent<CardView>();
    }

    /// <summary>
    /// Initialize the card with data
    /// </summary>
    /// <param name="gm"></param>
    /// <param name="id"></param>
    /// <param name="face"></param>
    /// <param name="back"></param>
    public void Init(GameManager gm, string id, Sprite face)
    {
        game = gm;
        CardId = id;
        View.SetFace(face);
        View.Flip(false, 0f);
    }


    void OnMouseUpAsButton()
    {
      
    }
}
