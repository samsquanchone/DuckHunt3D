using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DucksNeededUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text duckNeededText;

    int duckNeeded = 6; //Can remove this once we get the actual UI in!
    public void IncrementDucksNeeded()
    {
        duckNeeded += 1;
        duckNeededText.SetText("Ducks neeed: " + duckNeeded);
    }
}
