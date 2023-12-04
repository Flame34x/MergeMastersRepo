using UnityEngine;
using UnityEngine.EventSystems;

public class Border : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piece"))
        {
            EventManager.TriggerGameOver();
        }
    }
}
