using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RoundState {BIRDFLYAWAY, BIRDHIT, GAMEOVER, NEWROUND, DUCKSPAWNINTERIM, DUCKSPAWNING, DUCKSNEEDEDINCREASED, DUCKACTIVE };
public class RoundHandler : MonoBehaviour, IRoundSubject, IPlayerObserver
{
    [SerializeField] private int shots = 3;
    [SerializeField] private int birdsHit = 0;
    [SerializeField] private int birdsNeeded = 5;
    [SerializeField] private int birdCount = 0; //For checking how many birds have been spawned this round
    int round = 1;

    [SerializeField] private UnityEvent flyBirdAway;
   

    public List<IRoundObserver> RoundObservers { get; set; } //Did originally use events, but keeping track of observers was a bit obscure, so refactored and favoured this method of interface based subjects/observers. Downside is variables are set to observers regardless of if they need them
    public List<IPlayerObserver> PlayerObservers { get; set; }

    private void Start()
    {
        RoundObservers = BroadCastManager.Instance.GetRoundObservers();
        BroadCastManager.Instance.AddPlayerObserver(this);
        CheckCount(); // bird cout will be 0 so it will spawn a bird, removing the need to re-type the call to the spawn manager! 
    }

   
    public void BirdHit()
    {
        shots -= 1;
        birdsHit += 1;
        NotifyObservers(RoundState.BIRDHIT, round, birdsNeeded);
        CheckCount();
        
    }

    public void BirdMissed()
    {
        shots -= 1;
        CheckAmmo();
    }

    public void BirdTimedOUt()
    {
        NotifyObservers(RoundState.BIRDFLYAWAY, round, birdsNeeded);
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
        NotifyObservers(RoundState.NEWROUND, round, birdsNeeded);
        ResetAmmo();
        round += 1;
        GameManager.Instance.IncrementRound();
        CheckDucksNeededIncrement();
        CheckCount(); //saves us needing to repeat the call to the spawn manager!

    }

    private void CheckDucksNeededIncrement()
    {
        int round = GameManager.Instance.GetRound();

        if (birdsNeeded != 10 && round > 10)
            if (round % 3 == 0) //Would do it like duck hunter (11,13,15,20) but I like this approach (even if it is off from OG duckhunt by one round)
            {
                birdsNeeded += 1;
                NotifyObservers(RoundState.NEWROUND, round, birdsNeeded);
            }
    }

    IEnumerator DuckSpawnInterim()
    {
        birdCount += 1;
        yield return new WaitForSeconds(1);
        ResetAmmo();
        NotifyObservers(RoundState.DUCKSPAWNING, round, birdsNeeded);
        SpawnManager.Instance.SpawnBird();
        NotifyObservers(RoundState.DUCKACTIVE, round, birdsNeeded); //Will be used as flag by player
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

    public void NotifyObservers(RoundState state, int _currentRound, int _birdsNeeded)
    {
        //Notify observers about what has happened e.g. missed duck or shot duck
        foreach (var observer in RoundObservers)
        {
            observer.OnNotify(state, _currentRound, _birdsNeeded);
        }
    }
}
