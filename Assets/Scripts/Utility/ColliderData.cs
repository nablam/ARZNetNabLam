using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderData   {


    public Vector3 BC_center;
    public Vector3 BC_size;
    public string BC_Tag;
    public string BC_Layer;
    public string BC_BoneName;

    public ColliderData(Vector3 argCenter, Vector3 argSize, string argTag, string argLayer, string argBoneName) {
        BC_center = argCenter; BC_size = argSize; BC_Tag = argTag; BC_Layer = argLayer; BC_BoneName = argBoneName;
    }

    public ColliderData FlipLeft() {

        BC_center = new Vector3(BC_center.x * -1f, BC_center.y * -1f, BC_center.z);
        return this;
    }

    public override string ToString()
    {

        return BC_BoneName+"|" + BC_center.ToString("F2") + "|" + BC_size.ToString("F2") + "|" + BC_Tag + "|" + BC_Layer;
    }



}
