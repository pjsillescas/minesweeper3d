using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[SerializeField]
	private float comboWindow = 0.15f;

	private InputActions actions;
	private float leftPressedTime = -1f;
	private float rightPressedTime = -1f;

	private void Awake()
	{
		actions = new InputActions();
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

	}

	// Update is called once per frame
	void Update()
	{
		var isLeftButtonPressed = actions.Player.Interact.WasPressedThisFrame();
		var isRightButtonPressed = actions.Player.Mark.WasPressedThisFrame();

		if (isLeftButtonPressed)
		{
			leftPressedTime = Time.time;

			if (Time.time - rightPressedTime <= comboWindow)
			{
				OnBothButtonsPressed();
			}
			else
			{
				Invoke(nameof(OnLeftOnly), comboWindow);
			}
		}

		if (isRightButtonPressed)
		{
			rightPressedTime = Time.time;

			if (Time.time - leftPressedTime <= comboWindow)
			{
				OnBothButtonsPressed();
			}
			else
			{
				Invoke(nameof(OnRightOnly), comboWindow);
			}
		}
	}

	void OnBothButtonsPressed()
	{
		CancelInvoke();

		Debug.Log("Both buttons!");
	}

	void OnLeftOnly()
	{
		// Prevent false trigger if right was pressed shortly after
		if (Time.time - rightPressedTime > comboWindow)
		{
			Debug.Log("Left only");
		}
	}

	void OnRightOnly()
	{
		if (Time.time - leftPressedTime > comboWindow)
		{
			Debug.Log("Right only");
		}
	}
}
