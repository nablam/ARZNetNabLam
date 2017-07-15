using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR.WSA;

public class STEMAligner : MonoBehaviour
{

    public GameObject virtualObject;
    public GameObject trackerObject;
    public GameObject visualiser;
    public GameObject coreDevice;

    private SixenseCore.Device device;
    private WorldAnchorStore anchorStore;

    private void Start()
    {
        device = coreDevice.GetComponent<SixenseCore.Device>();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    public void Align()
    {
        // delete visualiser world anchor
        WorldAnchor anchor = visualiser.GetComponent<WorldAnchor>();
        if (anchor != null)
        {
            DestroyImmediate(anchor);
        }
        anchorStore.Delete("ARZStemBase");

        // align rotation
        Vector3 visualiserRot = visualiser.transform.rotation.eulerAngles;
        Vector3 trackerRot = trackerObject.transform.rotation.eulerAngles;
        Vector3 virtualRot = virtualObject.transform.rotation.eulerAngles;
        float rRotx = 0f; // virtualRot.x - trackerRot.x;
        float rRoty = virtualRot.y - trackerRot.y;
        float rRotz = 0f; // virtualRot.z - trackerRot.z;
        Vector3 relativeRot = new Vector3(rRotx, rRoty, rRotz);

        //Debug.Log("Visualiser Rotation: " + visualiserRot);
        //Debug.Log("Tracker Rotation: " + trackerRot);
        //Debug.Log("Virtual Rotation: " + virtualRot);
        //Debug.Log("Relative Rotation: " + relativeRot);

        visualiser.transform.Rotate(relativeRot, Space.World);

        // force update tracker after rotation before aligning position
        device.ForceUpdateTrackers();

        // align position
        float xoffset, yoffset, zoffset;
        xoffset = virtualObject.transform.position.x - trackerObject.transform.position.x;
        yoffset = virtualObject.transform.position.y - trackerObject.transform.position.y;
        zoffset = virtualObject.transform.position.z - trackerObject.transform.position.z;

        //Debug.Log("Visualiser Position Before: " + visualiser.transform.position);
        //Debug.Log("Translating: (" + xoffset + ", " + yoffset + ", " + zoffset + ")");

        visualiser.transform.Translate(new Vector3(xoffset, yoffset, zoffset), Space.World);

        //Debug.Log("Visualiser Position After: " + visualiser.transform.position);

        //save new position
        WorldAnchor attachingAnchor = visualiser.AddComponent<WorldAnchor>();
        if (attachingAnchor.isLocated)
        {
            anchorStore.Save("ARZStemBase", attachingAnchor);
        }
        else
        {
            attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
        }
    }

    private void AnchorStoreReady(WorldAnchorStore store)
    {
        // save instance
        anchorStore = store;

        anchorStore.Load("ARZStemBase", visualiser);
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            anchorStore.Save("ARZStemBase", self);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}
