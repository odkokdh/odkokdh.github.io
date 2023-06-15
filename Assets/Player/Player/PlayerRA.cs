using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRA : MonoBehaviour
{
    public static PlayerRA instance;

    public Slider RABar;
    public float currentRA;
    public float RAMax;

    void Awake()
    {
        instance = this;
		currentRA = 100;
		RAMax = 100;

	}

    void Start()
    {
		RABar = GetComponent<Slider>();
    }

    void Update()
    {
		RABar.value = currentRA / RAMax;
        
        if (currentRA < 0)
        {
            currentRA = 0;
        }

		if (currentRA > RAMax)
		{
			currentRA = RAMax;
		}
	}
}
