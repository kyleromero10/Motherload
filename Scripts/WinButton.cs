using Godot;
using System;

public partial class WinButton : Button
{
    public void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }
}
