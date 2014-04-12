using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
	public static Map instance;

	public GameObject tile;

	public Enemy enemyPrefab;

	public Dice dice;

	private bool[,] map;

	public int width = 10;
	public int height = 10;

	private List<Enemy> enemies;

	public Enemy GetEnemiesAt(int x, int y)
	{
		foreach(Enemy enemy in enemies)
		{
			if(enemy != null)
			{
				int ex = Mathf.RoundToInt(enemy.transform.position.x);
				int ey = Mathf.RoundToInt(enemy.transform.position.z);

				if(ex == x && ey == y)
				{
					return enemy;
				}
			}
		}

		return null;
	}

	private void FindEnemies()
	{
		enemies = new List<Enemy>();

		enemies.AddRange(GameObject.FindObjectsOfType<Enemy>());
	}

	void Awake()
	{
		instance = this;

		FindEnemies();
	}

	// Use this for initialization
	void Start () 
	{
		map = new bool[width,height];

		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				map[x,y] = true;
				Instantiate(tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0));
			}
		}

		Vector3 enemyPosition = new Vector3(Random.Range(0, width), 0, Random.Range(height - 4, height));

		Enemy enemy = (Enemy) Instantiate(enemyPrefab, enemyPosition, enemyPrefab.transform.rotation);

		enemy.power = Random.Range(2, 6);

		Instantiate(dice, new Vector3(5, 0.5f, 5), Quaternion.identity);

		FindEnemies();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
