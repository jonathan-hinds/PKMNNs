// Scripts/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(GridMover2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] KeyCode interactKey = KeyCode.Z;
    GridMover2D mover;
    public Vector2Int Facing => mover.Facing;

    void Awake() { mover = GetComponent<GridMover2D>(); }

    void Update()
    {
        if (Input.GetKeyDown(interactKey)) TryInteract();
    }

    void TryInteract()
    {
        Vector3 target = transform.position + new Vector3(Facing.x, Facing.y);
        var hits = Physics2D.OverlapBoxAll(target, new Vector2(0.6f, 0.6f), 0f);
        foreach (var h in hits)
        {
            var i = h.GetComponent<IInteractable>();
            if (i != null) { i.Interact(); break; }
        }
    }
}

public interface IInteractable { void Interact(); }
