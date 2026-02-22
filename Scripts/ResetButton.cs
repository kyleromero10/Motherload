using Godot;
using System;

public partial class ResetButton : Button
{
    public void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/test_scene.tscn");
    }
}
