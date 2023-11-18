using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
                }
                else
                {
                    Debug.Log("Duck Missed");
                }
            }
            
        }


    }
}

