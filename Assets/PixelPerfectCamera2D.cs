// Scripts/Rendering/PixelPerfectCamera2D.cs
using UnityEngine;

[ExecuteAlways]
public class PixelPerfectCamera2D : MonoBehaviour
{
    [SerializeField] private int referencePPU = 16;
    Camera cam;
    void OnEnable() { cam = GetComponent<Camera>(); }
    void LateUpdate()
    {
        if (cam == null || !cam.orthographic) return;
        float unitsPerPixel = 1f / referencePPU;
        var p = cam.transform.position;
        p.x = Mathf.Round(p.x / unitsPerPixel) * unitsPerPixel;
        p.y = Mathf.Round(p.y / unitsPerPixel) * unitsPerPixel;
        cam.transform.position = p;
    }
}
