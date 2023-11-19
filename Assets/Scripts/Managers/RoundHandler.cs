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
    [SerializeField] private List<UnityEvent> uiEventList = new(); //Just used for proof of concept hope to move this into a broadcast manager for better architecture! Index note: 0 reset ammo UI, 1 increment duck count, 2 reset duck count


    private void Start()
    {
        GameManager.Instance.AddPlayerObserver(this);
        CheckCount(); // bird cout will be 0 so it will spawn a bird, removing the need to re-type the call to the spawn manager! 
    }

   

    public void BirdHit()
    {
        shots -= 1;
        birdsHit += 1;
        uiEventList[1].Invoke();
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
        uiEventList[6].Invoke();
        CheckCount();
    }

    private void CheckCount()
    {
        //If bird count is max in a round then check results, else spawn another bird!
        if (birdCount == 10)
        {
            RoundResults();
        }
        else
        {
            SpawnManager.Instance.SpawnBird();
            birdCount += 1;
            uiEventList[7].Invoke();
        }
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
        uiEventList[0].Invoke();
    }

    private void RoundResults()
    {
        if (birdsHit >= birdsNeeded)
            //Increment round here
            NewRound();
        else
            GameManager.Instance.GameOver();
    }

    private void NewRound()
    {
        //Calculate birds needed for this round!
        birdsHit = 0;
        birdCount = 0;
        uiEventList[2].Invoke();
        ResetAmmo();
        uiEventList[3].Invoke();
        GameManager.Instance.IncrementRound();

        CheckDucksNeededIncrement();
        CheckCount(); //saves us needing to repeat the call to the spawn manager!

    }

    private void CheckDucksNeededIncrement()
    {
        int round = GameManager.Instance.GetRound();

        if (birdsNeeded != 10 && round > 10)
            if (round % 3 == 0) //Would do it like duck hunter (11,13,15,20) but I like the thi approach (even if it is off by OG duckhunt by one round)
            {
                birdsNeeded += 1;
                uiEventList[4].Invoke();
            }
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
