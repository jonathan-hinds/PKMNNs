using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// On step finish, if the player is on a Doorway, schedule spawn and
/// defer the scene load by one frame to avoid destroying the Grid
/// while other listeners are still handling the event.
/// </summary>
[RequireComponent(typeof(GridMover2D))]
public class DoorwayDetector : MonoBehaviour
{
    [SerializeField] private GridMover2D mover;
    [SerializeField] private Transform player;

    private bool isLoading;
    private string pendingSceneName;

    private void Awake()
    {
        if (!mover) mover = GetComponent<GridMover2D>();
        if (!player && mover) player = mover.transform;
    }

    private void OnEnable()
    {
        if (mover != null) mover.OnStepFinished += HandleStepFinished;
    }

    private void OnDisable()
    {
        if (mover != null) mover.OnStepFinished -= HandleStepFinished;
    }

    private void HandleStepFinished(Vector3Int _)
    {
        if (isLoading) return;

        var hits = Physics2D.OverlapBoxAll(player.position, new Vector2(0.6f, 0.6f), 0f);
        foreach (var h in hits)
        {
            var door = h.GetComponent<Doorway>();
            if (door != null && !string.IsNullOrEmpty(door.sceneName))
            {
                if (door.useSpawnCell)
                    PlayerPersistence.ScheduleSpawn(door.spawnCell);

                pendingSceneName = door.sceneName;
                StartCoroutine(LoadSceneDeferred()); // defer by one frame
                break;
            }
        }
    }

    private IEnumerator LoadSceneDeferred()
    {
        isLoading = true;
        yield return null; // let all OnStepFinished subscribers complete using the old Grid
        SceneManager.LoadScene(pendingSceneName, LoadSceneMode.Single);
        isLoading = false;
        pendingSceneName = null;
    }
}
