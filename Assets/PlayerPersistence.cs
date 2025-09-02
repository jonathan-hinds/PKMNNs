using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Persist the player across scene loads, rebind GridMover2D to the new scene's Grid,
/// and (optionally) move to a scheduled spawn cell set by DoorwayDetector.
/// </summary>
[RequireComponent(typeof(GridMover2D))]
public sealed class PlayerPersistence : MonoBehaviour
{
    private static PlayerPersistence _instance;

    [SerializeField] private GridMover2D mover;

    private static bool hasPendingSpawn;
    private static Vector3Int pendingSpawnCell;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        _instance = this;
        if (!mover) mover = GetComponent<GridMover2D>();

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RebindGridAndMaybeSpawn(); // bind for the initial scene too
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindGridAndMaybeSpawn();
    }

    private void RebindGridAndMaybeSpawn()
    {
        var grid = Object.FindObjectOfType<Grid>();
        if (mover == null || grid == null) return;

        mover.SetGrid(grid);

        if (hasPendingSpawn)
        {
            Vector3 world = grid.GetCellCenterWorld(pendingSpawnCell);
            transform.position = new Vector3(world.x, world.y, transform.position.z);
            hasPendingSpawn = false;
        }
    }

    /// <summary>Called before LoadScene to place the player at a cell in the next scene.</summary>
    public static void ScheduleSpawn(Vector3Int cell)
    {
        pendingSpawnCell = cell;
        hasPendingSpawn = true;
    }
}
