using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utility.Broadcast;

namespace UI.GamePlay.RoundInterim
{
    public class RoundInterimUI : MonoBehaviour, IRoundObserver
    {
        [SerializeField] private GameObject roundCountObject;
        private TMP_Text roundText;
        const string roundTextPrefix = "Round\n";

        [SerializeField] private GameObject perfectRoundObject;

        private void Start()
        {
            BroadCastManager.Instance.AddRoundObserver(this);
            roundText = roundCountObject.GetComponentInChildren<TMP_Text>(); //Get text reference
        }

        void SetRoundText(int round)
        {
            roundText.text = roundTextPrefix + round;
            roundCountObject.SetActive(true);
        }

        void DisableRoundText()
        {
            roundCountObject.SetActive(false);
        }

        void ActivatePerfectRoundText(bool isPerfectRound)
        {
            if (isPerfectRound)
                perfectRoundObject.SetActive(true);
        }

        void DisablePerfectRoundText()
        {
            perfectRoundObject.SetActive(false);
        }

        public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
        {
            switch (state)
            {
                case RoundState.NEWROUND:
                    SetRoundText(_currentRound);
                    DisablePerfectRoundText();
                    break;
                case RoundState.DUCKACTIVE:
                    DisableRoundText();
                    break;
                case RoundState.ROUNDINTERIM:
                    ActivatePerfectRoundText(_isPerfectRound);
                    break;
            }
        }
    }
}