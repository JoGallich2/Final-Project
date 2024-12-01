using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Inscribed")]
    public Camera mainCamera;  // Reference to the main Camera
    public Animator beeAnimator;  // Animator to control the bee's animations

    [Header("Dynamic")]
    public Vector3 lastValidPosition;  // Position to revert to when colliding with walls
    public Vector3 initialPosition;  // The bee's starting position
    public int currentLives;  // Current number of lives

    [Header("Game Settings")]
    public int totalLives = 3;  // Total starting lives
    public float followSpeed = 10f;  // Speed at which the bee follows the mouse

    private Rigidbody2D rb;  // Rigidbody for movement handling
    private bool isDead = false;  // Flag to track if the bee is dead

    void Start()
    {
        // Get necessary components and initialize variables
        beeAnimator = GetComponent<Animator>();
        lastValidPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 0;
        initialPosition = transform.position;
        currentLives = totalLives;
    }

    void Update()
    {
        if (isDead) return;  // Prevent movement if the bee is dead

        // Get the mouse position and convert it to world space
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = 2 - Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos2D);
        targetPos.z = 2;  // Keep the bee at the same depth
        targetPos.y -= 0.5f;  // Adjust the bee's Y position

        // Move the bee smoothly toward the mouse position
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        rb.MovePosition(newPos);

        // Set the "IsMoving" parameter for animations
        beeAnimator.SetBool("IsMoving", newPos != transform.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;  // Stop movement when hitting a wall
            transform.position = lastValidPosition;  // Revert to last valid position
        }
        else if (collision.collider.CompareTag("RedZone"))
        {
            DecreaseLivesAndRespawn();  // Handle collision with the RedZone
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            rb.MovePosition(lastValidPosition);  // Prevent passing through walls
        }
    }

    void FixedUpdate()
    {
        lastValidPosition = rb.position;  // Update last valid position for collision handling
    }

    public void DecreaseLivesAndRespawn()
    {
        currentLives--;
        if (currentLives <= 0)
        {
            beeAnimator.SetBool("IsMoving", false);  // Stop movement animation
            beeAnimator.SetTrigger("Die");  // Trigger death animation
            StartCoroutine(DieAndGameOver());  // Handle death and game over
        }
        else
        {
            StartCoroutine(RespawnDelay());  // Handle respawn after losing a life
        }
    }

    private IEnumerator DieAndGameOver()
    {
        isDead = true;  // Set the bee as dead
        beeAnimator.SetTrigger("Die");  // Trigger the "Die" animation
        yield return new WaitForSeconds(1f);  // Wait for the death animation to finish
        GameOver();  // Handle game over logic
    }

    private IEnumerator RespawnDelay()
    {
        isDead = true;  // Stop movement while dead
        beeAnimator.SetTrigger("Die");  // Trigger death animation
        yield return new WaitForSeconds(1f);  // Wait for death animation duration
        transform.position = initialPosition;  // Respawn the bee
        rb.velocity = Vector2.zero;  // Reset velocity after respawn
        isDead = false;  // Allow movement again
    }

    private void GameOver()
    {
        Debug.Log("Game Over! You lost all your lives.");
    }

    public void ResetBeePosition()
    {
        transform.position = initialPosition;  // Reset the bee's position
        rb.velocity = Vector2.zero;  // Reset velocity
    }
}
