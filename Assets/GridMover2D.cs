using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GridMover2D : MonoBehaviour
{
    public const float TILE_SIZE = 1f;

    [Header("Speed")]
    [SerializeField] private float tilesPerSecond = 6f;

    [Header("Collision")]
    [SerializeField] private LayerMask obstacleMask;

    [Header("Grid Reference")]
    [SerializeField] private Grid grid;   // Drag your Grid here (parent of your Tilemaps)

    [Header("Alignment Tuning (edit at runtime)")]
    [Tooltip("Nudges the character off the exact cell center. Adjust in Play mode until it looks centered.")]
    [SerializeField] private Vector2 cellCenterOffset = Vector2.zero;

    [Tooltip("Convenience overrides for the 'feet' hitbox so you can tweak feel at runtime.")]
    [SerializeField] private Vector2 feetSize = new Vector2(0.8f, 0.6f);
    [SerializeField] private Vector2 feetOffset = new Vector2(0f, -0.2f);

    public bool IsMoving { get; private set; }
    public Vector2Int Facing { get; private set; } = Vector2Int.down;

    public System.Action<Vector3Int> OnStepFinished;

    private BoxCollider2D hitbox;

    void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
        if (!grid) grid = GetComponentInParent<Grid>();
        ApplyFeetToCollider();
        SnapToCellCenterImmediate();
    }

    void OnValidate()
    {
        if (!hitbox) hitbox = GetComponent<BoxCollider2D>();
        ApplyFeetToCollider();
    }

    private void ApplyFeetToCollider()
    {
        if (!hitbox) return;
        hitbox.size = feetSize;
        hitbox.offset = feetOffset;
    }

    private void SnapToCellCenterImmediate()
    {
        if (!grid) return;
        Vector3Int cell = grid.WorldToCell(transform.position);
        Vector3 center = grid.GetCellCenterWorld(cell);
        transform.position = new Vector3(center.x + cellCenterOffset.x,
                                         center.y + cellCenterOffset.y,
                                         transform.position.z);
    }

    void Update()
    {
        // Read desired axis (cardinal only, like Pokémon)
        Vector2Int axis = GetHeldAxis();
        if (axis != Vector2Int.zero) Facing = axis;

        // If holding input and not currently moving, step immediately.
        if (!IsMoving && axis != Vector2Int.zero)
        {
            TryStep(axis);
        }
    }

    private Vector2Int GetHeldAxis()
    {
        int h = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int v = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (Mathf.Abs(h) > Mathf.Abs(v)) return new Vector2Int((int)Mathf.Sign(h), 0);
        if (Mathf.Abs(v) > 0) return new Vector2Int(0, (int)Mathf.Sign(v));
        return Vector2Int.zero;
    }

    public bool TryStep(Vector2Int dir)
    {
        if (IsMoving || dir == Vector2Int.zero || grid == null) return false;

        // IMPORTANT: sample the logical cell by removing the visual offset
        Vector3Int curCell = grid.WorldToCell(transform.position - (Vector3)cellCenterOffset);
        Vector3Int nextCell = curCell + new Vector3Int(dir.x, dir.y, 0);

        Vector3 start = grid.GetCellCenterWorld(curCell) + (Vector3)cellCenterOffset;
        Vector3 target = grid.GetCellCenterWorld(nextCell) + (Vector3)cellCenterOffset;

        if (!IsWalkable(target)) return false;

        StartCoroutine(MoveTo(nextCell, start, target));
        return true;
    }

    private bool IsWalkable(Vector3 targetWorldPos)
    {
        Vector3 checkCenter = targetWorldPos + (Vector3)hitbox.offset;
        Collider2D hit = Physics2D.OverlapBox(checkCenter, hitbox.size, 0f, obstacleMask);
        return hit == null;
    }

    private IEnumerator MoveTo(Vector3Int nextCell, Vector3 start, Vector3 target)
    {
        IsMoving = true;
        float speed = tilesPerSecond * TILE_SIZE;
        float dist = Vector3.Distance(start, target);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * (speed / dist);
            transform.position = Vector3.Lerp(start, target, Mathf.Clamp01(t));
            yield return null;
        }

        // Snap to exact center of the next cell (+ visual offset)
        Vector3 snapped = grid.GetCellCenterWorld(nextCell) + (Vector3)cellCenterOffset;
        transform.position = new Vector3(snapped.x, snapped.y, transform.position.z);

        IsMoving = false;
        OnStepFinished?.Invoke(nextCell);

        // 🔁 CHAIN: if input is still held, launch the next step immediately.
        Vector2Int axis = GetHeldAxis();
        if (axis != Vector2Int.zero)
        {
            // If a diagonal or turn is held, we still pick the dominant axis.
            TryStep(axis);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!hitbox) hitbox = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)hitbox.offset, hitbox.size);

        if (grid)
        {
            Vector3Int curCell = grid.WorldToCell(transform.position - (Vector3)cellCenterOffset);
            Vector3 center = grid.GetCellCenterWorld(curCell) + (Vector3)cellCenterOffset;
            Gizmos.color = new Color(0f, 1f, 1f, 0.6f);
            Gizmos.DrawWireCube(center, new Vector3(0.2f, 0.2f, 0f));
        }
    }
#endif
}
