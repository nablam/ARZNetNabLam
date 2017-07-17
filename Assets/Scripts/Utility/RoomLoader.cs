// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.SpatialMapping;

public class RoomLoader : MonoBehaviour {

    public GameObject managerObject;            // the room manager for this scene
    public GameObject surfaceObject;            // prefab for surface mesh objects
    public string fileName= "ARZArena";         // name of file used to store mesh
    public string anchorStoreName="ARZRoomMesh";// name of world anchor for room

    WorldAnchorStore anchorStore;               // store of world anchors
    List<Mesh> roomMeshes;                      // list of room meshes
    List<GameObject> roomObjects;               // list of game objects that hold room meshes

    // Use this for initialization
    bool _meshLoaded;
	void Start () {
        _meshLoaded = false;
        Debug.Log("load meshes async");
        // get instance of WorldAnchorStore     
    }
    public void LoadMeshed() {
        if (!_meshLoaded)
        {
            WorldAnchorStore.GetAsync(AnchorStoreReady);
        }
        else
        {
            Debug.Log("meshes already loaded");
        }


    }

    public void ToggleRoom()
    {
        if (_meshLoaded)
        {
            foreach (GameObject obj in roomObjects)
            {
                if (obj.activeInHierarchy)
                    obj.SetActive(false);
                else
                    obj.SetActive(true);
            }
        }

    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        // save instance
        anchorStore = store;

        // load room meshesn
        roomMeshes = MeshSaverOld.Load(fileName) as List<Mesh>;
        roomObjects = new List<GameObject>();

        foreach (Mesh surface in roomMeshes)
        {
            GameObject obj = Instantiate(surfaceObject) as GameObject;
            obj.GetComponent<MeshFilter>().mesh = surface;
            obj.GetComponent<MeshCollider>().sharedMesh = surface;
            roomObjects.Add(obj);

             if (!anchorStore.Load(surface.name, obj)) Debug.Log("WorldAnchor load failed...");
        }

        _meshLoaded = true;
        if (managerObject != null)
        {
            // managerObject.SendMessage("RoomLoaded");
            Debug.Log("roomloader has a manager named "+ managerObject.name);
        }
        else
        {
            Debug.Log("NO MANAGER for roomloader");
        }
    }

    void OnDestroy()
    {
        if (_meshLoaded)
        {
            foreach (Mesh mesh in roomMeshes)
            {
                Destroy(mesh);
            }
            roomMeshes.Clear();
        }
    }
}
