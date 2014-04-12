using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	private Transform camTransform;

	// Use this for initialization
	void Awake () 
	{
		camTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = camTransform.rotation;
	}
}
