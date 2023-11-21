using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum DuckState {DYING, DEAD, FLYINGAWAY, FLEWAWAY }



//Could add interface for further development to ensure that certain functions get implemented, then mark any to override as virtual. But for now this will act as a base class
public class Duck : MonoBehaviour, IPlayerObserver
{

    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float rotationSpeed;
    protected Quaternion lookRotation;
    protected Vector3 nextPosition; //Could use transform, but we only need the .position so we can just use a vector 3
    protected Vector3 direction;

    protected float duration = 5f;
    protected float timeOnScreen = 1;

   

    protected DuckAnimController duckAnim; //Could use events but it is on same object so will just used get component method

    protected PlayerState playerState = PlayerState.DUCK_MISSED; //Used as a flag so the bird is not flown away during death animation

    
    // Start is called before the first frame update
    protected void Start()
    {
        duckAnim = GetComponent<DuckAnimController>(); //Get script which handles animations so we can set anim triggers
    }

    //Called when pool is filled as we want to add this as player observer, but is being set to inactive so failing to register as observer1
    public void Innit()
    {
        BroadCastManager.Instance.AddPlayerObserver(this); //Due to the pooling manager this is only done once on game scene start :)
        
    }

    protected void OnEnable()
    {
        duration = 5f;
        timeOnScreen = 1; //We will just start from 1 so we don't devide by 0
        movementSpeed = Maths.CalculateBirdSpeed(GameManager.Instance.GetRound()); //Calculate a random duck speed based off the current round
        GetNextFlyToPosition();
        StartCoroutine(FlyAwayTimer());
    }

    //Fixed update for smoother movement
    protected void FixedUpdate()
    {
        timeOnScreen += Time.deltaTime;
       
        CalculateMovement();

        CalculateRotation();
       
    }

    protected void CalculateMovement()
    {
        // Move our position a step closer to the target.
        var step = movementSpeed * Time.deltaTime; // calculate speed to move (mvoement)

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, nextPosition) < 0.001f)
        {
            //Calculate a new position
            GetNextFlyToPosition();

        }
    }

    protected void CalculateRotation()
    {
        direction = (nextPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected void GetNextFlyToPosition()
    {
        if (playerState != PlayerState.DUCK_SHOT)
        {
            nextPosition.x = Maths.GetRandomPosition3D().X;
            nextPosition.y = Maths.GetRandomPosition3D().Y;
            nextPosition.z = Maths.GetRandomPosition3D().Z;
        }
    }

    public void FlyAway()
    {
        BroadCastManager.Instance.DuckFlownAway.Invoke();
    }

    //Will use polymorphism to override this function for rare duck to allow it add bonus points!
    virtual protected void CalculateScore()
    {
        int score = Maths.CalculateBirdShotScore(timeOnScreen, movementSpeed);
        GameManager.Instance.IncrementScore(score, new Vector2(this.transform.position.x, this.transform.transform.position.y));
    }


    //Time taken for duck to time out and begin the fly away offscreen sequence
    protected IEnumerator FlyAwayTimer()
    {
        yield return new WaitForSeconds(duration);
        if(playerState != PlayerState.DUCK_SHOT)
        BroadCastManager.Instance.DuckFlyingAway.Invoke();
        StartCoroutine(DespawnTimer());
        
    }
    //Time taken for animations or to fly off screen
    protected IEnumerator DespawnTimer()
    {

        if (playerState == PlayerState.DUCK_SHOT)
        {
           
            movementSpeed = 0;
            nextPosition = new Vector3(transform.position.x, -5, transform.position.z); //Freezebird
            duckAnim.SetAnimState(DuckAnimState.DEATH); //Set duck death animation to trigger

            yield return new WaitForSeconds(0.1f);
            duckAnim.SetAnimState(DuckAnimState.FALLING); //Set duck death animation to trigger
            transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.y, 0);
            movementSpeed = 8;

            yield return new WaitForSeconds(2);
            //Death animations
            BroadCastManager.Instance.DuckDead.Invoke();
            playerState = PlayerState.DUCK_MISSED; //A bit confusing this, but the flag is local and just saves having to add another flag, so serves its purpose
            
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

    //Can't use polymorphism on this interface function, so remember to add if making new duck types!
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
