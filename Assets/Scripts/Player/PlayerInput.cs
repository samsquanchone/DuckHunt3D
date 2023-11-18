using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enum used with the event system so difffrent parts of the code base can make decisions on an event invoke based on the enum state passed with the invoke of dialogue obserbers
/// </summary>
public enum PlayerState { DUCK_SHOT, DUCK_MISSED };


public interface PlayerSubject
{
    List<IPlayerObserver> playerObservers { get; set; }

    public void AddObserver(IPlayerObserver observer);
    public void RemoveObserver(IPlayerObserver observer);

    public void NotifyObservers(PlayerState state);
}
public class PlayerInput : MonoBehaviour, PlayerSubject
{
    public List<IPlayerObserver> playerObservers { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        playerObservers = new(GameManager.Instance.GetPlayerObservers()); //Set up observer list, currently everything is j on start and end observer wise, but can modify if observer lifecycle needs to be more dynamic!
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                Debug.Log("Player has shot");

                RaycastHit hit;
                // Create a particle if hit
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Duck"))
                    {
                        Debug.Log("Duck Hit");
                        PoolingManager.Instance.CoolObject(hit.collider.gameObject, PoolingObjectType.DUCK); //Could you observer pattern here, but we have the gameobject to return to pool, so let's not overcomplicate it and can just use the singleton!
                        NotifyObservers(PlayerState.DUCK_SHOT);
                    }
                    else
                    {
                        Debug.Log("Duck Missed");
                        NotifyObservers(PlayerState.DUCK_MISSED);
                    }
                }

            }


    }


    public void AddObserver(IPlayerObserver observer)
    {
        playerObservers.Add(observer);
    }

    public void NotifyObservers(PlayerState state)
    {
        //Notify observers about what has happened e.g. missed duck or shot duck
        foreach (var observer in playerObservers)
        {
            observer.OnNotify(state);
        }
    }

    public void RemoveObserver(IPlayerObserver observer)
    {
        playerObservers.Remove(observer);
    }
}

