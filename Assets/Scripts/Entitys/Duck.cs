using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum DuckState {DYING, DEAD, FLYINGAWAY, FLEWAWAY }

public class Duck : MonoBehaviour, IPlayerObserver
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private Quaternion lookRotation;
    private Vector3 nextPosition; //Could use transform, but we only need the .position so we can just use a vector 3
    private Vector3 direction;

    private float duration = 5f;
    float timeOnScreen = 1;

    UnityAction birdFlewAway;

    PlayerState playerState; //Used as a flag so the bird is not flown away during death animation
    
    // Start is called before the first frame update
    void Start()
    {
        birdFlewAway += () => SpawnManager.Instance.DespawnBird(); //Create an action that will notify spawn manager to despawn current bird when it flys away!
        //GetNextFlyToPosition();
    }

    //Called when pool is filled as we want to add this as player observer, but is being set to inactive so failing to register as observer1
    public void Innit()
    {
        BroadCastManager.Instance.AddPlayerObserver(this); //Due to the pooling manager this is only done once on game scene start :)
        
    }

    private void OnEnable()
    {
        duration = 5f;
        timeOnScreen = 1; //We will just start from 1 so we don't devide by 0
        movementSpeed = Maths.CalculateBirdSpeed(GameManager.Instance.GetRound()); //Calculate a random duck speed based off the current round
        GetNextFlyToPosition();
        StartCoroutine("FlyAwayTimer");
    }

    //Fixed update for smoother movement
    void FixedUpdate()
    {
        timeOnScreen += Time.deltaTime;
        // Move our position a step closer to the target.
        var step = movementSpeed * Time.deltaTime; // calculate speed to move (mvoement)

        direction = (nextPosition - transform.position).normalized;

        lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, nextPosition) < 0.001f)
        {
            //Calculate a new position
            GetNextFlyToPosition();

        }
    }

    private void GetNextFlyToPosition()
    {
        nextPosition.x = Maths.GetRandomPosition3D().X;
        nextPosition.y = Maths.GetRandomPosition3D().Y;
        nextPosition.z = Maths.GetRandomPosition3D().Z;
    }

    public void FlyAway()
    {
        BroadCastManager.Instance.DuckFlownAway.Invoke();
    }

    private void CalculateScore()
    {
        int score = Maths.CalculateBirdShotScore(timeOnScreen, movementSpeed);
        GameManager.Instance.IncrementScore(score, new Vector2(this.transform.position.x, this.transform.transform.position.y));
    }


    //Time taken for duck to time out and begin the fly away offscreen sequence
    IEnumerator FlyAwayTimer()
    {
        yield return new WaitForSeconds(duration);
        if(playerState != PlayerState.DUCK_SHOT)
        BroadCastManager.Instance.DuckFlyingAway.Invoke();
        StartCoroutine(DespawnTimer());
        
    }
    //Time taken for animations or to fly off screen
    IEnumerator DespawnTimer()
    {

        if (playerState == PlayerState.DUCK_SHOT)
        {
            yield return new WaitForSeconds(2);
            //Death animations
            BroadCastManager.Instance.DuckDead.Invoke();
            //SpawnManager.Instance.DespawnBird();
        }
        else
        {
            //Rocket ship to mars :P
            nextPosition = new Vector3(transform.position.x, 100, transform.position.z);
            movementSpeed = 50;

            yield return new WaitForSeconds(1);
            FlyAway();
        }
        

    }
    void IPlayerObserver.OnNotify(PlayerState state)
    {
        if (this.isActiveAndEnabled) //Could use ID but this should work fine, avoids calling behaviour on all pooled objects
        {
            playerState = state;
            switch (state)
            {
                case PlayerState.DUCK_SHOT:
                    CalculateScore();
                    StartCoroutine(DespawnTimer());
                    break;
            }
        }
    }
}
