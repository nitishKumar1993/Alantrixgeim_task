using UnityEngine;
using UnityEngine.UI;

/// Fits a GridLayoutGroup to a holder RectTransform, computing both cellSize and spacing
/// so a rows×columns grid fills the area while preserving a given card aspect (W/H).
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGridFitter : MonoBehaviour
{
    [Header("Grid dimensions (set at runtime)")]
    public int columns = 4;
    public int rows = 3;

    [Header("Card shape")]
    [Min(0.01f)] public float cardAspect = 100f / 140f; // width / height (e.g. playing card 5:7)

    [Header("Spacing")]
    [Min(0f)] public float minSpacingX = 12f;
    [Min(0f)] public float minSpacingY = 12f;
    [Min(0f)] public float maxSpacingX = 40f; // cap how much we auto-expand spacing
    [Min(0f)] public float maxSpacingY = 40f;

    [Header("Inner padding of holder (pixels)")]
    public Vector2 innerPadding = new Vector2(24f, 24f); // left/right and top/bottom margins inside holder

    private RectTransform rt;
    private GridLayoutGroup grid;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    }

    /// Call this when layout changes (difficulty switch, rotation, resolution change)
    public void Apply(int cols, int rws)
    {
        columns = Mathf.Max(1, cols);
        rows = Mathf.Max(1, rws);

        grid.constraintCount = columns;
        Recalculate();
    }

    // Re-run when the RectTransform changes (orientation/resize)
    void OnRectTransformDimensionsChange()
    {
        if (!isActiveAndEnabled) return;
        if (columns <= 0 || rows <= 0) return;
        Recalculate();
    }

    void Recalculate()
    {
        // 1) Compute inner available size (minus padding)
        float innerW = Mathf.Max(0f, rt.rect.width - innerPadding.x * 2f);
        float innerH = Mathf.Max(0f, rt.rect.height - innerPadding.y * 2f);

        // 2) Start with minimum spacing to maximize card size
        float gapsX = (columns - 1) * (columns > 1 ? minSpacingX : 0f);
        float gapsY = (rows - 1) * (rows > 1 ? minSpacingY : 0f);

        // Max cell sizes ignoring aspect (with min spacing)
        float maxCellW = (innerW - gapsX) / columns;
        float maxCellH = (innerH - gapsY) / rows;

        if (maxCellW <= 0f || maxCellH <= 0f)
        {
            // Degenerate case: holder too small. Fall back to tiny cells.
            grid.cellSize = Vector2.zero;
            grid.spacing = new Vector2(minSpacingX, minSpacingY);
            grid.padding = new RectOffset(
                Mathf.RoundToInt(innerPadding.x),
                Mathf.RoundToInt(innerPadding.x),
                Mathf.RoundToInt(innerPadding.y),
                Mathf.RoundToInt(innerPadding.y));
            return;
        }

        // 3) Respect aspect: choose the limiting dimension
        // Width-limited option
        float wl_cellW = maxCellW;
        float wl_cellH = wl_cellW / cardAspect;

        // Height-limited option
        float hl_cellH = maxCellH;
        float hl_cellW = hl_cellH * cardAspect;

        Vector2 cell;
        if (wl_cellH <= maxCellH)
            cell = new Vector2(wl_cellW, wl_cellH);
        else
            cell = new Vector2(hl_cellW, hl_cellH);

        // 4) Compute used size with min spacing
        float usedW = columns * cell.x + (columns - 1) * (columns > 1 ? minSpacingX : 0f);
        float usedH = rows * cell.y + (rows - 1) * (rows > 1 ? minSpacingY : 0f);

        float leftoverW = Mathf.Max(0f, innerW - usedW);
        float leftoverH = Mathf.Max(0f, innerH - usedH);

        // 5) Distribute leftover into spacing up to maxSpacing; remainder to padding
        float spacingX = minSpacingX;
        float spacingY = minSpacingY;

        float addPerGapX = (columns > 1) ? leftoverW / (columns - 1) : 0f;
        float addPerGapY = (rows > 1) ? leftoverH / (rows - 1) : 0f;

        if (columns > 1)
        {
            spacingX = Mathf.Clamp(minSpacingX + addPerGapX, minSpacingX, maxSpacingX);
            // Recompute used and leftover after spacing clamp
            usedW = columns * cell.x + (columns - 1) * spacingX;
            leftoverW = Mathf.Max(0f, innerW - usedW);
        }

        if (rows > 1)
        {
            spacingY = Mathf.Clamp(minSpacingY + addPerGapY, minSpacingY, maxSpacingY);
            usedH = rows * cell.y + (rows - 1) * spacingY;
            leftoverH = Mathf.Max(0f, innerH - usedH);
        }

        // 6) Center the grid with padding (leftover halves on each side)
        int left = Mathf.RoundToInt(innerPadding.x + leftoverW * 0.5f);
        int right = Mathf.RoundToInt(innerPadding.x + leftoverW * 0.5f);
        int top = Mathf.RoundToInt(innerPadding.y + leftoverH * 0.5f);
        int bottom = Mathf.RoundToInt(innerPadding.y + leftoverH * 0.5f);

        // 7) Apply to GridLayoutGroup
        grid.cellSize = cell;
        grid.spacing = new Vector2(spacingX, spacingY);
        grid.padding = new RectOffset(left, right, top, bottom);
    }
}
