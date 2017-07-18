using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransData  {
    Vector3 Pos;
    Quaternion Quat;
    string Id;
    public TransData() { }
    public TransData(Transform argTrans, string argID) {
        Pos = argTrans.position;
        Quat = argTrans.rotation;
        Id = argID;
    }
    public string GetID() { return this.Id; }
    public Vector3 Getpos() { return this.Pos; }
    public Quaternion GetRot() { return this.Quat; }

}
