using UnityEngine;

public class AutoSlideZone : MonoBehaviour
{
    [Header("Vector-Style Slide Settings")]
    [SerializeField] float slideSpeed = 12f; // Fast, aggressive slide speed
    [SerializeField] float slideDuration = 1.0f; // How long the slide lasts (optional backup)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // 1. Calculate direction (based on where the zone is facing)
                // Assuming the zone creates a slide to the RIGHT. 
                // Multiply by -1 if your game goes left.
                Vector2 forcedVelocity = new Vector2(slideSpeed, 0);

                // 2. Command the player to start the sequence
                player.StartAutomatedSlide(forcedVelocity);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // 3. Return control when they leave the box
                player.StopAutomatedSlide();
            }
        }
    }
}