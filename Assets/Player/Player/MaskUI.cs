using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MonoBehaviour
{
    public static MaskUI instance;

    public Slider maskBar;
    public float currentMask;
    public float maskMax;

    void Awake()
    {
        instance = this;
		maskMax = 100;
	}

    void Start()
    {
		maskBar = GetComponent<Slider>();

	}

    void Update()
    {
		maskBar.value = Player.instance.maskDurability / maskMax;
	}
}
