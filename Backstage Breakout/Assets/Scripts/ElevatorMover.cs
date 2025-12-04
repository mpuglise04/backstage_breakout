using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ElevatorMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 3f;   // How far up/down it travels
    [SerializeField] private float speed = 2f;          // Movement speed
    [SerializeField] private bool startsGoingUp = true; // Optional direction toggle

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingUp;

    private void Start()
    {
        startPos = transform.position;
        movingUp = startsGoingUp;

        targetPos = startPos + Vector3.up * moveDistance;
    }

    private void Update()
    {
        // Pick direction
        Vector3 destination = movingUp ? targetPos : startPos;

        // Move toward the destination
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // When we reach the top or bottom, flip direction
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            movingUp = !movingUp;
        }
    }
}
