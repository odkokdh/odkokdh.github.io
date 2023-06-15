using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP instance;

    public Slider HPBar;
    public float currentHP;
    public float HPMax;

    void Awake()
    {
        instance = this;
		currentHP = 10;
        HPMax = 10;

	}

    void Start()
    {
        HPBar = GetComponent<Slider>();
    }

    void Update()
    {
        HPBar.value = currentHP / HPMax;

		if (currentHP < 0)
		{
			currentHP = 0;
		}

		if (currentHP > HPMax)
		{
			currentHP = HPMax;
		}
	}
}
