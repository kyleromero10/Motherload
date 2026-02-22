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
    [Export] public TileMapLayer destructiblesTileMap;
	public Vector2 snappedWorld;
	[Signal] public delegate void LevelEndEventHandler();

	public override void _Ready()
	{
		base._Ready();
        Visible = false;
		gameMaster = GetNode<GameMaster>("/root/Level/GameMaster");
		buildManager = GetNode<BuildManager>("/root/Level");
		tileMap = GetNode<TileMapLayer>("/root/Level/PlaceableTiles");
        destructiblesTileMap = GetNode<TileMapLayer>("/root/Level/Destructibles");
        // gets its position and send it to the builders dictionary
		Vector2 localPos = tileMap.ToLocal(this.GlobalPosition);
		Vector2I cell = tileMap.LocalToMap(localPos);
		Vector2 snappedLocal = tileMap.MapToLocal(cell);
		snappedWorld = tileMap.ToGlobal(snappedLocal);
		buildManager.occupiedCells.Add(snappedWorld, this);
        if (isGoal)
        {
            buildManager.occupiedCells.Add(new Vector2 (snappedWorld.X + 64, snappedWorld.Y), this);
            buildManager.occupiedCells.Add(new Vector2 (snappedWorld.X, snappedWorld.Y + 64), this);
            buildManager.occupiedCells.Add(new Vector2 (snappedWorld.X + 64, snappedWorld.Y + 64), this);
        }
	}

	public async void Explode()
	{
		// Mark that it has been lit and change its texture
		exploded = true;
		Visible = false;
        // trying to delete visual
        //destructiblesTileMap.SetCell((Vector2I) snappedWorld, 0, new Vector2I (7, 0));
        //destructiblesTileMap.EraseCell((Vector2I) snappedWorld);
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
