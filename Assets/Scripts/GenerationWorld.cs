using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationWorld : MonoBehaviour
{
	[Header("Terrain Generation")]
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] int minStoneHeight, maxStoneHeight;
	[SerializeField] float smoothness;
	[SerializeField] int seed;

	[Header("Cave Generation")]
	[Range(0, 1)]
	[SerializeField] float modifer;

	// Tile Maps
	[Header("Tile Maps")]
	[SerializeField] Tilemap dirtTilemap;
	[SerializeField] Tilemap grassTilemap;
	[SerializeField] Tilemap stoneTilemap;
	[SerializeField] Tilemap caveTilemap;

	// Tiles
	[Header("Tiles")]
	[SerializeField] Tile dirt;
	[SerializeField] Tile grass;
	[SerializeField] Tile stone;
	[SerializeField] Tile backgroundStone;

	private int[,] map;


	void Start()
	{
		this.seed = Random.Range(-1000000, 1000000);

		map = GenerateArray(width, height, true);
		map = GenerateTerrain(map);

		RenderMap(map);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			seed = Random.Range(-1000000, 1000000);
			map = GenerateArray(width, height, true);
			map = GenerateTerrain(map);
			map = GenerateCaves(map);

			RenderMap(map);
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			stoneTilemap.ClearAllTiles();
			dirtTilemap.ClearAllTiles();
			grassTilemap.ClearAllTiles();
		}
	}

	public int[,] GenerateArray(int width, int height, bool empty)
	{
		int[,] map = new int[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				map[x, y] = (empty) ? 0 : 1;
			}
		}

		return map;
	}

	public int[,] GenerateTerrain(int[,] map)
	{
		int perlinHeight;

		for (int x = 0; x < this.width; x++)
		{
			perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / this.smoothness, seed) * this.height / 2);
			perlinHeight += this.height / 2;
			int minStoneSpawnDistance = perlinHeight - minStoneHeight;
			int maxStoneSpawnDistance = perlinHeight - maxStoneHeight;
			int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

			for (int y = 0; y < perlinHeight; y++)
			{
				if (y < totalStoneSpawnDistance)
				{
					map[x, y] = 2;
				}
				else if (y == perlinHeight - 1)
				{
					map[x, y] = 3;
				}
				else
				{
					map[x, y] = 1;
				}
			}
		}

		return map;
	}

	public int[,] GenerateCaves(int[,] map)
	{
		int perlinHeight;

		for (int x = 0; x < this.width; x++)
		{
			int minStoneSpawnDistance = height - minStoneHeight;
			int maxStoneSpawnDistance = height - maxStoneHeight;
			int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);
			perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / this.smoothness, seed) * (totalStoneSpawnDistance));
			//perlinHeight += this.height / 2;

			for (int y = 0; y < perlinHeight; y++)
			{
				//int caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x * modifer) + seed, (y * modifer) + seed));
				int caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x * modifer) + seed, (y * modifer) + seed));
				
				map[x, y] = (caveValue == 1) ? 2 : 4;
			}
		}

		return map;
	}

	public void RenderMap(int[,] map)
	{
		for (int x = 0; x < this.width; x++)
		{
			for (int y = 0; y < this.height; y++)
			{
				if (map[x, y] == 1)
				{
					this.dirtTilemap.SetTile(new Vector3Int(x, y, 0), this.dirt);
				}
				else if (map[x, y] == 2)
				{
					this.stoneTilemap.SetTile(new Vector3Int(x, y, 0), this.stone);
				}
				else if (map[x, y] == 3)
				{
					this.grassTilemap.SetTile(new Vector3Int(x, y, 0), this.grass);
				} else if (map[x,y] == 4) {
					this.caveTilemap.SetTile(new Vector3Int(x, y, 0), this.backgroundStone);
				}
			}
		}
	}

	/*void GenerateWorld()
	{
		for (int x = 0; x < width; x++)
		{
			int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));
			int minStoneSpawnDistance = height - minStoneHeight;
			int maxStoneSpawnDistance = height - maxStoneHeight;
			int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

			for (int y = 0; y < height; y++)
			{
				if (y < totalStoneSpawnDistance)
				{
					stoneTilemap.SetTile(new Vector3Int(x, y, 0), stone);
				}
				else
				{
					dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
				}

			}
			if (totalStoneSpawnDistance == height)
			{
				stoneTilemap.SetTile(new Vector3Int(x, height, 0), stone);
			}
			else
			{
				grassTilemap.SetTile(new Vector3Int(x, height, 0), grass);
			}

			// Generate Caves
			if (this.caveStartHeight < totalStoneSpawnDistance + 5)
			{
				int caveHeight = Mathf.RoundToInt(this.caveStartHeight * Mathf.PerlinNoise(x / smoothness, seed));

				Debug.Log(caveHeight);

				for (int y = 0; y < height - (Mathf.RoundToInt(this.caveStartHeight) * 2); y++)
				{
					stoneTilemap.SetTile(new Vector3Int(x, y, 0), testTile);
				}
			}
		}
	}*/
}
