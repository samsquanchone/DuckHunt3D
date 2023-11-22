
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data.Persistent;
using Data.Objects;


namespace UI.LeaderBoards.NameInput
{
    public class NewScoreInput : MonoBehaviour
    {
        [SerializeField] Button enterValueButton;
        [SerializeField] TMP_Text scoreText;
        private const string scorePrefix = "Score: ";

        [SerializeField] TMP_InputField nameInput;

        [SerializeField] GameObject leaderboardsObject;
        // Start is called before the first frame update
        void Start()
        {
            if (PersistentData.GetHighScores().Count == 0 || PersistentData.GetHighScores().Count < 10)
            {
                SetButtonListner();
                SetScoreText();
            }
            //If the player has got a new high score then allow them to enter their details, if not show the high score boards, also check the list is above size of 0, could remove this or create catch as once fully implemented it should not need to check for this
            else if (PersistentData.GetGameFinishedStats().Item1 > PersistentData.GetHighScores().Last().score)
            {
                SetButtonListner();
                SetScoreText();
            }
            else
            {
                ActivateLeaderBoards();
            }
        }

        void EnterScoreIntoLeaderBoards()
        {

            //Create new score
            PlayerScore newScore = new();
            newScore.playeName = nameInput.text;
            newScore.score = PersistentData.GetGameFinishedStats().Item1;
            newScore.round = PersistentData.GetGameFinishedStats().Item2;
            PersistentData.AddAndSortHighScores(newScore);

            //Activate leaderboards
            ActivateLeaderBoards();

        }

        void SetScoreText()
        {
            scoreText.SetText(scorePrefix + PersistentData.GetGameFinishedStats().Item1);
        }

        void SetButtonListner()
        {
            enterValueButton.onClick.AddListener(EnterScoreIntoLeaderBoards);
        }

        void ActivateLeaderBoards()
        {
            leaderboardsObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

}