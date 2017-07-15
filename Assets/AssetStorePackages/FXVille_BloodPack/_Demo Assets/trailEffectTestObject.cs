using UnityEngine;
using System.Collections;

public class trailEffectTestObject : MonoBehaviour 
{
	public float speed = 0.5f;
	public float width = 5f;
	public float height = 1f;
	public float wiggleFreq = 4f;

	public Vector3 centerPosition;

	public float localTime = 0f;
	// Use this for initialization
	void Start () 
	{
		centerPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		localTime += Time.deltaTime * speed;

		Vector3 tempPos = Vector3.zero;
		tempPos.x = Mathf.Sin (localTime) * width;
		tempPos.z = Mathf.Cos (localTime) * width;
		tempPos.y = Mathf.Sin (localTime * wiggleFreq) * height;

		transform.localPosition = tempPos + centerPosition;
	}
}
