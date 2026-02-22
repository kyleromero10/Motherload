using Godot;

public partial class ControlsMenu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("ReturnButton").Pressed += () =>
		{
			AudioManager.I?.PlayUiClick();
			GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
		};
	}
}
