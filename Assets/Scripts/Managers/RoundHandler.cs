using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RoundState { BIRDFLYAWAY, GAMEOVER, NEWROUND, DUCKSPAWNINTERIM, DUCKSPAWNING, DUCKSNEEDEDINCREASED, DUCKACTIVE, ROUNDINTERIM }; //Change all to duck or bird, be consistent
public class RoundHandler : MonoBehaviour, IRoundSubject, IPlayerObserver
{
    [SerializeField] private int shots = 3;
    [SerializeField] private int birdsHit = 0;
    [SerializeField] private int birdsNeeded = 6;
    [SerializeField] private int birdCount = 0; //For checking how many birds have been spawned this round
    int round = 1;
    bool isPerfectRound = false;

    [SerializeField] private UnityEvent flyBirdAway;
    UnityAction newDuckAction; //May be able to remove and allow bird missed to just be handled by the event created
    

    public List<IRoundObserver> RoundObservers { get; set; } //Did originally use events, but keeping track of observers was a bit obscure, so refactored and favoured this method of interface based subjects/observers. Downside is variables are set to observers regardless of if they need them
    public List<IPlayerObserver> PlayerObservers { get; set; }

    private void Start()
    {
        newDuckAction += CheckCount;

        RoundObservers = BroadCastManager.Instance.GetRoundObservers();
        BroadCastManager.Instance.DuckFlownAway.AddListener(newDuckAction);
        BroadCastManager.Instance.DuckDead.AddListener(newDuckAction);
        BroadCastManager.Instance.AddPlayerObserver(this);

        CheckCount(); // bird cout will be 0 so it will spawn a bird, removing the need to re-type the call to the spawn manager! 
    }


    public void BirdHit()
    {
        shots -= 1;
        birdsHit += 1;
    
       // CheckCount();

    }

    public void BirdMissed()
    {
        shots -= 1;
        CheckAmmo();
    }

    public void BirdTimedOUt()
    {
        NotifyObservers(RoundState.BIRDFLYAWAY, round, birdsNeeded, isPerfectRound);
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
            StartCoroutine(DuckSpawnInterim());
        }
    }

    private void CheckAmmo()
    {
        if (shots == 0)
        {
            SpawnManager.Instance.DespawnBird(); // need to chagne
            BirdTimedOUt();
            ResetAmmo();
        }
    }

    private void ResetAmmo()
    {
        shots = 3;
    }

    private void RoundResults()
    {
        if (birdsHit >= birdsNeeded)
        {
            IsPerfectRound();
            StartCoroutine(RoundInterim());
        }
        else
        {
            GameManager.Instance.GameOver();
        }
    }

    private void NewRound()
    {
        //Calculate birds needed for this round!
        birdsHit = 0;
        birdCount = 0;
        round += 1;
        NotifyObservers(RoundState.NEWROUND, round, birdsNeeded, isPerfectRound);
        ResetAmmo();
        GameManager.Instance.IncrementRound();
        CheckDucksNeededIncrement();
        CheckCount(); //saves us needing to repeat the call to the spawn manager!

    }

    private void CheckDucksNeededIncrement()
    {
        if (birdsNeeded != 10 && round > 10)
            if (round % 3 == 0) //Would do it like duck hunter (11,13,15,20) but I like this approach (even if it is off from OG duckhunt by one round)
            {
                birdsNeeded += 1;
                NotifyObservers(RoundState.DUCKSNEEDEDINCREASED, round, birdsNeeded, isPerfectRound);
            }
    }
    void IsPerfectRound()
    {
        if (birdsHit == 10)
        {
            isPerfectRound = true;
            GameManager.Instance.RoundBonus();
        }
        else
        {
            isPerfectRound = false;
        }
    }

    IEnumerator DuckSpawnInterim()
    {
        
        yield return new WaitForSeconds(1);
        ResetAmmo();
        NotifyObservers(RoundState.DUCKSPAWNING, round, birdsNeeded, isPerfectRound);
        SpawnManager.Instance.SpawnBird();
        birdCount += 1;
        NotifyObservers(RoundState.DUCKACTIVE, round, birdsNeeded, isPerfectRound); //Will be used as flag by player
    }

    IEnumerator RoundInterim()
    {
        NotifyObservers(RoundState.ROUNDINTERIM, round, birdsNeeded, isPerfectRound);
        yield return new WaitForSeconds(3);
        NewRound();
    }

    //Define specific interface so it can be distinguished, as I plan on using various interface based subjects for observer pattern
    void IPlayerObserver.OnNotify(PlayerState state)
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

    //Anything not wanting to use the broadcast manager can call this function to add observer
    public void AddObserver(IRoundObserver observer) //MAYBE CHAage to initialise observers with no argument! called on start
    {
        RoundObservers.Add(observer);
    }

    public void RemoveObserver(IRoundObserver observer)
    {
        RoundObservers.Remove(observer);
    }

    public void NotifyObservers(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        //Notify observers about what has happened e.g. missed duck or shot duck
        foreach (var observer in RoundObservers)
        {
            observer.OnNotify(state, _currentRound, _birdsNeeded, _isPerfectRound);
        }
    }
}
