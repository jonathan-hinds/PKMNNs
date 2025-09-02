// Scripts/Rendering/RowAwareSort.cs
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class RowAwareSort : MonoBehaviour
{
    [Header("Scene refs")]
    [SerializeField] private Grid grid;                // Drag your Grid (parent of tilemaps)
    [SerializeField] private Tilemap grassTilemap;     // Drag Tilemap_Grass here

    [Header("Orders (match your setup)")]
    [SerializeField] private int groundOrder = 0;      // Tilemap_Ground
    [SerializeField] private int grassOrder  = 1;      // Tilemap_Grass
    [SerializeField] private int foregroundOrder = 3;  // Tilemap_Foreground (just for reference)
    [SerializeField] private int playerBehindGrassOrder = 0; // behind grass
    [SerializeField] private int playerFrontOfGrassOrder = 2; // in front of grass

    [Header("Alignment with your mover (optional)")]
    [Tooltip("If your mover uses a visual cellCenterOffset, mirror it here so the sampled cell matches.")]
    [SerializeField] private Vector2 cellCenterOffset = Vector2.zero;

    [Header("Row test mode")]
    [Tooltip("If true, only hide player when standing ON a grass tile. If false, hide when any grass exists on the same Y row within +/- xLeeway tiles.")]
    [SerializeField] private bool requireStandingOnGrass = true;
    [SerializeField, Min(0)] private int xLeeway = 0;   // used when requireStandingOnGrass == false

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (!grid) grid = GetComponentInParent<Grid>();
        if (!sr) sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (!grid || !grassTilemap) return;

        // Sample the grid cell the player is visually occupying.
        Vector3 samplePos = transform.position - (Vector3)cellCenterOffset;
        Vector3Int playerCell = grid.WorldToCell(samplePos);

        bool sameRowAsGrass = false;

        if (requireStandingOnGrass)
        {
            // Strict: only when player is ON a grass tile.
            sameRowAsGrass = grassTilemap.HasTile(playerCell);
        }
        else
        {
            // Row-based: if any grass tile exists on the same Y row near the player.
            int y = playerCell.y;
            for (int dx = -xLeeway; dx <= xLeeway; dx++)
            {
                if (grassTilemap.HasTile(new Vector3Int(playerCell.x + dx, y, 0)))
                {
                    sameRowAsGrass = true;
                    break;
                }
            }
        }

        // Apply the rule:
        // - same row → player behind grass (order <= grassOrder)
        // - otherwise → player in front of grass (order > grassOrder but < foreground)
        sr.sortingLayerName = "Default";
        sr.sortingOrder = sameRowAsGrass ? playerBehindGrassOrder : playerFrontOfGrassOrder;
    }
}
