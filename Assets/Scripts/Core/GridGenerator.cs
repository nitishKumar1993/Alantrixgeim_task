using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;


public class GridGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public RectTransform gridRoot;
    public GameObject cardPrefab;

    public GameManager _gm;
    public GameConfig cfg;

    public int columns, rows;
    public float spacing;

    private void Start()
    {
        SpawnGrid(_gm, cfg, columns, rows);
    }

    public List<CardController> SpawnGrid(GameManager _gm, GameConfig _cfg, int _columns, int _rows)
    {
        var list = new List<CardController>();
        int total = _columns * _rows;
        int pairs = total / 2;
        var deck = DeckBuilder.Build(cfg, pairs);

        for (int r = 0, k = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++, k++)
            {
                if (k < deck.Count)
                {
                    var go = Instantiate(cardPrefab, gridRoot);
                    var ctrl = go.GetComponent<CardController>();

                    var (id, face) = deck[k];
                    ctrl.Init(_gm, id, face);
                    list.Add(ctrl);
                }
            }
        }

        gridLayout.constraintCount = columns;

        var fitter = this.GetComponent<ResponsiveGridFitter>();
        fitter.Apply(columns, rows);

        return list;
    }

    float CardSpacing
    {
        get { return spacing; }
    }
}