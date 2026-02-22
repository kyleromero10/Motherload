using Godot;

public partial class MainMenu : Control
{
	[Export] public PackedScene ControlsMenuScene { get; set; }

	public override void _Ready()
	{
		AudioManager.I?.StopBgm();
		var controlsButton = GetNode<Button>("ControlsButton");
		controlsButton.Pressed += OnControlsPressed;
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
