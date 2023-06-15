using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGB : MonoBehaviour
{
    void Start()
    {
		StartCoroutine(AutoDestroy());
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Wall")
		{
			Destroy(gameObject);
		}
	}

	IEnumerator AutoDestroy()
	{
		yield return new WaitForSecondsRealtime(1.0f);
		Destroy(gameObject);
	}
}
