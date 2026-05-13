using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	// Expert 16x30
	// Intermediate 16x16
	// Beginner 9x9

	[SerializeField]
	private GameObject HiddenCellPrefab;

	[SerializeField]
	private GameObject Cell1Prefab;
	[SerializeField]
	private GameObject Cell2Prefab;
	[SerializeField]
	private GameObject Cell3Prefab;
	[SerializeField]
	private GameObject Cell4Prefab;
	[SerializeField]
	private GameObject Cell5Prefab;
	[SerializeField]
	private GameObject Cell6Prefab;
	[SerializeField]
	private GameObject Cell7Prefab;
	[SerializeField]
	private GameObject Cell8Prefab;

	[SerializeField]
	private GameObject MinePrefab;

	private List<Cell> cells;
	public void BuildBoard(int rows, int columns, float cellWidth)
	{
		cells = new();
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				var x = (i - rows / 2) * cellWidth;
				var z = (j - columns / 2) * cellWidth;
				var cell = Instantiate(HiddenCellPrefab, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Cell>();
				cell.Init(i,j);
				cells.Add(cell);
			}
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		BuildBoard(16, 30, 1f);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
