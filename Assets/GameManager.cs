using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance => m_instance;
    private static GameManager m_instance;

    [SerializeField] ScoreUI scoreUI; //This is just to get the full game loop and most of the features required! Should move to our purposed broadcast manager and add to a score UI event system!

    private List<IPlayerObserver> playerObservers = new();


    int score = 0;
    void Awake()
    {
        m_instance = this;
    }
    public void AddPlayerObserver(IPlayerObserver observer)
    {
        playerObservers.Add(observer);
    }

    public List<IPlayerObserver> GetPlayerObservers()
    {
        return playerObservers;
    }

    //Should move this out of here once we set up the proper broadcast manager!
    public void IncrementScore(int scoreToIncrementBy)
    {
        score += scoreToIncrementBy;
        //Pop up score for what the score is and maybe pass vector 2 of position!
        scoreUI.SetScore(score);

    }

    public void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
