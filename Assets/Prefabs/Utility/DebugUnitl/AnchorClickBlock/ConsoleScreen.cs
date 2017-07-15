using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class ConsoleScreen : MonoBehaviour {

    public TextMesh tm;
    WorldAnchorStore anchorStore;
    void Start () {
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
    }

    public void ShowAllClicked()
    {
        tm.text += "\n showall";
        string[] ids = anchorStore.GetAllIds();
        string s1 = "found " + ids.Length + "anchors locally";
        tm.text = s1;
        for (int x = 0; x < ids.Length; x++)
        {
            tm.text += ids[x].ToString();
            tm.text += "\n";
        }
    }
    public void ClearAllClicked()
    {
        tm.text += "\n clearall";
        anchorStore.Clear();
    }
    public void CallScreen(object o)
    {
        if (o is string)
        {
            if (o.ToString() == "LocalAnchorsOBJ") { ShowAllClicked(); }
            else
                 if (o.ToString() == "DeleteAllLocalOBJ") { ClearAllClicked(); }

        }
    }
}
