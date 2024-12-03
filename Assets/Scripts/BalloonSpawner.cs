using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    [Header("Balloon Settings")]
    public GameObject balloonPrefab;  // The balloon prefab
    public Color[] balloonColors;  // Array of possible colors for balloons
    public Vector2 sizeRange = new Vector2(0.5f, 2f);  // Min and max scale for balloons

    [Header("Spawning Settings")]
    public float spawnInterval = 2f;  // Time interval between spawns
    public PolygonCollider2D spawnAreaCollider;  // Collider defining the spawn area
    public PolygonCollider2D redZoneCollider;  // Collider defining the red zone

    [Header("Balloon Spawn Limits")]
    public int maxBalloonCount = 10;  // Maximum number of balloons to spawn, set to 0 for unlimited
    public bool AllowSpawning = true;

    private int currentBalloonCount = 0;  // Current number of balloons spawned

    private void Start()
    {
        StartCoroutine(SpawnBalloons());
        AllowSpawning = true;
}

    private IEnumerator SpawnBalloons()
    {
        while (maxBalloonCount == 0 || currentBalloonCount < maxBalloonCount && AllowSpawning == true)  // Check if the balloon count is within the limit
        {
            Vector2 randomPosition = GenerateValidPosition();

            // If a valid position is found, spawn a balloon
            if (randomPosition != Vector2.zero) // Ensure the position is valid
            {
                SpawnBalloon(randomPosition);
                currentBalloonCount++;  // Increment the spawned balloon count
            }

            yield return new WaitForSeconds(spawnInterval);  // Wait before spawning the next balloon
        }
    }

    private void SpawnBalloon(Vector2 position)
    {
        // Instantiate a new balloon
        GameObject newBalloon = Instantiate(balloonPrefab, position, Quaternion.identity);

        // Set random size
        float randomSize = Random.Range(sizeRange.x, sizeRange.y);

        // Get the current scale of the balloon
        Vector3 currentScale = newBalloon.transform.localScale;

        // Multiply the current scale by the random size multiplier
        newBalloon.transform.localScale = new Vector3(
            currentScale.x * randomSize,  // Apply random size multiplier to X axis
            currentScale.y * randomSize,  // Apply random size multiplier to Y axis
            currentScale.z * randomSize   // Apply random size multiplier to Z axis (if it's a 3D balloon)
        );

        // Set random color
        MeshRenderer meshRenderer = newBalloon.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = balloonColors[Random.Range(0, balloonColors.Length)];
        }
    }

    private Vector2 GenerateValidPosition()
    {
        Vector2 position;
        int maxRetries = 20;  // Fewer retries for closer positioning
        int attempts = 0;

        do
        {
            // Generate a random position inside the bounds of the spawn area collider
            Bounds bounds = spawnAreaCollider.bounds;
            position = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            attempts++;

            // If max retries are reached, break out of the loop to avoid infinite looping
            if (attempts >= maxRetries)
            {
                Debug.LogWarning("Max retries reached. Balloons may be spawning far apart.");
                return Vector2.zero;  // Return an invalid position if max retries are reached
            }
        }
        while (!IsInsideSpawnArea(position) || IsInsideRedZone(position) || IsPositionOccupied(position));

        return position;
    }


    private bool IsInsideSpawnArea(Vector2 position)
    {
        return spawnAreaCollider.OverlapPoint(position);
    }

    private bool IsInsideRedZone(Vector2 position)
    {
        return redZoneCollider != null && redZoneCollider.OverlapPoint(position);
    }

    // Check if the position is already occupied by another balloon
    private bool IsPositionOccupied(Vector2 position)
    {
        // Get the radius of the CapsuleCollider2D (using the width as the radius for simplicity)
        CapsuleCollider2D balloonCollider = balloonPrefab.GetComponent<CapsuleCollider2D>();
        float radius = balloonCollider != null ? balloonCollider.size.x / 2 : Mathf.Max(sizeRange.x, sizeRange.y);  // Use capsule width / 2 as radius

        // Check for any colliders within the radius around the position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        foreach (var collider in colliders)
        {
            // If the collider is not the current balloon prefab, return true (position is occupied)
            if (collider.gameObject.CompareTag("Balloon"))
            {
                return true;
            }
        }

        return false;  // Position is free
    }

    public void StopBalloonSpawning()
    {
        AllowSpawning = false;
    }
}
