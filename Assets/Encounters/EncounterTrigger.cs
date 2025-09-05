using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Checks for tall grass steps and triggers encounters using the active EncounterTable.
/// Attach to the persistent player so encounters work across scenes.
/// </summary>
[RequireComponent(typeof(GridMover2D))]
public class EncounterTrigger : MonoBehaviour, IGridBound
{
    [SerializeField] private GridMover2D mover;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private string grassTilemapName = "GrassBehind";

    private EncounterTable table;
    private Grid currentGrid;

    private void Awake()
    {
        if (!mover) mover = GetComponent<GridMover2D>();
        mover.OnStepFinished += HandleStep;
    }

    private void OnDestroy()
    {
        if (mover != null) mover.OnStepFinished -= HandleStep;
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

    private void RebindSceneRefs()
    {
        table = FindFirstObjectByType<EncounterTable>();
        if (!grassTilemap || !grassTilemap.gameObject.scene.IsValid())
            grassTilemap = GameObject.Find(grassTilemapName)?.GetComponent<Tilemap>();

        currentGrid = mover ? mover.CurrentGrid : null;
    }

    public void Rebind(Grid newGrid)
    {
        currentGrid = newGrid;
    }

    private void HandleStep(Vector3Int cell)
    {
        if (table == null || grassTilemap == null || currentGrid == null)
            return;

        Vector3 worldCenter = currentGrid.GetCellCenterWorld(cell);
        Vector3Int grassCell = grassTilemap.layoutGrid.WorldToCell(worldCenter);
        if (grassTilemap.HasTile(grassCell))
        {
            table.TryEncounter();
        }
    }
}
