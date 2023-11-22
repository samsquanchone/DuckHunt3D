using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    const string highScoreTextPrefix = "Top Score = ";

    [SerializeField] private EventReference musicReference;
    public EventInstance musiceInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (JSONManager.CheckIfSaveExits())
        {
            PlayerScore highScore = PersistentData.GetHighScores()[0];
            highScoreText.SetText(highScoreTextPrefix + highScore.playeName + ": " + highScore.score);
        }
        musiceInstance = RuntimeManager.CreateInstance(musicReference);
        musiceInstance.start();
        
    }
 

    private void OnDestroy()
    {
        musiceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musiceInstance.release();
    }
}
