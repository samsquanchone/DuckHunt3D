using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Broadcast;

namespace UI.GamePlay.GameOver
{
    public class GameOverUI : MonoBehaviour, IRoundObserver
    {

        [SerializeField] private GameObject gameOverUIObject;
        void Start()
        {
            BroadCastManager.Instance.AddRoundObserver(this);
        }

        void ShowGameOverUI()
        {
            gameOverUIObject.SetActive(true);
        }
        public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
        {
            switch (state)
            {
                case RoundState.GAME_OVER:
                    ShowGameOverUI();
                    break;
            }
        }
    }
}