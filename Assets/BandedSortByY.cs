using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class BandedSortByY : MonoBehaviour
{
    [Header("Baseline = grass order")]
    [SerializeField] private int baseOrder = 1;

    [Header("Clamp band (keep between ground 0 and under foreground 3)")]
    [SerializeField] private int safeMinOrder = 0;
    [SerializeField] private int safeMaxOrder = 2;

    [Header("Strength (try 30–60)")]
    [SerializeField] private int unitsPerStep = 40;

    [Header("Feet bias (if pivot is Center, use ~-0.10)")]
    [SerializeField] private float yVisualOffset = 0f;

    [SerializeField] private Camera cam; // assign Main Camera (optional auto-find)

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (!cam) cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        if (!cam) cam = Camera.main;

        sr.sortingLayerName = "Default";

        // Camera-relative Y keeps values small around 0 near mid-screen
        float relY = (transform.position.y - cam.transform.position.y) + yVisualOffset;

        int delta = -Mathf.RoundToInt(relY * unitsPerStep);
        int order = Mathf.Clamp(baseOrder + delta, safeMinOrder, safeMaxOrder);
        sr.sortingOrder = order;
    }
}
