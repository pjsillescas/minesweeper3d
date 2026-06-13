using System;
using System.Collections;
using UnityEngine;

public class EndGameComponent : MonoBehaviour
{
	private StartGameWidget startGameWidget;
	private BoardManager boardManager;
	private bool finishGame;

	private void OnEnable()
	{
		BoardManager.OnEndGame += OnEndGame;
	}

	private void OnDisable()
	{
		BoardManager.OnEndGame -= OnEndGame;
	}

	private void OnClick(object sender, Vector2 mousePosition)
	{
		finishGame = true;
	}

	private void OnEndGame(object sender, BoardManager.GameResult result)
	{
		if(result == BoardManager.GameResult.WON)
		{
			EndGame(result);
		}
		else
		{
			boardManager.RevealMines();
			StartCoroutine(WaitForUser(result));
		}
	}

	private void EndGame(BoardManager.GameResult result)
	{
		boardManager.ClearBoard();
		startGameWidget.Activate(result);
	}

	private IEnumerator WaitForUser(BoardManager.GameResult result)
	{
		InputManager.OnClick += OnClick;
		finishGame = false;
		while (!finishGame)
		{
			yield return null;
		}

		InputManager.OnClick -= OnClick;
		EndGame(result);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		startGameWidget = FindAnyObjectByType<StartGameWidget>();
		boardManager = FindAnyObjectByType<BoardManager>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
