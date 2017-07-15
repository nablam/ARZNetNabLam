// Modified from Persistomatic class as posted by @Patrick from https://forums.hololens.com/discussion/514/creating-and-assigning-spatial-anchors
// Modified by Jeffrey M. Paquette

using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class PersistoMatic : MonoBehaviour
{
    public string AnchorStoreBaseName;
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    public bool placeParent = false;

    WorldAnchorStore anchorStore;

    bool Placing = false;
    string anchorStoreName;
    bool rotateOnNormals = false;
    bool keepUpright = false;
    bool faceCamera = false;

    Transform trans;
    // Use this for initialization
    void Awake()
    {
        if (placeParent)
            trans = transform.parent;
        else
            trans = transform;
    }

    public void SetAnchorStoreName(string value)
    {
        anchorStoreName = value;
        AnchorStoreBaseName = value;
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    public string GetAnchorStoreName()
    {
        return anchorStoreName;
    }

    public void SetRotateOnNormals(bool value)
    {
        rotateOnNormals = value;
    }

    public void KeepUpright(bool value)
    {
        keepUpright = value;
    }

    public void SetFaceCamera(bool value)
    {
        faceCamera = value;
        FaceCamera();
    }

    void FaceCamera()
    {
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        // save instance
        anchorStore = store;

        // load saved anchor
        if (!anchorStore.Load(anchorStoreName, trans.gameObject))
        {
            // if no saved anchor then create one
            WorldAnchor attachingAnchor = trans.gameObject.AddComponent<WorldAnchor>();
            if (attachingAnchor.isLocated)
            {
                anchorStore.Save(anchorStoreName, attachingAnchor);
            }
            else
            {
                attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, layerMask))
            {
                trans.position = hitInfo.point;

                // TO TRY: if rotateOnNormals do from to rotation on normal
                // then if keep upright to from to rotation on transform.up to vector3.up

                // if rotateOnNormals is true turn object to hug surfaces
                if (rotateOnNormals)
                {
                    trans.rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
                    if (keepUpright)
                    {
                        //clamp the x axis to prevent weird rotations
                        transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
                        //Vector3 newRotation = transform.rotation.eulerAngles;
                        //newRotation.x = -newRotation.x;
                        //newRotation.y = 0f;
                        //newRotation.z = -newRotation.z;
                        //transform.Rotate(newRotation);
                    }  
                }
                if (faceCamera)
                {
                    FaceCamera();
                }
            }
        }
    }

    void OnSelect()
    {
        if (anchorStore == null)
        {
            return;
        }

        if (Placing)
        {
            WorldAnchor attachingAnchor = trans.gameObject.AddComponent<WorldAnchor>();
            if (attachingAnchor.isLocated)
            {
                anchorStore.Save(anchorStoreName, attachingAnchor);
            }
            else
            {
                attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
            }
        }
        else
        {
            WorldAnchor anchor = trans.gameObject.GetComponent<WorldAnchor>();
            if (anchor != null)
            {
                DestroyImmediate(anchor);
            }
            anchorStore.Delete(anchorStoreName);
        }

        Placing = !Placing;
    }

    void OnRemove()
    {
        GameObject.Find("WorldManager").GetComponent<WorldManager>().Removing(this);

        WorldAnchor anchor = trans.gameObject.GetComponent<WorldAnchor>();
        if (anchor != null)
        {
            DestroyImmediate(anchor);
        }
        anchorStore.Delete(anchorStoreName);
        Destroy(gameObject);
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            anchorStore.Save(anchorStoreName, self);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}