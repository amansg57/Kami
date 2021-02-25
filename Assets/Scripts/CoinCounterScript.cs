using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounterScript : MonoBehaviour
{
    public IntVariable coinCounter;
    private TMP_Text coinCountText;

    private void Start()
    {
        coinCountText = GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        coinCountText.text = coinCounter.Value.ToString();
    }
}
