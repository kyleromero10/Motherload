using Godot;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

public partial class GoldVein : StaticBody2D
{
    public bool explode = false;
    [Export] public bool isGoal = false;
    [Export] public GameMaster gameMaster;
    [Signal] public delegate void LevelEndEventHandler();

    public override void _Ready()
    {
        base._Ready();
        gameMaster = GetNode<GameMaster>("/root/GameMaster");
    }

    public async void Explode()
    {
        // Mark that it has been lit and change its texture
        explode = true;
        GetChild<Sprite2D>(1).Visible = false;

        if(isGoal)
        {
            EmitSignal("LevelEnd");
        }
        else
        {
            gameMaster.goldCollected++;
            GD.Print("Gold collected: " + gameMaster.goldCollected);
            // wait to explode
            await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
            QueueFree();
        }
    }





}
