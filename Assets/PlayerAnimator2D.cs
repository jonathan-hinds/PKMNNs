// Scripts/Animation/PlayerAnimator2D.cs
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator2D : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis   = "Vertical";

    // default face Down like classic Pokémon
    private Vector2 lastFace = Vector2.down;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    void Update()
    {
        float rawH = Input.GetAxisRaw(horizontalAxis);
        float rawV = Input.GetAxisRaw(verticalAxis);

        // cardinal snap (no diagonals)
        float mx = 0f, my = 0f;
        if (Mathf.Abs(rawH) > Mathf.Abs(rawV)) mx = Mathf.Sign(rawH);
        else if (Mathf.Abs(rawV) > 0f)         my = Mathf.Sign(rawV);

        bool isMoving = (mx != 0f || my != 0f);

        // remember last non-zero facing
        if (isMoving)
            lastFace = new Vector2(mx, my);

        // feed the blend trees with the *current face*:
        //   - when moving: current input
        //   - when idle:   lastFace
        Vector2 face = isMoving ? new Vector2(mx, my) : lastFace;

        animator.SetFloat("moveX", face.x);
        animator.SetFloat("moveY", face.y);
        animator.SetBool ("isMoving", isMoving);
    }
}
