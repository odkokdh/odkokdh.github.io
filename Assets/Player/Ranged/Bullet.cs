using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int speed = 80;

    void Update()
    {
		transform.Translate(Player.instance.ShootLocation.normalized * speed * Time.deltaTime);
        StartCoroutine(AutoDestroy());
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }

        else if (other.tag == "Enemy")
		{
			Debug.Log("Hit");
			Destroy(gameObject);
		}
	}

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSecondsRealtime(1.0f);
		Destroy(gameObject);
	}
}
