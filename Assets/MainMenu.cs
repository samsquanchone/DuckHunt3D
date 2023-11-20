using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    const string highScoreTextPrefix = "Top Score = ";
    // Start is called before the first frame update
    void Start()
    {
        if (JSONManager.CheckIfSaveExits())
        {
            PlayerScore highScore = PersistentData.GetHighScores()[0];
            highScoreText.SetText(highScoreTextPrefix + highScore.playeName + ": " + highScore.score);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
