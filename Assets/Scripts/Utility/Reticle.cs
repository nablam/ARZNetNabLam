// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;
using HoloToolkit.Unity.InputModule;

public class Reticle : MonoBehaviour {

    float baseScale;    // scale of reticle at a distance of 1 from camera

	// Use this for initialization
	void Start () {

        // get starting scale
        baseScale = transform.localScale.x;

	}
	
	// Update is called once per frame
	void LateUpdate () {

        // place object at gaze hit position
        if (GazeManager.Instance.IsGazingAtObject)
            transform.position = GazeManager.Instance.HitInfo.point;
        else
            transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 2.0f);

        // change scale by distance to camera (baseScale * distance)
        float newScale = Vector3.Distance(Camera.main.transform.position, transform.position) * baseScale;
        transform.localScale = new Vector3(newScale, newScale, 1.0f);

        // rotate to face camera (quads must look in same direction to see material)
        transform.rotation = Camera.main.transform.rotation;

    }
}
