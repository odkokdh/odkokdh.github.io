using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public static DoorTrigger instance;

    public bool activeDoor = false;
    private void Awake()
    {
        instance = this;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            activeDoor = true;
        }
    }
}
