using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARBullet : MonoBehaviour
{
    private int speed = 80;
    float angle;
    Vector2 target, mouse;
    Vector3 dir, ShootLocation;

    private void Start()
    {
		target = transform.position;
		mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
        dir = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

		ShootLocation = (dir - transform.position);
	}

    void Update()
    {
		transform.Translate(ShootLocation.normalized * speed * Time.deltaTime);
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
            Destroy(other.gameObject);
		}
	}

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSecondsRealtime(1.0f);
		Destroy(gameObject);
	}
}
