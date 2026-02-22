using Godot;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

public partial class GoldVein : StaticBody2D
{
	public bool exploded = false;
	[Export] public bool isGoal = false;
	[Export] public GameMaster gameMaster;
	[Export] public BuildManager buildManager;
	[Export] public TileMapLayer tileMap;
	public Vector2 snappedWorld;
	[Signal] public delegate void LevelEndEventHandler();

	public override void _Ready()
	{
		base._Ready();
		gameMaster = GetNode<GameMaster>("/root/Level/GameMaster");
		buildManager = GetNode<BuildManager>("/root/Level");
		tileMap = GetNode<TileMapLayer>("/root/Level/TileMapLayer");
		// gets its position and send it to the builders dictionary
		Vector2 localPos = tileMap.ToLocal(this.GlobalPosition);
		Vector2I cell = tileMap.LocalToMap(localPos);
		Vector2 snappedLocal = tileMap.MapToLocal(cell);
		snappedWorld = tileMap.ToGlobal(snappedLocal);
		buildManager.occupiedCells.Add(snappedWorld, this);
	}

	public async void Explode()
	{
		// Mark that it has been lit and change its texture
		exploded = true;
		Visible = false;

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
