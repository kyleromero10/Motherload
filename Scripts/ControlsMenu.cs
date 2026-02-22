using Godot;

public partial class ControlsMenu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("ReturnButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
		};
	}
}
