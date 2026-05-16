using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private Transform mainCam;

	private void OnEnable()
	{
		mainCam = FindAnyObjectByType<Camera>().transform;
		Debug.Log("Main Cam = " + mainCam.name);
	}

	private void LateUpdate()
	{
		transform.LookAt(mainCam);
		transform.RotateAround(transform.position, transform.up, 180f);
	}
}
