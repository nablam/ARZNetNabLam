using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorme : MonoBehaviour {

 
 
 
    MeshRenderer _meshRenderer;
    bool tog;
    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
  
    }
    void Start()
    {
        tog = false;
        _meshRenderer.material.color = Color.white;
    }

      void ColorRed()
    {
        _meshRenderer.material.color = Color.red;
    }

      void ColorWhie()
    {
        _meshRenderer.material.color = Color.white;
    }

    public void DOToggoleColor() {
        if (!tog)
        {
            ColorRed();
        }
        else {
            ColorWhie();
        }
        tog = !tog;

    }
}
