using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enum used with the event system so difffrent parts of the code base can make decisions on an event invoke based on the enum state passed with the invoke of dialogue obserbers
/// </summary>
public enum PlayerState { DUCK_SHOT, DUCK_MISSED };

public class PlayerInput : MonoBehaviour, IPlayerSubject, IRoundObserver
{
    public List<IPlayerObserver> PlayerObservers { get; set; }

    RoundState roundState; //Will be used as a flag by the player so they can't shoot inbetween rounds

    // Start is called before the first frame update
    void Start()
    {

        //playerObservers = new(GameManager.Instance.GetPlayerObservers()); //Set up observer list, currently everything is j on start and end observer wise, but can modify if observer lifecycle needs to be more dynamic!
        StartCoroutine("LateStart");
        BroadCastManager.Instance.AddRoundObserver(this);
    }

    //Cheesy fix for now as Unity changed order of execution when i last built game. Should sort out order of execution in player settings!
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerObservers = new(BroadCastManager.Instance.GetPlayerObservers()); //Set up 

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
        if (roundState == RoundState.DUCKACTIVE)
        {
            // Construct a ray from the current touch coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            Debug.Log("Player has shot");

            RaycastHit hit;
            // Create a particle if hit
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.CompareTag("Duck"))
                {
                    Debug.Log("Duck Hit");
                    this.NotifyObservers(PlayerState.DUCK_SHOT);
                    PoolingManager.Instance.CoolObject(hit.collider.gameObject, PoolingObjectType.DUCK); //Could you observer pattern here, but we have the gameobject to return to pool, so let's not overcomplicate it and can just use the singleton!

                }
                else
            {

                Debug.Log("Duck Missed");
                NotifyObservers(PlayerState.DUCK_MISSED);
            }
               
            }
            else
            {

                Debug.Log("Duck Missed");
                NotifyObservers(PlayerState.DUCK_MISSED);
            }
        }
    }


    public void AddObserver(IPlayerObserver observer)
    {
        PlayerObservers.Add(observer);
    }

    public void NotifyObservers(PlayerState state)
    {
        //Notify observers about what has happened e.g. missed duck or shot duck
        foreach (var observer in PlayerObservers)
        {
            observer.OnNotify(state);
        }
    }

    public void RemoveObserver(IPlayerObserver observer)
    {
        PlayerObservers.Remove(observer);
    }

    public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        roundState = state;
    }
}

