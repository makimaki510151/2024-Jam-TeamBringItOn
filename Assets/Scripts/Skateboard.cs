using UnityEngine;

public class Skateboard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().SkateboardTime();
            Destroy(collision.gameObject);
        }
    }
}
