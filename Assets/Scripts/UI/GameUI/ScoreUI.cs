using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreUI : MonoBehaviour
{

    const string prefix = "Score: ";
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>(); //Get ref to TMP text
        scoreText.SetText(prefix + 0);
    }

    public void SetScore(int score)
    {
        scoreText.SetText(prefix + score);
    }
}
