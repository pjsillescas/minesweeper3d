using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public static event EventHandler<int> OnMinesChanged;

	// Expert 16x30
	// Intermediate 16x16
	// Beginner 9x9

	[SerializeField]
	private LayerMask cellLayer;


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

	//private List<Cell> cells;
	private Cell[,] cells;
	private int numMines;
	private bool isInitialized;
	private int numRows;
	private int numColumns;

	public void BuildBoard(int rows, int columns, float cellWidth, int numMines)
	{
		numRows = rows;
		numColumns = columns;

		this.numMines = numMines;
		OnMinesChanged?.Invoke(this, numMines);

		// cells = new();
		cells = new Cell[rows, columns];
		
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				var x = (i - rows / 2) * cellWidth;
				var z = (j - columns / 2) * cellWidth;
				var cell = Instantiate(HiddenCellPrefab, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Cell>();
				cell.Init(i, j);
				//cells.Add(cell);
				cells[i, j] = cell;
			}
		}
	}

	class Position
	{
		public int i;
		public int j;

		public override bool Equals(object p)
		{
			Position p1 = p as Position;
			return i == p1.i && j == p1.j;
		}

		public override int GetHashCode()
		{
			return i + j;
		}
	}
	
	private List<Position> SetMines(int i, int j)
	{
		//int k = i * numRows + j;
		var p0 = new Position() { i = i, j = j };
		var mines = new List<Position>();
		mines.Add(new Position() { i = 0, j = 0 });

		for (int n = 0; n < numMines; n++)
		{
			Position p;
			do
			{
				p = new Position() { i = UnityEngine.Random.Range(0, numRows - 1), j = UnityEngine.Random.Range(0, numColumns - 1) };
			}
			while (p0.Equals(p) || mines.Contains(p));

			mines.Add(p);
		}

		return mines;
	}

	private int GetSurroundingMinesNumber(Cell cell)
	{
		var num = 0;
		//int j = index % numRows;
		//int i = (index - j) / numRows;
		var i = cell.GetRow();
		var j = cell.GetColumn();

		for (int r = -1; r <= 1; r++)
		{
			var row = i + r;
			for (int c = -1; c <= 1; c++)
			{
				var col = j + c;
				//var index = (i + r) * numRows + j + c;
				//if (cells[index].GetValue() == Cell.CellValue.MINE)
				if (0 <= row && row < numRows && 0 <= col && col < numColumns &&
					cells[row,col].GetValue() == Cell.CellValue.MINE)
				{
					num++;
				}
			}
		}


		return num;
	}

	private void Init(int i, int j)
	{
		var mines = SetMines(i, j);

		mines.ForEach(index =>
		{
			Debug.Log($"mine ({index.i},{index.j})");
			//cells[index].SetValue(Cell.CellValue.MINE);
			cells[index.i, index.j].SetValue(Cell.CellValue.MINE);

			//int j = index % numRows;
			//int i = (index - j) / numRows;
		});

		/*
		cells.ForEach(cell =>
		{
			if (cell.GetValue() != Cell.CellValue.MINE)
			{
				cell.SetValue(GetSurroundingMinesNumber(cell));
			}
		});
		*/
		for(int r=0;r<numRows;r++)
		{
			for(int c=0;c<numColumns;c++)
			{
				var cell = cells[r, c];
				if (cell.GetValue() != Cell.CellValue.MINE)
				{
					var n = GetSurroundingMinesNumber(cell);
					Debug.Log($"({i},{j})={n}");
					cell.SetValue(GetSurroundingMinesNumber(cell));
				}
			}
		}

	}

	private Cell GetClickedCell(Vector2 mousePosition)
	{
		var ray = FindAnyObjectByType<Camera>().ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 50f, cellLayer) && hit.collider.TryGetComponent(out Cell cell))
		{
			return cell;
		}

		return null;
	}

	public void TryMarkCell(Vector2 mousePosition)
	{
		var cell = GetClickedCell(mousePosition);
		if (cell != null)
		{
			if (numMines > 0 && !cell.GetIsMarked())
			{
				cell.ToggleMark();
				numMines--;
			}
			else if (cell.GetIsMarked())
			{
				cell.ToggleMark();
				numMines++;
			}
		}
	}

	public void TryClickCell(Vector2 mousePosition)
	{
		var cell = GetClickedCell(mousePosition);
		if (cell != null)
		{
			if (!isInitialized)
			{
				isInitialized = true;
				Init(cell.GetRow(), cell.GetColumn());
			}

			Debug.Log($"value {cell.GetValue()}");
			var value = cell.Uncover();

			GameObject prefab = value switch
			{
				Cell.CellValue.V1 => Cell1Prefab,
				Cell.CellValue.V2 => Cell2Prefab,
				Cell.CellValue.V3 => Cell3Prefab,
				Cell.CellValue.V4 => Cell4Prefab,
				Cell.CellValue.V5 => Cell5Prefab,
				Cell.CellValue.V6 => Cell6Prefab,
				Cell.CellValue.V7 => Cell7Prefab,
				Cell.CellValue.V8 => Cell8Prefab,
				Cell.CellValue.MINE => MinePrefab,
				_ => null,
			};

			if (prefab != null)
			{
				//Instantiate(prefab, cell.transform.position, Quaternion.identity);
				//Instantiate(prefab, cell.transform.position, new Quaternion(90,0,180,0));
				Instantiate(prefab, cell.transform.position, cell.transform.rotation).transform.Rotate(90,0,90);
			}
		}
	}

	private void OnEnable()
	{
		InputManager.OnClick += OnClick;
		InputManager.OnMark += OnMark;
		InputManager.OnChordStart += OnChordStart;
		InputManager.OnChordEnd += OnChordEnd;
	}

	private void OnDisable()
	{
		InputManager.OnClick -= OnClick;
		InputManager.OnMark -= OnMark;
		InputManager.OnChordStart -= OnChordStart;
		InputManager.OnChordEnd -= OnChordEnd;
	}

	private void OnClick(object sender, Vector2 mousePosition)
	{
		TryClickCell(mousePosition);
	}

	private void OnMark(object sender, Vector2 mousePosition)
	{
		TryMarkCell(mousePosition);
	}
	private void OnChordStart(object sender, Vector2 mousePosition)
	{
		;
	}
	private void OnChordEnd(object sender, Vector2 mousePosition)
	{
		;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//BuildBoard(16, 30, 1f, 99);
		BuildBoard(5, 5, 1f, 5);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
