using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance => m_instance;
    private static GameManager m_instance;

    private List<IPlayerObserver> playerObservers = new();

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

}
