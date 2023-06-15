using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove1 : MonoBehaviour
{
	public float cameraSpeed = 5.0f;

	public GameObject door;

	public static CameraMove1 instance;

	public CameraMove1 cameraMove;

	private void Awake()
	{
		instance = this;
		cameraMove = GetComponent<CameraMove1>();
	}

	private void Start()
	{
		StartCoroutine(back());
	}

	void FixedUpdate()
	{
		Vector3 dir = door.transform.position - this.transform.position;
		Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
		this.transform.Translate(moveVector);
	}

	IEnumerator back()
	{
		yield return new WaitForSecondsRealtime(5.0f);
		CameraMove.instance.cameraMove.enabled = true;
		cameraMove.enabled = false;
	}
}
