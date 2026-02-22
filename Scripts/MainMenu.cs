using Godot;

public partial class MainMenu : Control
{
	[Export] public PackedScene ControlsMenuScene { get; set; }

	public override void _Ready()
	{
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

		GetTree().ChangeSceneToPacked(ControlsMenuScene);
	}
}
