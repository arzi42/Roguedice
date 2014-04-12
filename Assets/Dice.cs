using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour 
{
	/*
	 * 1 = -90 0 0  
	 * 2 = 0 0 180
	 * 3 = 0 0 -90
	 * 4 = 0 0 90
	 * 5 = 0 0 0
	 * 6 = 90 0 0 
	 */ 

	public AnimationCurve attackCurve;

	private int[][] rotationToResult = new int[][] 
	{
		new int[] { -90, 0 }, 
		new int[] { 0, 180 },
		new int[] { 0, -90 },
		new int[] { 0, 90 },
		new int[] { 0, 0 },
		new int[] { 90, 0 }

	};

	private Transform camTransform;

	private Quaternion direction = Quaternion.identity;

	private bool isTurning;
	private bool isMoving;

	private int currentPower;

	// Use this for initialization
	void Awake () 
	{
		Debug.Log("dice awake");

		camTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		camTransform.position = transform.position + direction * new Vector3(0, 2.5f, -4);

		if(!isTurning && Input.GetKeyDown(KeyCode.LeftArrow))
		{
			StartCoroutine(LerpCamera(new Vector3(0, 90, 0)));
		}

		if(!isTurning && Input.GetKeyDown(KeyCode.RightArrow))
		{
			StartCoroutine(LerpCamera(new Vector3(0, -90, 0)));
		}

		if(!isMoving && Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 moveDirection = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;

			Vector3 targetPosition = transform.position + moveDirection;

			int x = Mathf.RoundToInt(targetPosition.x);
			int y = Mathf.RoundToInt(targetPosition.z);

			Enemy enemy = Map.instance.GetEnemiesAt(x, y);

			if(enemy == null)
			{
				UI.instance.Moved();

				StartCoroutine(LerpToRotation(new Vector3(moveDirection.z * 90, 0, moveDirection.x * -90), moveDirection));
			}
			else
			{


				StartCoroutine(Attack(enemy));
			}
		}
	}

	private IEnumerator Attack(Enemy target)
	{
		Vector3 position = transform.position;

		isMoving = true;

		bool attacked = false;

		for(float t = 0; t < 1f; t+= Time.deltaTime * 2f)
		{
			position.y = attackCurve.Evaluate(t) + 0.5f;

			transform.position = position;

			if(!attacked && t > 0.5f)
			{
				if(currentPower > target.power)
				{
					target.Hit();
				}

				attacked = true;
			}

			yield return null;
		}

		position.y = 0.5f;

		isMoving = false;

		transform.position = position;
	}

	private IEnumerator LerpCamera(Vector3 angles)
	{
		Quaternion start = direction;
		Quaternion end = Quaternion.Euler(direction.eulerAngles + angles);
		Quaternion tStart = camTransform.rotation;
		Quaternion tEnd = Quaternion.Euler(camTransform.rotation.eulerAngles + angles);

		isTurning = true;

		for(float t = 0; t < 1f; t +=  Time.deltaTime * 4f)
		{
			direction = Quaternion.Lerp(start, end, t);

			camTransform.rotation = Quaternion.Lerp(tStart, tEnd, t);

			yield return null;
		}

		direction = end;
		camTransform.rotation = tEnd;

		isTurning = false;
	}

	private IEnumerator LerpToRotation(Vector3 angles, Vector3 moveDirection)
	{
		Quaternion start = transform.rotation;
		Quaternion end = Quaternion.Euler(angles) * start;

		Vector3 startPos = transform.position;
		Vector3 endPos = transform.position + moveDirection;

		isMoving = true;

		for(float t = 0; t < 1f; t +=  Time.deltaTime * 2f)
		{
			transform.rotation = Quaternion.Lerp(start, end, t);
			transform.position = Vector3.Lerp(startPos, endPos, t);
			yield return null;
		}

		transform.rotation = end;
		transform.position = endPos;

		int x = Mathf.RoundToInt(transform.rotation.eulerAngles.x);
		int z = Mathf.RoundToInt(transform.rotation.eulerAngles.z);

		if(x == 270) x = -90;

		currentPower = GetResult(x, z);

		//Debug.Log("x = " + x + " z = " + z + " result = " + );

		isMoving = false;
	}

	private int GetResult(int x, int z)
	{
		for(int i = 0; i < rotationToResult.Length; i++)
		{
			if(x == rotationToResult[i][0] && z == rotationToResult[i][1])
			{
				return i +1;
			}
		}

		return 0;
	}

}
