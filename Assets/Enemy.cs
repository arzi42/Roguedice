using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	[SerializeField]
	private int _power;

	private TextMesh powerText;

	public int power
	{
		get
		{
			return _power;
		}
		set
		{
			_power = value;

			UpdatePowerText();
		}
	}

	private void UpdatePowerText()
	{
		powerText.text = _power.ToString();
	}

	void Awake()
	{
		powerText = GetComponentInChildren<TextMesh>();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
