using System;
using TMPro;
using UnityEngine;

public class MinesWidget : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI MinesText;

	private void OnEnable()
	{
		BoardManager.OnMinesChanged += OnMinesChanged;
	}

	private void OnDisable()
	{
		BoardManager.OnMinesChanged -= OnMinesChanged;
	}

	private void OnMinesChanged(object sender, int numMines)
	{
		MinesText.text = numMines.ToString();
	}
}
