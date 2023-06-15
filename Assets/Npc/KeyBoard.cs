using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    public GameObject fireShutter;
    bool activeShutter = true;
	private IEnumerator shutterCoroutine;

	private void Awake()
    {
		shutterCoroutine = Shutter();
    }

    void Start()
    {
        fireShutter.SetActive(activeShutter);
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            if (activeShutter == false)
            {
                CameraMove.instance.CameraEvent();
                StartCoroutine(Shutter());
                StartCoroutine(shutterCoroutine);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            activeShutter = false;
        }
    }

    IEnumerator Shutter()
    {
		yield return new WaitForSecondsRealtime(1.0f);

        for (int i = 0; i < 50; i++)
        {
			yield return new WaitForSecondsRealtime(0.015f);
			fireShutter.transform.Translate(0, (float)-0.03, 0);
		}

		yield return new WaitForSecondsRealtime(0.5f);

		while (true)
        {
			yield return new WaitForSecondsRealtime(0.02f);
			fireShutter.transform.Translate(0, (float)-0.03, 0);
		}
	}

    IEnumerator remove()
    {
        yield return new WaitForSecondsRealtime(6.0f);
		StopCoroutine(shutterCoroutine);
		Destroy(fireShutter);
    }
}
