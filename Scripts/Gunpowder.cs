using Godot;
using System;

public partial class Gunpowder : StaticBody2D
{
	[Export]
	public Area2D lightRadius;
	public bool lit = false;
	
	public override void _Ready()
	{
		var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("idle");
	}

	public async void Lit()
	{
		lightRadius.Monitoring = false;
		lightRadius.Monitoring = true;

		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		lit = true;
		AudioManager.I?.PlayGunpowderLit();

		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("burn");

		// Wait until animation finishes
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);

		LightNearby();
	}

	public void LightNearby()
	{
		// checks each node in its radius and lights the specific ones
		foreach(StaticBody2D node in lightRadius.GetOverlappingBodies())
		{
			if (node == this)
				continue;
			// Gunpowder trails
			if(node is Gunpowder gunpowder)
			{
				if(!gunpowder.lit)
					gunpowder.Lit();
			}
			// Small Explosives
			else if(node is SmallExplosive smallExplosive)
			{
				if(!smallExplosive.lit)
					smallExplosive.Lit();
			}
			// Medium Explosives
			else if(node is MediumExplosive mediumExplosive)
			{
				if(!mediumExplosive.lit)
					mediumExplosive.Lit();
			}
		}
		// deletes itself because it was causing lag by staying in the world
		QueueFree();
	}
}
