// Scripts/Rendering/CameraFollow2D.cs
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smooth = 20f;
    void LateUpdate()
    {
        if (!target) return;
        var p = transform.position;
        p.x = Mathf.Lerp(p.x, target.position.x, smooth * Time.deltaTime);
        p.y = Mathf.Lerp(p.y, target.position.y, smooth * Time.deltaTime);
        transform.position = p;
    }
}
