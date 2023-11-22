using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerSubject, IRoundObserver
{
    public List<IPlayerObserver> PlayerObservers { get; set; }

    RoundState roundState; //Will be used as a flag by the player so they can't shoot inbetween rounds

    // Start is called before the first frame update
    void Start()
    {
        AddObservers();
        BroadCastManager.Instance.AddRoundObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) //Check if finger down and if it is not pressing button
            {
                PlayerShoot();
            }
    }

    void PlayerShoot()
    {
        //shoot if duck is active and gam is not paused 
        if (roundState == RoundState.DUCKACTIVE && Time.timeScale != 0)
        {
            // Construct a ray from the current touch coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            //Notify observers whether duck is hit or not
            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                if (hit.collider.CompareTag("Duck"))
                {

                    NotifyObservers(PlayerState.DUCK_SHOT);
                }
                else
                {

                    NotifyObservers(PlayerState.DUCK_MISSED);
                }
            }
            else
            {
                NotifyObservers(PlayerState.DUCK_MISSED);
            }
        }
    }

    void OnDestroy()
    {
        //Remove observers on destroy
        RemoveObservers();
    }

    //Usually I have the Interface type as an argument, but for this game it will all be done on start, so will just do it through the broadcast manager
    public void AddObservers()
    {
        PlayerObservers = BroadCastManager.Instance.GetPlayerObservers(); //Set up without cloning (new), as we want them as ref
    }

    public void NotifyObservers(PlayerState state)
    {
        //Notify observers about what has happened e.g. missed duck or shot duck
        foreach (var observer in PlayerObservers)
        {
            observer.OnNotify(state);
        }
    }

    //Getting the list by reference as it is non primitive, so will clear list here
    public void RemoveObservers()
    {
        PlayerObservers.Clear();
    }

    
    void IRoundObserver.OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        roundState = state;
    }


}


