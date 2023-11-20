using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{


    public static GameManager Instance => m_instance;
    private static GameManager m_instance;

    [SerializeField] ScoreUI scoreUI; //This is just to get the full game loop and most of the features required! Should move to our purposed broadcast manager and add to a score UI event system!

    int score = 0;
    int round = 1;
    void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1; //Just incase user goes to main menu and then play's again
        Maths.SetBounds(new System.Numerics.Vector2(-5, 5), new System.Numerics.Vector2(0.9f, 10), new System.Numerics.Vector2(-7, 7)); 
    }

    //Should move this out of here once we set up the proper broadcast manager!
    public void IncrementScore(int scoreToIncrementBy, Vector2 displayPopUpPos) //This must be changed to be handled through observer pattern!
    {
        scoreUI.SetPopUpScore(scoreToIncrementBy, displayPopUpPos);
        score += scoreToIncrementBy;
        //Pop up score for what the score is and maybe pass vector 2 of position!
        scoreUI.SetScore(score);
        
    }

    public void RoundBonus()
    {
        score += 1000;
        scoreUI.SetScore(score);
    }
    public void IncrementRound()
    {
        round += 1;
    }

    public int GetRound()
    {
        return round;
    }

    public void GamePaused()
    {
        Time.timeScale = 0;
    }

    public void GameResumed()
    {
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        PersistentData.SetGameResults(score, round);
        SceneManager.Instance.TransationToScene(2);
    }

}
