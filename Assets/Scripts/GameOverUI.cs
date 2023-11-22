using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameOverUI : MonoBehaviour, IRoundObserver
{

    [SerializeField] private GameObject GameOverUIObject;
    void Start()
    {
        BroadCastManager.Instance.AddRoundObserver(this);
    }

    void ShowGameOverUI()
    {
        GameOverUIObject.SetActive(true);
    }
    public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        switch (state)
        {
            case RoundState.GAMEOVER:
                ShowGameOverUI();
                break;
        }
    }
}
