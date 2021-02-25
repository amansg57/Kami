using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private const int playerLayer = 9;
    public IntVariable coinCount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            coinCount.Value++;
            Destroy(this.gameObject);
        }
    }
}
