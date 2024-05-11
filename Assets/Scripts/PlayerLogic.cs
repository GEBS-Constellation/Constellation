using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KarMovement;

public class PlayerLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addCollectableEffect(string effect)
    {
        // Check what effect the collectable has
        switch (effect)
        {
            case "speed":
                // Increase the speed of the player
                GetComponent<KarMovement>().force *= 2;
                break;
            case "missile":
                // Give the player a missile
                break;
            case "shield":
                // Give the player a shield
                break;
        }
    }
}
