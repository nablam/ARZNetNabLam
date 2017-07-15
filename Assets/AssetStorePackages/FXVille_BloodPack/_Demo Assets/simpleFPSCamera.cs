using UnityEngine;
using System.Collections;

public class simpleFPSCamera : MonoBehaviour 
{
	public Vector2 sensitivity = new Vector2(1f, 1f);
	public Vector2 speed = new Vector2(1f, 1f);

	public float minimumY = 60f;
	public float maximumY = 60f;

	private float rotationY = 0f;
	private float rotationX = 0f;

	// Update is called once per frame
	void Update () 
	{

		rotationX = transform.parent.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivity.x;

		rotationY += Input.GetAxis("Mouse Y") * sensitivity.y;
		rotationY = Mathf.Clamp (rotationY, -minimumY, maximumY);

		transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		transform.parent.localEulerAngles = new Vector3(0, rotationX, 0);

		transform.parent.position += transform.parent.forward * Input.GetAxis("Vertical") * speed.y * 0.1f +
									 transform.parent.right * Input.GetAxis("Horizontal") * speed.x * 0.1f;

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Time.timeScale = 0.2f;
			Debug.Log("shift");
		}
		if (Input.GetKeyDown(KeyCode.RightShift))
		{
			Time.timeScale = 1f;
			Debug.Log("unshift");
		}


	}
}
