using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPath  {

    private string _nameOfStartObject;

    public string NameOfStartObject
    {
        get { return _nameOfStartObject; }
        set { _nameOfStartObject = value; }
    }

    private string _nameOfEndObject;

    public string NameOfENDObject
    {
        get { return _nameOfEndObject; }
        set { _nameOfEndObject = value; }
    }

    List<Vector3> V3sOnPath;
    public DataPath() { V3sOnPath = new List<Vector3>(); }
    public DataPath(List<Vector3> argV3sOnPath) { V3sOnPath = argV3sOnPath; V3sOnPath = argV3sOnPath; }
    public DataPath(List<Vector3> argV3sOnPath, string argStartName, string argEndName) { V3sOnPath = argV3sOnPath; _nameOfStartObject = argStartName; _nameOfEndObject = argEndName; V3sOnPath = argV3sOnPath; }

    public List<Vector3> GEtTheListOfV3() { return this.V3sOnPath; }
    public void SetListOfV3s(List<Vector3> arglist) { V3sOnPath = arglist; }
}
