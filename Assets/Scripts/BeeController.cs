using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Inscribed")]
    public Camera mainCamera; // Reference to the main Camera
    public float followSpeed = 10f; // Adjust this value for more/less delay

    [Header("Dynamic")]
    private Vector3 lastValidPosition;

    private Rigidbody2D rb;

    void Start()
    {
        lastValidPosition = transform.position;

        // Get or add the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.isKinematic = false; // Ensure Rigidbody2D physics handles movement
        rb.gravityScale = 0; // No gravity effect on bee
    }

    void Update()
    {
        // Get the mouse position
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = 2 - Camera.main.transform.position.z; // Match bee's Z depth
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Lock the Z position at 2
        targetPos.z = 2;
        targetPos.y = targetPos.y - 0.5f;

        // Smoothly move the bee towards the target position
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // Update Rigidbody2D's position for collision checks
        rb.MovePosition(newPos);  // Use MovePosition to interact with physics system
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Wall"))
        {
            // Stop bee at the point of collision
            rb.velocity = Vector2.zero; // Reset velocity
            transform.position = lastValidPosition; // Revert to the last valid position
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Wall"))
        {
            // Keep the bee from moving through walls
            rb.MovePosition(lastValidPosition);  // Avoid passing through
        }
    }

    void FixedUpdate()
    {
        // Save the last valid position for collision fallback
        lastValidPosition = rb.position;
    }
}
