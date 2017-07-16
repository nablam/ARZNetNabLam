using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.Events;


public class GenericClicker  : MonoBehaviour, IFocusable, IInputClickHandler
{
   
    public UnityEvent OnSelectEvents;
    public string ClickerText;
    TextMesh _textMesh;
    MeshRenderer _meshRenderer;
    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _textMesh = GetComponentInChildren<TextMesh>();
    }
    void Start()
    {
        Debug.Log("on-->" + gameObject.name);
        _meshRenderer.material.color = Color.white;
        if (_textMesh != null) {
            _textMesh.text = ClickerText;
           
        }
    }

    public void OnFocusEnter()
    {
        _meshRenderer.material.color = Color.red;
    }

    public void OnFocusExit()
    {
        _meshRenderer.material.color = Color.white;
    }



    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (OnSelectEvents != null)
        {
            OnSelectEvents.Invoke();
        }
        else {
            _textMesh.color = Color.red;
        }
    }

   
}
