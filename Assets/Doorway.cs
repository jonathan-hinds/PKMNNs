using UnityEngine;

/// <summary>
/// Marks a tile as a doorway that leads to another scene.
/// Attach to a trigger collider occupying one grid cell.
/// </summary>
public class Doorway : MonoBehaviour
{
    [Tooltip("Name of the scene to load when the player steps on this tile.")]
    public string sceneName;

    [Tooltip("If true, the player will be placed at this cell after the scene loads.")]
    public bool useSpawnCell = false;

    [Tooltip("Grid cell where the player should appear in the destination scene.")]
    public Vector3Int spawnCell = Vector3Int.zero;
}