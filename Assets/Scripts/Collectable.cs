using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Detect collision with other colliders
    void OnTriggerEnter(Collider collision)
    {
        // Check if the player has collided with the collectable
        if (collision.gameObject.tag == "Player")
        {
            // Send event to the player
            collision.gameObject.GetComponent<PlayerLogic>().addCollectableEffect(randomEffect());
        }
    }

    string randomEffect()
    {
        // Return a random effect
        switch (Random.Range(0, 3))
        {
            case 0:
                return "speed";
            case 1:
                return "missile";
            case 2:
                return "shield";
        }
        return null;
    }

}

