using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeNessSpecialEffect : MonoBehaviour
{
    Player player;

    public void Init(Player player)
    {
        this.player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // “G‚Éƒqƒbƒg
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().DirectShot(player.character);
        }
    }
}
