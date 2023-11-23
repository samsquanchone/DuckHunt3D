using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility.Broadcast;
using GamePlay.Manager;

namespace Gameplay.Round
{

    public class RoundHandler : MonoBehaviour, IRoundSubject, IPlayerObserver
    {
        [SerializeField] private int shots = 3;
        [SerializeField] private int birdsHit = 0;
        [SerializeField] private int birdsNeeded = 6;
        [SerializeField] private int birdCount = 0; //For checking how many birds have been spawned this round
        int round = 1;
        bool isPerfectRound = false;

        UnityAction newDuckAction; //May be able to remove and allow bird missed to just be handled by the event created

        public List<IRoundObserver> RoundObservers { get; set; } //Did originally use events, but keeping track of observers was a bit obscure, so refactored and favoured this method of interface based subjects/observers. Downside is variables are sentt to observers regardless of if they need them


        private void Start()
        {
            newDuckAction += CheckCount;

            AddObservers();
            BroadCastManager.Instance.DuckFlownAway.AddListener(newDuckAction);
            BroadCastManager.Instance.DuckDead.AddListener(newDuckAction);
            BroadCastManager.Instance.AddPlayerObserver(this);


            StartCoroutine(StartRoundTimer());

        }

        IEnumerator StartRoundTimer()
        {
            yield return new WaitForSeconds(0.5f);
            NotifyObservers(RoundState.NEW_ROUND, round, birdsNeeded, isPerfectRound);

            yield return new WaitForSeconds(0.5f);
            CheckCount(); // bird cout will be 0 so it will spawn a bird, removing the need to re-type the call to the spawn manager! 
        }


        public void BirdHit()
        {
            NotifyObservers(RoundState.DUCK_NOT_ACTIVE, round, birdsNeeded, isPerfectRound);
            shots -= 1;
            birdsHit += 1;
        }

        public void BirdMissed()
        {
            shots -= 1;
            CheckAmmo();
        }

        public void BirdTimedOUt()
        {
            NotifyObservers(RoundState.DUCK_FLY_AWAY, round, birdsNeeded, isPerfectRound);
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
                BroadCastManager.Instance.DuckFlyingAway.Invoke();
                NotifyObservers(RoundState.DUCK_NOT_ACTIVE, round, birdsNeeded, isPerfectRound);
                //NotifyObservers(RoundState.BIRDFLYAWAY, round, birdsNeeded, isPerfectRound);
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
                NotifyObservers(RoundState.GAME_OVER, round, birdsNeeded, isPerfectRound);
                GameManager.Instance.GameOver();
            }
        }

        private void NewRound()
        {
            //Calculate birds needed for this round!
            birdsHit = 0;
            birdCount = 0;
            round += 1;
            NotifyObservers(RoundState.NEW_ROUND, round, birdsNeeded, isPerfectRound);
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
                    NotifyObservers(RoundState.DUCKS_NEEDED_INCREASED, round, birdsNeeded, isPerfectRound);
                }
        }
        void IsPerfectRound()
        {
            if (birdsHit == 10)
            {
                isPerfectRound = true;
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
            NotifyObservers(RoundState.DUCK_SPAWNING, round, birdsNeeded, isPerfectRound);
            birdCount += 1;
            NotifyObservers(RoundState.DUCK_ACTIVE, round, birdsNeeded, isPerfectRound); //Will be used as flag by player
        }

        IEnumerator RoundInterim()
        {
            NotifyObservers(RoundState.ROUND_INTERIM, round, birdsNeeded, isPerfectRound);
            yield return new WaitForSeconds(3.5f);
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
        public void AddObservers()
        {
            RoundObservers = BroadCastManager.Instance.GetRoundObservers();
        }


        //Will just remove everything on destroy, should be GC'd, but C++ habbits die hard. The list also obtained by ref as we wont clone it (new), so should destroy the list in broadcast manager!
        public void RemoveObservers()
        {
            RoundObservers.Clear();
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
}