using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will be resposible for defining all subjects and act as a 'hub' for listeners to subscribe to various subjects!
/// </summary>
/// 

#region Player
public interface IPlayerSubject
{
    List<IPlayerObserver> PlayerObservers { get; set; }

    public void AddObserver(IPlayerObserver observer);
    public void RemoveObserver(IPlayerObserver observer);
    public void NotifyObservers(PlayerState state);
}

/// <summary>
/// Interface for observers of player, will be used so this script does not need to know about the player (decoupling)/
/// Interface is used as it supports multiple inheritance! As we may use a similar pattern for getting duck notifications
/// </summary>
public interface IPlayerObserver
{
    public void OnNotify(PlayerState state);
}
#endregion

#region Round
public interface IRoundSubject
{
    List<IRoundObserver> RoundObservers { get; set; }

    public void AddObserver(IRoundObserver observer);
    public void RemoveObserver(IRoundObserver observer);
    public void NotifyObservers(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound);
}

public interface IRoundObserver
{
    public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound);
}
#endregion

public class BroadCastManager : MonoBehaviour
{
    //Singleton creation shorthand
    public static BroadCastManager Instance => m_instance;
    private static BroadCastManager m_instance;

    //Lists of subject observers
    private List<IPlayerObserver> PlayerObservers = new();
    private List<IRoundObserver> RoundObservers = new();

    private void Awake()
    {
        m_instance = this;
    }

    public void AddPlayerObserver(IPlayerObserver observer)
    {
        PlayerObservers.Add(observer);
    }

    public List<IPlayerObserver> GetPlayerObservers()
    {
        return PlayerObservers;
    }

    public void AddRoundObserver(IRoundObserver observer)
    {
        RoundObservers.Add(observer);
    }

    public List<IRoundObserver> GetRoundObservers()
    {
        return RoundObservers;
    }

}
