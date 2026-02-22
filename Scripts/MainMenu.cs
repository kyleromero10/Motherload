using Godot;

public partial class MainMenu : Control
{
	[Export] public PackedScene ControlsMenuScene { get; set; }
	[Export] public PackedScene LevelScene { get; set; } 

	public override void _Ready()
	{
		AudioManager.I?.StopBgm();

		GetNode<Button>("ControlsButton").Pressed += OnControlsPressed;

		GetNode<Button>("StartButton").Pressed += OnStartPressed;
	}

	private void OnStartPressed()
	{
		if (LevelScene == null)
		{
			GD.PushError("LevelScene is not assigned in the Inspector!");
			return;
		}

		AudioManager.I?.PlayUiClick();
		GetTree().ChangeSceneToPacked(LevelScene);
	}

	private void OnControlsPressed()
	{
		if (ControlsMenuScene == null)
		{
			GD.PushError("ControlsMenuScene is not assigned in the Inspector!");
			return;
		}

		AudioManager.I?.PlayUiClick();
		GetTree().ChangeSceneToPacked(ControlsMenuScene);
	}
}
