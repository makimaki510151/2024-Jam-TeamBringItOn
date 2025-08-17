using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player == null)
            {
                player = collision.gameObject.GetComponentInParent<Player>();
            }
            MainGameRoot.Instance.GoalPlayer(player.character);
        }
    }
}
