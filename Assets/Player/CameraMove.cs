using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public float cameraSpeed = 5.0f;

	public GameObject player;

	public static CameraMove instance;

	public CameraMove cameraMove;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		cameraMove = GetComponent<CameraMove>();
	}

	void FixedUpdate()
	{
		Vector3 dir = player.transform.position - this.transform.position;
		Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
		this.transform.Translate(moveVector);
	}

	public void CameraEvent()
	{
		CameraMove1.instance.cameraMove.enabled = true;
		cameraMove.enabled = false;
	}
}
