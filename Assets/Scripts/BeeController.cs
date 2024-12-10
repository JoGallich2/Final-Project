using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeeController : MonoBehaviour
{
    [Header("Inscribed")]
    public Camera mainCamera;  // Reference to the main Camera
    public Animator beeAnimator;  // Animator to control the bee's animations
    private AudioController audioController;

    [Header("Dynamic")]
    public Vector3 lastValidPosition;  // Position to revert to when colliding with walls
    public Vector3 initialPosition;  // The bee's starting position
    public int currentLives;  // Current number of lives

    [Header("Game Settings")]
    public int totalLives = 3;  // Total starting lives
    public float followSpeed = 10f;  // Speed at which the bee follows the mouse
    public int points = 10;  // Points to add when the bee pops a balloon
    private GameManager gameManager;  // Reference to the GameManager for points
    private BalloonSpawner balloonSpawner;  // Reference to the BalloonSpawner for stopping balloon spawns

    private Rigidbody2D rb;  // Rigidbody for movement handling
    private bool isDead = false;  // Flag to track if the bee is dead
    private bool isFirstSpawn = true;  // Flag to track if this is the first spawn
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
        audioController = FindObjectOfType<AudioController>();

        // Get the GameManager reference (ensure a GameManager exists in the scene)
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene. Please ensure there is one.");
        }
        balloonSpawner = FindObjectOfType<BalloonSpawner>();
        if (balloonSpawner == null)
        {
            Debug.LogError("BalloonSpawner not found in the scene. Please ensure there is one.");
        }
        // Trigger the respawn logic on the first spawn
        if (isFirstSpawn)
        {
            StartCoroutine(RespawnDelay());
            isFirstSpawn = false;  // Mark the first spawn as complete
        }

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
        if (isDead)
        {
            return;
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;  // Stop movement when hitting a wall
            transform.position = lastValidPosition;  // Revert to last valid position
        }
        else if (collision.collider.CompareTag("RedZone"))
        {
            Debug.Log("Bee entered the Red Zone! Losing 1 life.");
            DecreaseLivesAndRespawn();  // Handle collision with the RedZone
        }
        else if (collision.collider.CompareTag("Bomb"))
        {
            Debug.Log("Bee Hit a Bomb! Losing 1 life.");
            DecreaseLivesAndRespawn();  // Handle collision with the Bomb

            // Destroy the Bomb
            Destroy(collision.gameObject);
        }
        else if (collision.collider.CompareTag("Balloon"))
        {

            // Add points to the GameManager when the balloon is popped
            if (gameManager != null)
            {
                gameManager.AddPoints(points);  // Add points to the game
                balloonSpawner.StopBalloonSpawning();
            }

            //play sound effect
            AudioClip balloonPopClip = collision.collider.GetComponent<Balloon>().popSound;

            if (balloonPopClip != null)
            {
                audioController.PopSound(balloonPopClip);
            }
            else
            {
                Debug.LogWarning("No AudioClip found on the balloon!");
            }

            // Destroy the balloon
            Destroy(collision.gameObject);
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
        gameManager.DecreaseLives();
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
        if (!isFirstSpawn) {
            beeAnimator.SetTrigger("Die");  // Only trigger animation for subsequent spawns
            yield return new WaitForSeconds(1f);   // Wait for death animation duration
        };

        transform.position = initialPosition;  // Respawn the bee
        rb.velocity = Vector2.zero;  // Reset velocity after respawn

        PauseGameWithTextBox();  // Pause the game and display the text box
        isDead = false;  // Allow the bee to move again
    }

    private void PauseGameWithTextBox()
    {
        gameManager.UIController.BeeDiedPause();
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
