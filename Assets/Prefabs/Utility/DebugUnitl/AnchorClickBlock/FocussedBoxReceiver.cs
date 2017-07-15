using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class FocussedBoxReceiver : MonoBehaviour, IFocusable, IInputClickHandler
{
    public void OnFocusEnter()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void OnFocusExit()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

  

    public void OnInputClicked(InputClickedEventData eventData)
    {
        SendMessageUpwards("CallScreen", this.gameObject.name);
    }
}
