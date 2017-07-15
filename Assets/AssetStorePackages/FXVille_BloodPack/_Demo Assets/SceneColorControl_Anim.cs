using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SceneColorControl_Anim : MonoBehaviour 
{
	public Material	skyMaterial;
	public Color skyColor		= new Color(0.15f, 0.3f, 0.5f, 1f);
	public Color horizonColor	= new Color(0.7f, 0.85f, 1f, 1f);
	public Color groundColor	= new Color(0.4f, 0.35f, 0.3f, 1f);

	public float skyIntensity		 = 1.1f;
	public float skyFocus			 = 0.2f;
	public float horizonColorBanding = 0.25f;

	public bool customFogColor	= false;
	public Color fogColor		= new Color(0.7f, 0.85f, 1f, 1f);

	void Start() 
	{
		skyMaterial = RenderSettings.skybox;
	}
	
	void Update() 
	{
		UpdateColors();
	}

	void OnValidate()
	{
		UpdateColors();
	}

	void UpdateColors()
	{
		if(skyMaterial == null)
		{
			skyMaterial = RenderSettings.skybox;
		}
		skyMaterial.SetColor("_SkyColor", skyColor);
		skyMaterial.SetColor("_HorizonColor", horizonColor);
		skyMaterial.SetColor("_GroundColor", groundColor);
		skyMaterial.SetFloat("_SkyIntensity", skyIntensity);
		skyMaterial.SetFloat("_SunSkyFocus", skyFocus);
		skyMaterial.SetFloat("_HorizonBand", horizonColorBanding);

		Color averageAmbient = (skyColor * 1.4f + horizonColor * 0.8f + groundColor * 0.8f) * 0.33f;

		//set solo ambient color
		if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat)
		{
			RenderSettings.ambientSkyColor = averageAmbient * skyIntensity;
		}

		//set ambient gradient
		else 
		{
			RenderSettings.ambientSkyColor = (skyColor * 1.5f + horizonColor * 0.5f + averageAmbient) * 0.33f * skyIntensity;
			RenderSettings.ambientEquatorColor = (horizonColor + skyColor + groundColor + averageAmbient) * 0.25f * skyIntensity;
			RenderSettings.ambientGroundColor = (groundColor + horizonColor + averageAmbient) * 0.33f * skyIntensity;
		}

		//set fog
		if (customFogColor)
		{
			RenderSettings.fogColor = fogColor;
		}
		else
		{
			//fog color derived from sky and sun
			fogColor = (averageAmbient + horizonColor + groundColor) * 0.33f * skyIntensity;
			RenderSettings.fogColor = fogColor;
		}
	}
}
