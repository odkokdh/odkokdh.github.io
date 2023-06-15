using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public static DoorAnimation instance;

    public Animator doorAnim;
    public AudioSource sound;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    public void DoorOpen()
    {
        doorAnim.SetTrigger("Open");
        sound.Play();
    }
}
