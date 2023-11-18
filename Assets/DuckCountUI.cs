using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DuckCountUI : MonoBehaviour
{
    const string prefix = "Duck Count: ";
    const string suffix = "/10";
    [SerializeField] private TMP_Text duckCountText; //Just for prototyping
    private int duckCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        duckCountText.SetText(prefix + duckCount + suffix);
    }

    public void IncrementDuckCount()
    {
        duckCount += 1;
        duckCountText.SetText(prefix + duckCount + suffix);
    }

    //Maybe have duck missed / duck hit to fill or leave blank with an array of images! Could have hit fill in 

    public void ResetDuckCount()
    {
        duckCount = 0;
        duckCountText.SetText(prefix + duckCount + suffix);
    }
}
