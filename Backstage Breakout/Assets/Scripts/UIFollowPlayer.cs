using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform target;        // The player transform
    public Vector3 offset;          // Offset above the player
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Convert world ? screen position
        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }
}
