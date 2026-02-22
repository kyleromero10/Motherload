using Godot;
using System;

public partial class Start : StaticBody2D
{
	[Export] public Area2D lightRadius;
	[Export] public GameMaster gameMaster;
	[Export] public BuildManager buildManager;
	[Export] public TileMapLayer tileMap;

	private AnimatedSprite2D sprite;
	public bool lit = false;
	public Vector2 snappedWorld;

	public override void _Ready()
	{
		base._Ready();
		
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("idle");

		//set up build manager and tile map references
		buildManager = GetNode<BuildManager>("/root/Level");
		tileMap = GetNode<TileMapLayer>("/root/Level/PlaceableTiles");

		//snap position to grid and add to occupied cells
		Vector2 localPos = tileMap.ToLocal(this.GlobalPosition);
		Vector2I cell = tileMap.LocalToMap(localPos);
		Vector2 snappedLocal = tileMap.MapToLocal(cell);
		snappedWorld = tileMap.ToGlobal(snappedLocal);
		buildManager.occupiedCells.Add(snappedWorld, this);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		//press space to light the start node
		if (Input.IsActionJustPressed("ui_start"))
		{
			Lit();
		}
	}

	public async void Lit()
	{
		if (lit) 
			return;

		//force recheck on overlapping bodies
		lightRadius.Monitoring = false;
		lightRadius.Monitoring = true;

		//slight start delay
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		lit = true;

		//play the burn animation
		sprite.Play("burn");

		//wait for burn to finish
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);

		if (sprite.Animation == "burn")
			LightNearby();
	}

	public void LightNearby()
	{
		foreach (StaticBody2D node in lightRadius.GetOverlappingBodies())
		{
			if (node == this) 
				continue;

			if (node is Gunpowder gunpowder && !gunpowder.lit)
				gunpowder.Lit();
			else if (node is SmallExplosive smallExplosive && !smallExplosive.lit)
				smallExplosive.Lit();
			else if (node is MediumExplosive mediumExplosive && !mediumExplosive.lit)
				mediumExplosive.Lit();
		}

		QueueFree();
	}    
}
