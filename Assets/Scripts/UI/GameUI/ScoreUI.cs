using System.Collections;
using UnityEngine;
using TMPro;
using Utility.Broadcast;
using Data.Persistent;


namespace UI.GamePlay.Score
{
    /// <summary>
    /// Will be used to handle the overal score and the UI for it
    /// </summary>
    public class ScoreUI : MonoBehaviour, IDuckObserver, IRoundObserver
    {
        private TMP_Text overalScoreText;
        const string overalScoreSuffix = "\nScore";

        [SerializeField] TMP_Text popUpScoreText;
        const string popUpScorePrefix = "+";

        int totalScore = 0;


        private void Start()
        {
            //Set liteners to subjects
            BroadCastManager.Instance.AddDuckObserver(this);
            BroadCastManager.Instance.AddRoundObserver(this);

            overalScoreText = GetComponent<TMP_Text>(); //Get ref to TMP text
            overalScoreText.SetText(0 + overalScoreSuffix);
        }

        private void SetScore(int score)
        {
            totalScore += score;
            overalScoreText.SetText(totalScore + overalScoreSuffix);
        }

        private void SetPopUpScore(int score, Vector2 pos)
        {
            popUpScoreText.SetText(popUpScorePrefix + score);
            popUpScoreText.transform.position = pos;
            popUpScoreText.gameObject.SetActive(true);

            StartCoroutine(PopUpTimer());
        }

        void SetFinalScore(int round)
        {
            //Set persistent values for final score
            PersistentData.SetGameResults(totalScore, round);
        }

        IEnumerator PopUpTimer()
        {
            yield return new WaitForSeconds(1f);
            popUpScoreText.gameObject.SetActive(false);
        }

        public void OnNotify(int score, Vector2 position)
        {
            SetPopUpScore(score, position);
            SetScore(score);
        }

        public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
        {
            switch (state)
            {
                case RoundState.GAME_OVER:
                    SetFinalScore(_currentRound); //Set final values 
                    break;
                case RoundState.ROUND_INTERIM:
                    if (_isPerfectRound) { SetScore(1000); } //Set perfect round bonus to score
                    break;

            }
        }
    }
}