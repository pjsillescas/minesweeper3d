using System;
using TMPro;
using UnityEngine;

public class TimeWidget : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI TimeText;

	private bool isGameEnabled;
	private float time;

	public float GetTime() => time;

	private void OnEnable()
	{
		BoardManager.OnStartGame += OnStartGame;
		BoardManager.OnEndGame += OnEndGame;
	}
	private void OnDisable()
	{
		BoardManager.OnStartGame -= OnStartGame;
		BoardManager.OnEndGame -= OnEndGame;
	}

	private void OnStartGame(object sender, EventArgs args)
	{
		time = 0;
		Enable();
	}

	private void OnEndGame(object sender, EventArgs args)
	{
		Disable();
	}
	
	private void Awake()
	{
		isGameEnabled = false;
		time = 0;
	}

	public void Disable()
	{
		isGameEnabled = false;
	}
	public void Enable()
	{
		isGameEnabled = true;
	}

	private void Update()
	{
		if (!isGameEnabled)
		{
			return;
		}

		time += Time.deltaTime;

		TimeText.text = Mathf.Floor(time).ToString();
	}
}
