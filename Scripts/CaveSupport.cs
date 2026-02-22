using Godot;
using System;

public partial class CaveSupport : StaticBody2D
{
	public bool exploded = false;
	[Export] public GameMaster gameMaster;
	[Export] public BuildManager buildManager;
	[Export] public TileMapLayer tileMap;
	[Export] public TileMapLayer destructiblesTileMap;
	public Vector2 snappedWorld;
	[Signal] public delegate void LevelFailedEventHandler();
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
	}

	public async void Explode()
	{
		// Mark that it has been lit and change its texture
		exploded = true;
		Visible = false;
		// trying to delete/change visual
		destructiblesTileMap.SetCell(tileMap.LocalToMap(snappedWorld), 1, new Vector2I (7, 2));
		if(gameMaster.totalHealth <= 1)
		{
			AudioManager.I?.PlayDemerit();
			EmitSignal("LevelFailed");
		}
		else
		{
			gameMaster.totalHealth--;
			AudioManager.I?.PlayDemerit();
			GD.Print("Structual Health Remaining: " + gameMaster.totalHealth);
			gameMaster.updateLabels();
			// wait to explode
			await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
			QueueFree();
		}
	}
}
