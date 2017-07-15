// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.SpatialMapping;

public class RoomSaver : MonoBehaviour {

    public string fileName= "ARZArena";             // name of file to store meshes
    public string anchorStoreName= "ARZRoomMesh";      // name of world anchor to store for room

    List<MeshFilter> roomMeshFilters;
    WorldAnchorStore anchorStore;
    int meshCount = 0;

    // Use this for initialization
    void Start()
    {
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
    }

    public void SaveRoom()
    {
        // if the anchor store is not ready then we cannot save the room mesh
        if (anchorStore == null)
            return;

        // delete old relevant anchors
        string[] anchorIds = anchorStore.GetAllIds();
        for (int i = 0; i < anchorIds.Length; i++)
        {
            if (anchorIds[i].Contains(anchorStoreName))
            {
                anchorStore.Delete(anchorIds[i]);
            }
        }
        // get all mesh filters used for spatial mapping meshes
        roomMeshFilters = SpatialMappingManager.Instance.GetMeshFilters() as List<MeshFilter>;
        
        // create new list of room meshes for serialization
        List<Mesh> roomMeshes = new List<Mesh>();

        // cycle through all room mesh filters
        foreach (MeshFilter filter in roomMeshFilters)
        {
            // increase count of meshes in room
            meshCount++;

            // make mesh name = anchor name + mesh count
            string meshName = anchorStoreName + meshCount.ToString();
            filter.mesh.name = meshName;

            // add mesh to room meshes for serialization
            roomMeshes.Add(filter.mesh);

            // save world anchor
            WorldAnchor attachingAnchor = filter.gameObject.GetComponent<WorldAnchor>();
            if (attachingAnchor == null)
            {
                attachingAnchor = filter.gameObject.AddComponent<WorldAnchor>();
            }
            else
            {
                DestroyImmediate(attachingAnchor);
                attachingAnchor = filter.gameObject.AddComponent<WorldAnchor>();
            }
            if (attachingAnchor.isLocated)
            {
                if (!anchorStore.Save(meshName, attachingAnchor))
                    Debug.Log("" + meshName + ": Anchor save failed...");
                else
                    Debug.Log("" + meshName + ": Anchor SAVED...");
            }
            else
            {
                attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
            }
        }

        // serialize and save meshes
        MeshSaverOld.Save(fileName, roomMeshes);
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            string meshName = self.gameObject.GetComponent<MeshFilter>().mesh.name;
            if (!anchorStore.Save(meshName, self))
                Debug.Log("" + meshName + ": Anchor save failed...");
            else
                Debug.Log("" + meshName + ": Anchor SAVED...");

            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}
