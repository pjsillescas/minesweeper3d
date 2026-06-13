using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameWidget : MonoBehaviour
{
	private const float CELL_WIDTH = 1f;

	private const int MAX_ROWS = 16;
	private const int MAX_COLS = 30;
	private const int MAX_MINES = 200;

	[SerializeField]
	private TMP_Dropdown OptionsDropdown;
	[SerializeField]
	private TextMeshProUGUI GameResultText;

	[SerializeField]
	private TMP_InputField NumRowsText;
	[SerializeField]
	private TMP_InputField NumColsText;
	[SerializeField]
	private TMP_InputField NumMinesText;

	[SerializeField]
	private Button StartButton;

	private BoardManager boardManager;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GameResultText.text = "";
		boardManager = FindAnyObjectByType<BoardManager>();

		StartButton.onClick.RemoveAllListeners();
		StartButton.onClick.AddListener(StartButtonClick);
		BoardManager.OnEndGame += OnEndGame;
	}

	private void OnEndGame(object sender, BoardManager.GameResult result)
	{
		//Activate(result);
	}

	public void Activate(BoardManager.GameResult result)
	{
		Debug.Log(result);
		GameResultText.text = (result == BoardManager.GameResult.WON) ? "You Won!" : "You lost!";
		gameObject.SetActive(true);
	}

	private int GetValue(TMP_InputField input, int maxValue)
	{
		return Mathf.Clamp(int.Parse(input.text), 0, maxValue);
	}

	private void StartButtonClick()
	{
		int rows;
		int cols;
		int mines;

		var value = OptionsDropdown.options[OptionsDropdown.value].text.ToUpper();

		if (value == "BEGINNER")
		{
			rows = 9;
			cols = 9;
			mines = 10;
		}
		else if (value == "INTERMEDIATE")
		{
			rows = 16;
			cols = 16;
			mines = 25;
		}
		else if (value == "EXPERT")
		{
			rows = 16;
			cols = 30;
			mines = 99;
		}
		else // CUSTOM
		{
			rows = GetValue(NumRowsText, MAX_ROWS);
			cols = GetValue(NumColsText, MAX_COLS);
			mines = GetValue(NumMinesText, MAX_MINES);
		}

		boardManager.BuildBoard(rows, cols, CELL_WIDTH, mines);
		gameObject.SetActive(false);
	}
}
