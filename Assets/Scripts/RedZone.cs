using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    // Reference to the GameManager to access lives and game state
    public GameManager gameManager;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bee"))
        {
            Debug.Log("Bee entered the Red Zone! Losing 1 life.");

            // Call the BeeController's method to handle bee death and respawn
            BeeController beeController = other.GetComponent<BeeController>();
            if (beeController != null)
            {
                beeController.DecreaseLivesAndRespawn();
            }
        }
    }
}


