using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour 
{
	private int moves = 0;

	public static UI instance;

	void Awake()
	{
		instance = this;
	}

	public void Moved()
	{
		moves ++;
	}

	void OnGUI()
	{
		GUI.Box(new Rect(10, 10, 70, 30), "");

		GUI.Label(new Rect(20, 15, 90, 20), "Moves: " + moves);
	}

}
