using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GrassFrontSwapper : MonoBehaviour, IGridBound
{
    [Header("Scene refs (required)")]
    [SerializeField] private Tilemap grassBehind;    // ORDER 1: all bushes painted here at start
    [SerializeField] private Tilemap grassFront;     // ORDER 3: starts EMPTY
    private Grid playerGrid;                         // The Grid your map/mover use (parent of tilemaps)

    [Header("Player (required)")]
    [SerializeField] private GridMover2D playerMover; // We rely on OnStepFinished to avoid flicker
    [SerializeField] private Transform player;        // Will be auto-filled from mover if left empty

    [Header("Sampling")]
    [Tooltip("Small downward bias to avoid landing exactly on a row boundary.")]
    [SerializeField] private float yEpsilon = 0.0005f;

    // Cached
    private BoxCollider2D feet;                      // The feet BoxCollider2D on Player
    // Track the BEHIND tilemap cell currently fronted; 'invalid' means none.
    private static readonly Vector3Int Invalid = new Vector3Int(int.MaxValue, int.MaxValue, 0);
    private Vector3Int frontedBehindCell = Invalid;

    private void Awake()
    {
        if (!player && playerMover) player = playerMover.transform;

        if (player == null || playerMover == null)
        {
            Debug.LogError("GrassFrontSwapper: wire playerMover and player.");
            enabled = false;
            return;
        }

        feet = player.GetComponent<BoxCollider2D>();
        if (!feet)
            Debug.LogWarning("GrassFrontSwapper: Player has no BoxCollider2D. Feet sampling will fallback to transform.position (less precise).");

        // Subscribe to steps (single evaluation per tile)
        playerMover.OnStepFinished += HandleStepFinished;
    }

    private void OnDestroy()
    {
        if (playerMover != null) playerMover.OnStepFinished -= HandleStepFinished;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RebindSceneRefs();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindSceneRefs();
    }

    public void Rebind(Grid newGrid)
    {
        playerGrid = newGrid;
        RebindSceneRefs();
    }

    private void RebindSceneRefs()
    {
        if (playerMover)
            playerGrid = playerMover.CurrentGrid;

        if (!grassBehind || !grassBehind.gameObject.scene.IsValid())
            grassBehind = GameObject.Find("GrassBehind")?.GetComponent<Tilemap>();

        if (!grassFront || !grassFront.gameObject.scene.IsValid())
            grassFront = GameObject.Find("GrassFront")?.GetComponent<Tilemap>();

        if (playerGrid && grassBehind && grassFront)
            HandleStepFinished(GetFeetCellInMoverGrid());
    }

    // ---- Core logic ----

    private void HandleStepFinished(Vector3Int _)
    {
        if (playerGrid == null || grassBehind == null || grassFront == null) return;
        // We ignore the mover's cell and re-sample using FEET so “which bush” matches visuals 1:1
        Vector3Int feetCell = GetFeetCellInMoverGrid();
        EvaluateAndSwap(feetCell);
    }

    /// Returns the grid cell under the FEET using the feet collider center (world) -> WorldToCell.
    private Vector3Int GetFeetCellInMoverGrid()
    {
        Vector3 sampleWorld;

        if (playerGrid == null) return Vector3Int.zero;

        if (feet)
        {
            sampleWorld = player.position + (Vector3)feet.offset;
        }
        else
        {
            // Fallback if no feet collider
            sampleWorld = player.position;
        }

        // Tiny downward bias to prefer the tile the feet are visually inside, not the row above on boundaries.
        sampleWorld.y -= yEpsilon;

        return playerGrid.WorldToCell(sampleWorld);
    }

    private void EvaluateAndSwap(Vector3Int moverFeetCell)
    {
        if (playerGrid == null || grassBehind == null || grassFront == null) return;

        // 1) Convert mover feet cell -> world center (authoritative world point for that tile)
        Vector3 worldCenter = playerGrid.GetCellCenterWorld(moverFeetCell);

        // 2) Map to each grass tilemap's cell space
        Vector3Int behindCell = grassBehind.layoutGrid.WorldToCell(worldCenter);
        Vector3Int frontCell  = grassFront .layoutGrid.WorldToCell(worldCenter);

        bool tileIsBehindHere = grassBehind.HasTile(behindCell);
        bool tileIsFrontHere  = grassFront .HasTile(frontCell);

        // 3) If we previously fronted a different behind-cell, restore it (idempotent cleanup)
        if (frontedBehindCell != Invalid && frontedBehindCell != behindCell)
        {
            RestoreFrontedToBehind(frontedBehindCell);
            frontedBehindCell = Invalid;
        }

        // 4) Desired state at current feet cell:
        //    - If a bush exists in BEHIND at this cell → it should be FRONT here.
        //    - If no bush in BEHIND → ensure nothing is fronted at this spot.
        if (tileIsBehindHere)
        {
            if (!tileIsFrontHere)
            {
                MoveBehindToFront(behindCell, frontCell);
            }
            // Remember which BEHIND cell we fronted (even if it was already fronted)
            frontedBehindCell = behindCell;
        }
        else
        {
            if (tileIsFrontHere)
            {
                // We're not on a behind-bush here; ensure there's no stray front tile at this spot.
                RestoreFrontedToBehind(behindCell);
            }

            // If the spot we think we fronted equals current, clear it
            if (frontedBehindCell == behindCell)
                frontedBehindCell = Invalid;
        }

#if UNITY_EDITOR
        DrawCellGizmo(worldCenter, Color.cyan);
#endif
    }

    private void MoveBehindToFront(Vector3Int behindCell, Vector3Int frontCell)
    {
        TileBase t = grassBehind.GetTile(behindCell);
        if (!t) return;

        grassFront.SetTile(frontCell, t);
        CopyPerTileData(grassBehind, grassFront, behindCell, frontCell);
        grassBehind.SetTile(behindCell, null);
    }

    private void RestoreFrontedToBehind(Vector3Int behindCell)
    {
        // Compute matching front cell via world mapping (keeps us robust)
        Vector3 worldCenter = grassBehind.layoutGrid.GetCellCenterWorld(behindCell);
        Vector3Int frontCell = grassFront.layoutGrid.WorldToCell(worldCenter);

        TileBase t = grassFront.GetTile(frontCell);
        if (!t) return;

        grassBehind.SetTile(behindCell, t);
        CopyPerTileData(grassFront, grassBehind, frontCell, behindCell);
        grassFront.SetTile(frontCell, null);
    }

    private void CopyPerTileData(Tilemap from, Tilemap to, Vector3Int fromCell, Vector3Int toCell)
    {
        to.SetColor(toCell, from.GetColor(fromCell));
        to.SetTransformMatrix(toCell, from.GetTransformMatrix(fromCell));
        // If you use non-default TileFlags per-cell, mirror them here as needed.
    }

#if UNITY_EDITOR
    private void DrawCellGizmo(Vector3 worldCenter, Color c)
    {
        float s = 0.5f;
        Debug.DrawLine(worldCenter + new Vector3(-s, -s), worldCenter + new Vector3( s, -s), c, 0f, false);
        Debug.DrawLine(worldCenter + new Vector3( s, -s),  worldCenter + new Vector3( s,  s), c, 0f, false);
        Debug.DrawLine(worldCenter + new Vector3( s,  s),  worldCenter + new Vector3(-s,  s), c, 0f, false);
        Debug.DrawLine(worldCenter + new Vector3(-s,  s),  worldCenter + new Vector3(-s, -s), c, 0f, false);
    }
#endif
}
