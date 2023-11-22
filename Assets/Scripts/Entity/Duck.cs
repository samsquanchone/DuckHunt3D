using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility.Math;


//Could add interface for further development to ensure that certain functions get implemented, then mark any to override as virtual. But for now this will act as a base class
public class Duck : MonoBehaviour, IPlayerObserver, IDuckSubject
{

    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float rotationSpeed;
    protected Quaternion lookRotation;
    protected Vector3 nextPosition; //Could use transform, but we only need the .position so we can just use a vector 3
    protected Vector3 direction;

    protected float duration = 5f; //Time taken until bird flys away
    protected float timeOnScreen = 1; //Time on screen to calculate score

    protected DuckAnimController duckAnim; //Could use events but it is on same object so will just used get component method

    protected PlayerState playerState = PlayerState.DUCK_MISSED; //Used as a flag so the bird is not flown away during death animation

    public List<IDuckObserver> DuckObservers { get; set; }

    protected UnityAction flyDuckAway; //Used to manually fly duck away if shots run out

    bool shouldGetPosition = true;

    // Start is called before the first frame update
    protected void Start()
    {
        AddObservers();
        BroadCastManager.Instance.AddPlayerObserver(this); //Due to the pooling manager this is only done once on game scene start :)
        flyDuckAway += ManuallyFlyDuckAway;

        BroadCastManager.Instance.DuckFlyingAway.AddListener(flyDuckAway);
        duckAnim = GetComponent<DuckAnimController>(); //Get script which handles animations so we can set anim triggers
       
    }

    protected void OnEnable()
    {
        shouldGetPosition = true;
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
        //Calculate rotation based off the new position, could increment with duck speed to make even better
        direction = (nextPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected void GetNextFlyToPosition()
    {
        if (shouldGetPosition)
        {
            //Calculate random postion to fly to
            nextPosition = new(Maths.GetRandomPosition3D().X, Maths.GetRandomPosition3D().Y, Maths.GetRandomPosition3D().Z);
        }
    }

    public void FlyAway()
    {
        BroadCastManager.Instance.DuckFlownAway.Invoke();
    }

    //Will use polymorphism to override this function for rare duck to allow it add bonus points!
    virtual protected void CalculateScore()
    {
        //Calculate the score based off time on screen and movement, calculations done in a static utillity class I created for abstracting math based code
        int score = Maths.CalculateBirdShotScore(timeOnScreen, movementSpeed);
        NotifyObservers(score, new Vector2(this.transform.position.x, this.transform.transform.position.y));
      
    }

    protected void ManuallyFlyDuckAway()
    {
        if(isActiveAndEnabled)
        StartCoroutine(DespawnTimer());
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
            shouldGetPosition = false;

            yield return new WaitForSeconds(2);
          
            BroadCastManager.Instance.DuckDead.Invoke();

            playerState = PlayerState.DUCK_MISSED; //Is just an internal flag to make sure if player does not click inbetween rounds the death sequence is not triggered instead of flyaway
            
            
        }
        else
        {
            //Get random position to fly bird off screen and set flag to false so it can't get a new position
            nextPosition = new(Maths.GetDespawnPosition().X, Maths.GetDespawnPosition().Y, Maths.GetDespawnPosition().Z);
            shouldGetPosition = false; ///Just to make sure it does not get another position before despawn
            movementSpeed = movementSpeed += 5;

            yield return new WaitForSeconds(2f);
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

    public void AddObservers()
    {
        DuckObservers = BroadCastManager.Instance.GetDuckObservers();
    }

    public void RemoveObservers()
    {
        if(DuckObservers != null) //Gc was getting to it before this script after creating build assemblies, as I am using ref to broadcast manager list
        DuckObservers.Clear();
    }

    public void NotifyObservers(int score, Vector2 position)
    {
        if (isActiveAndEnabled)
        {
            foreach (var observer in DuckObservers)
            {
                observer.OnNotify(score, position);
            }
        }
    }

    void OnDestroy()
    {
        RemoveObservers();
    }
}
