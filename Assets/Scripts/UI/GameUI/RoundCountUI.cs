using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCountUI : MonoBehaviour, IRoundObserver
{
    [SerializeField] private TMP_Text roundText;
    const string prefix = "R = ";
    

    private void Start()
    {
        BroadCastManager.Instance.AddRoundObserver(this);
        roundText.SetText(prefix + 1);
    }

    public void IncrementRound(int round)
    {
        roundText.SetText(prefix + round);
    }

    public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded)
    {
        switch (state)
        {
            
            case RoundState.NEWROUND:
                IncrementRound(_currentRound);
                break;
            
        }
    }
}
