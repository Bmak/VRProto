using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public float Time = 0.05f;
	
	private float timer;
	
	void Start ()
	{
		
	}
	
	IEnumerator Flicker()
	{
		while(true)
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			
			do
			{
				timer -= UnityEngine.Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			yield break;
		}
	}

	public void Play()
	{
		timer = Time;
		StartCoroutine("Flicker");
	}
}
