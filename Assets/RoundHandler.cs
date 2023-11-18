using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Interface for observers of player, will be used so this script does not need to know about the player (decoupling)/
/// Interface is used as it supports multiple inheritance! As we may use a similar pattern for getting duck notifications
/// </summary>
public interface IPlayerObserver
{
    public void OnNotify(PlayerState state);
}



public class RoundHandler : MonoBehaviour, IPlayerObserver
{
    [SerializeField] private int shots = 3;
    [SerializeField] private int birdsHit = 0;
    [SerializeField] private int birdsNeeded = 5;
    [SerializeField] private int birdCount = 0; //For checking how many birds have been spawned this round

    [SerializeField] private UnityEvent flyBirdAway;
    
   
    private void Start()
    {
        GameManager.Instance.AddPlayerObserver(this);
        CheckCount(); // bird cout will be 0 so it will spawn a bird, removing the need to re-type the call to the spawn manager!
    }

    public void BirdHit()
    {
        shots -= 1;
        birdsHit += 1;
        CheckCount();
        ResetAmmo();
    }

    public void BirdMissed()
    {
        shots -= 1;
        CheckAmmo();
    }

    public void BirdTimedOUt()
    {
        CheckCount();
    }

    private void CheckCount()
    {
        //If bird count is max in a round then check results, else spawn another bird!
        if (birdCount == 10)
            RoundResults();
        else
            birdCount += 1;
            SpawnManager.Instance.SpawnBird();
    }

    private void CheckAmmo()
    {
        if (shots == 0)

        {
            flyBirdAway.Invoke();
            ResetAmmo();
        }
    }

    private void ResetAmmo()
    {
           shots = 3;
    }

    private void RoundResults()
    {
        if (birdsHit == birdsNeeded)
            //Increment round here
            NewRound();
    }

    private void NewRound()
    {
        //Calculate birds needed for this round!
        birdsHit = 0;
        birdCount = 0;
        ResetAmmo();
        CheckCount(); //saves us needing to repeat the call to the spawn manager!

    }

    public void OnNotify(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.DUCK_SHOT:
                BirdHit();
                break;

            case PlayerState.DUCK_MISSED:
                BirdMissed();
                break;
        }
    }
}
