// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class CameraTrackPack : MonoBehaviour {

    Vector3 originalPosition;
    Quaternion originalRotation;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
