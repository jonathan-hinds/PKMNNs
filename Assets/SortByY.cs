// Scripts/Rendering/SortByY.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortByY : MonoBehaviour
{
    [SerializeField] int baseOrder = 0;
    [SerializeField] int step = 100;
    SpriteRenderer sr;
    void Awake(){ sr = GetComponent<SpriteRenderer>(); }
    void LateUpdate(){ sr.sortingOrder = baseOrder - Mathf.RoundToInt(transform.position.y * step); }
}
