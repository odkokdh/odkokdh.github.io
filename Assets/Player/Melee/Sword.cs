using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Player.instance.isAttacking == true)
        {
            anim.SetBool("Swing", true);
        }

        else
        {
			anim.SetBool("Swing", false);
		}
	}
}
