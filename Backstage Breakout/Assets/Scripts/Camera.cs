using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // assign in inspector
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, transform.position.z);
    }
}
