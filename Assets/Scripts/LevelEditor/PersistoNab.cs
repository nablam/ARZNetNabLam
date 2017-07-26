// Modified from Persistomatic class as posted by @Patrick from https://forums.hololens.com/discussion/514/creating-and-assigning-spatial-anchors
// Modified by Nabil Lamriben

using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class PersistoNab : MonoBehaviour
{
 
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
     
    public string GetBaseName() { return anchorNameInStore; }
    WorldAnchorStore anchorStore;

    bool Placing = false;
    string anchorNameInStore="";
 

    Transform trans;
    // Use this for initialization
    void Awake()
    { 
       trans = transform;
    }
  

    public void SetAnchorStoreName(string argIDpassed)
    {
      //  Debug.Log("setting name of persisionab to " + argIDpassed);
        anchorNameInStore = argIDpassed;
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }
 

  
    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;

        // load saved anchor
        if (!anchorStore.Load(anchorNameInStore, trans.gameObject))
        {
            // if no saved anchor then create one
            WorldAnchor attachingAnchor = trans.gameObject.AddComponent<WorldAnchor>();
            if (attachingAnchor.isLocated)
            {
                anchorStore.Save(anchorNameInStore, attachingAnchor);
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
        DoPlaceHere();
    }

    void DoPlaceHere()
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
                anchorStore.Save(anchorNameInStore, attachingAnchor);
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
            anchorStore.Delete(anchorNameInStore);
        }

        Placing = !Placing;
    }

    void OnRemove()
    {
        DictoPlacedObjects.Instance.Removing(this);

        WorldAnchor anchor = trans.gameObject.GetComponent<WorldAnchor>();
        if (anchor != null)
        {
            DestroyImmediate(anchor);
        }
        anchorStore.Delete(anchorNameInStore);
        Destroy(gameObject);
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            anchorStore.Save(anchorNameInStore, self);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}