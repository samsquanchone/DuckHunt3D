
using System.Collections.Generic;
using UnityEngine;
using Data.Persistent;
using Data.Objects;
using UI.LeaderBoards.ScoreInstantiation;

namespace UI.LeaderBoards.ScoreBoard
{

    public class ScoreHandler : MonoBehaviour
    {
        //Get reference to the score prefab
        [SerializeField] private GameObject scoreUIObject;
        [SerializeField] private GameObject scoreUIParentContainer;

        //As the entry into the leaderboards can be seperated (thanks to composition), if enabaling the scoreboard object the entry would have been added / no new high score!
        private void OnEnable()
        {
            CreateScoreBoard();
        }

        public void CreateScoreBoard()
        {
            List<PlayerScore> leaderBoards = PersistentData.GetHighScores();

            //Instantiate the amount of scoreUIs as is in the scores container and set the scores (Vertical layout group and content fitter will handle the rest :))
            foreach (var score in PersistentData.GetHighScores())
            {
                GameObject _obj = Instantiate(scoreUIObject, scoreUIParentContainer.transform);

                _obj.GetComponent<ScoreEntry>().SetUIValues(score);

            }
        }
    }
}