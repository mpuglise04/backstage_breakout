using UnityEngine;

public class CameraFollowProxy : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float fixedY;
    private float fixedZ;

    private void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            fixedY,
            fixedZ
        );
    }
}
