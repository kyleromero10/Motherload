using Godot;
using System;

public partial class SmallExplosive : StaticBody2D
{
	[Export]
	public Area2D explosionRadius;

	public bool lit = false;

	private AnimatedSprite2D sprite;
	[Export] public BuildManager buildManager;
	public Vector2 snappedWorld;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("idle");
	}
	
	public async void Lit()
	{
		buildManager = GetNode<BuildManager>("/root/Level");
		if (lit)
			return;

		explosionRadius.Monitoring = false;
		explosionRadius.Monitoring = true;

		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		lit = true;

		sprite.Play("lit");

		//wait until the lit animation finishes
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);

		if (sprite.Animation == "lit")
		{
			Explode();
		}
	}

	public async void Explode()
	{
		AudioManager.I?.PlaySmallExplode();

		sprite.Play("explode");

		//damage nearby objects immediately
		foreach (StaticBody2D node in explosionRadius.GetOverlappingBodies())
		{
			if (node == this)
				continue;

			if (node is Gunpowder gunpowder && !gunpowder.lit)
				gunpowder.Lit();

			else if (node is SmallExplosive smallExplosive && !smallExplosive.lit)
				smallExplosive.Lit();

			else if (node is MediumExplosive mediumExplosive && !mediumExplosive.lit)
				mediumExplosive.Lit();

			else if (node is GoldVein goldVein && !goldVein.exploded)
				goldVein.Explode();

			else if (node is CaveSupport caveSupport && !caveSupport.exploded)
				caveSupport.Explode();
		}

		//wait for explosion animation to finish
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);

		Delete();
	}

	public void Delete()
	{
		QueueFree();
	}
}
