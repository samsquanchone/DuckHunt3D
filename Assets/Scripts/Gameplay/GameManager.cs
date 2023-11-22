using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Math;

public class GameManager : MonoBehaviour
{
    //Singleton 
    public static GameManager Instance => m_instance;
    private static GameManager m_instance;

    int round = 1;
    void Awake()
    {
        //Make sure there is only one instance of the singleton
        if (m_instance == null)
            m_instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1; //Just incase user goes to main menu and then play's again
        Maths.SetBounds(new System.Numerics.Vector2(-5, 5), new System.Numerics.Vector2(0.9f, 10), new System.Numerics.Vector2(-7, 1)); //Set the game bounds for getting random positions for ducks
    }

    public void IncrementRound()
    {
        round += 1;
    }

    //Just a global point for things in the scene to get the round
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
        StartCoroutine(WaitForGameOver());
    }

    //Give things a little bit to execute before changing scenes
    IEnumerator WaitForGameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.Instance.TransationToScene(2);

    }

}
