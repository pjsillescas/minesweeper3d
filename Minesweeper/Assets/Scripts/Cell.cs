using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
	public enum CellValue { NONE, MINE, V1, V2, V3, V4, V5, V6, V7, V8 }

	[SerializeField]
	private Material MarkedMaterial;
	[SerializeField]
	private Material UnmarkedMaterial;
	[SerializeField]
	private Material ChordedMaterial;
	
	private int i;
	private int j;
	private bool isMarked;
	private MeshRenderer meshRenderer;
	private CellValue value;

	public void SetValue(CellValue value)
	{
		this.value = value;
	}

	public CellValue GetValue() => value;


	public void SetValue(int value)
	{
		this.value = value switch
		{
			0 => CellValue.NONE,
			1 => CellValue.V1,
			2 => CellValue.V2,
			3 => CellValue.V3,
			4 => CellValue.V4,
			5 => CellValue.V5,
			6 => CellValue.V6,
			7 => CellValue.V7,
			8 => CellValue.V8,
			_ => CellValue.NONE,
		};
	}

	public void Chord()
	{
		if (isMarked)
		{
			return;
		}

		meshRenderer.material = ChordedMaterial;
	}

	public void Unchord()
	{
		if (isMarked)
		{
			return;
		}

		meshRenderer.material = UnmarkedMaterial;
	}

	public void Init(int i, int j)
	{
		this.i = i;
		this.j = j;
		isMarked = false;
		value = CellValue.NONE;
	}

	public int GetRow() => i;
	public int GetColumn() => j;

	public bool GetIsMarked() => isMarked;

	public bool ToggleMark()
	{
		isMarked = !isMarked;

		if (isMarked)
		{
			meshRenderer.material = MarkedMaterial;
			Debug.Log($"mark ({i},{j})");
		}
		else
		{
			meshRenderer.material = UnmarkedMaterial;
			Debug.Log($"unmark ({i},{j})");
		}

		return isMarked;
	}

	public CellValue Uncover()
	{
		Debug.Log($"uncover ({i},{j})");
		Destroy(gameObject, 0.01f);

		return value;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.material = UnmarkedMaterial;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
