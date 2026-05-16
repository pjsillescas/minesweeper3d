using UnityEngine;

public class Billboard : MonoBehaviour
{
	[SerializeField]
	private int value;

	private int row;
	private int column;

	public int GetValue() => value;

	public void SetCell(Cell cell)
	{
		row = cell.GetRow();
		column = cell.GetColumn();
	}

	public int GetRow() => row;
	public int GetColumn() => column;
}
