// Scripts/Rendering/CameraFollow2D.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smooth = 20f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RebindTarget();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindTarget();
    }

    public void RebindTarget()
    {
        target = FindObjectOfType<PlayerController>()?.transform;
    }

    void LateUpdate()
    {
        if (!target) return;
        var p = transform.position;
        p.x = Mathf.Lerp(p.x, target.position.x, smooth * Time.deltaTime);
        p.y = Mathf.Lerp(p.y, target.position.y, smooth * Time.deltaTime);
        transform.position = p;
    }
}
