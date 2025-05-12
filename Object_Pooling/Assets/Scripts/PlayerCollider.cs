using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.Die();
    }
}