using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;
    const string prefix = "R = ";
    int round = 1;

    private void Start()
    {
        roundText.SetText(prefix + round);
    }

    public void IncrementRound()
    {
        round += 1;

        roundText.SetText(prefix + round);
    }
}
