
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;
using Data.Persistent;
using Data.JSON;
using Data.Objects;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        const string highScoreTextPrefix = "Top Score = ";

        [SerializeField] private EventReference musicReference;
        public EventInstance musiceInstance { get; private set; } //Not much going on in the main menu so thought it was acceptable to have one audio instance handled here

        // Start is called before the first frame update
        void Start()
        {
            //Check a save file exists before innit the save data
            if (JSONManager.CheckIfSaveExits())
            {
                PlayerScore highScore = PersistentData.GetHighScores()[0];
                highScoreText.SetText(highScoreTextPrefix + highScore.playeName + ": " + highScore.score);
            }

            //Wont use the Audio.Playback namespace as it releases events from mmory and this is looping so wan't to release it at a certain time
            musiceInstance = RuntimeManager.CreateInstance(musicReference);
            musiceInstance.start();

        }


        private void OnDestroy()
        {
            musiceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musiceInstance.release();
        }
    }
}