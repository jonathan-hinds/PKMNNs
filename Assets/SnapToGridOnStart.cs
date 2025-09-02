// Scripts/Movement/SnapToGridOnStart.cs
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class SnapToGridOnStart : MonoBehaviour
{
    [SerializeField] private Grid grid; // Drag your Grid here (parent of tilemaps)

    void Awake()
    {
        if (!grid) grid = GetComponentInParent<Grid>();
        if (!grid)
        {
            // Fallback: integer rounding if no Grid is found
            var p = transform.position;
            transform.position = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), p.z);
            return;
        }

        Vector3Int cell = grid.WorldToCell(transform.position);
        Vector3 center = grid.GetCellCenterWorld(cell);
        transform.position = new Vector3(center.x, center.y, transform.position.z);
    }
}
