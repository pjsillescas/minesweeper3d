using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public static event EventHandler<Vector2> OnClick;
	public static event EventHandler<Vector2> OnMark;
	public static event EventHandler<Vector2> OnChordStart;
	public static event EventHandler<Vector2> OnChordEnd;

	[SerializeField]
	private float comboWindow = 0.15f;

	private InputActions actions;
	private float leftPressedTime;
	private float rightPressedTime;
	private bool isChord;

	private void Awake()
	{
		actions = new InputActions();
		leftPressedTime = -1f;
		rightPressedTime = -1f;
		isChord = false;
	}

	private void OnEnable()
	{
		actions.Enable();
	}

	private void OnDisable()
	{
		actions.Disable();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		;
	}

	// Update is called once per frame
	void Update()
	{
		var isLeftButtonPressed = actions.Player.Interact.IsPressed();
		var isRightButtonPressed = actions.Player.Mark.IsPressed();

		var isLeftButtonPressedNow = actions.Player.Interact.WasPressedThisFrame();
		var isRightButtonPressedNow = actions.Player.Mark.WasPressedThisFrame();

		if (isLeftButtonPressedNow && !isChord)
		{
			leftPressedTime = Time.time;

			if (Time.time - rightPressedTime <= comboWindow)
			{
				OnChordStartMethod();
				isChord = true;
			}
			else
			{
				Invoke(nameof(OnLeftClick), comboWindow);
			}
		}

		if (isRightButtonPressedNow && !isChord)
		{
			rightPressedTime = Time.time;

			if (Time.time - leftPressedTime <= comboWindow)
			{
				OnChordStartMethod();
				isChord = true;
			}
			else
			{
				Invoke(nameof(OnRightClick), comboWindow);
			}
		}

		if (isChord && !isLeftButtonPressed && !isRightButtonPressed)
		{
			isChord = false;
			OnChordEndMethod();
		}
	}

	private void OnChordStartMethod()
	{
		CancelInvoke();

		//Debug.Log("Both buttons!");
		OnChordStart?.Invoke(this, Mouse.current.position.value);
	}

	private void OnChordEndMethod()
	{
		//Debug.Log("Chord end");
		OnChordEnd?.Invoke(this, Mouse.current.position.value);
	}

	private void OnLeftClick()
	{
		// Prevent false trigger if right was pressed shortly after
		if (Time.time - rightPressedTime > comboWindow)
		{
			//Debug.Log("Left only");
			OnClick?.Invoke(this, Mouse.current.position.value);
		}
	}

	private void OnRightClick()
	{
		if (Time.time - leftPressedTime > comboWindow)
		{
			// Debug.Log("Right only");
			OnMark?.Invoke(this, Mouse.current.position.value);
		}
	}
}
