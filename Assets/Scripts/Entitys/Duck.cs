using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{

    [SerializeField] private float speed;
    private Vector3 nextPosition; //Could use transform, but we only need the .position so we can just use a vector 3
    // Start is called before the first frame update
    void Start()
    {
       
        Maths.SetBounds(new System.Numerics.Vector2(-5, 5), new System.Numerics.Vector2(0, 10), new System.Numerics.Vector2(-7, 7));
        GetNextFlyToPosition();

    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
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
}
