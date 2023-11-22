using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{


    public static GameManager Instance => m_instance;
    private static GameManager m_instance;

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
        StartCoroutine(WaitForGameOver());
    }

    //Shoul move and not ref the ui here
    IEnumerator WaitForGameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.Instance.TransationToScene(2);

    }

}
