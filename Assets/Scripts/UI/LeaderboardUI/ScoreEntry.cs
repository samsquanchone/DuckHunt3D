
using UnityEngine;
using TMPro;
using Data.Objects;

namespace UI.LeaderBoards.ScoreInstantiation
{
    public class ScoreEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text roundText;


        //Set UI values using some lovely jubbly composition 
        public void SetUIValues(PlayerScore score)
        {
            nameText.SetText(score.playeName);
            scoreText.SetText(score.score.ToString());
            roundText.SetText(score.round.ToString());
        }
    }
}