using Godot;
using System;

public partial class MediumExplosive : StaticBody2D
{
	[Export]
	public Area2D explosionRadius;

	private AnimatedSprite2D sprite;
	public bool lit = false;

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// start in idle animation
		sprite.Play("idle");

		// make sure explosive is not lit initially
		lit = false;
	}

	public async void Lit()
	{
		if (lit)
			return;

		// force the explosion radius to recheck overlapping bodies
		explosionRadius.Monitoring = false;
		explosionRadius.Monitoring = true;

		// wait one physics frame for safety
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		lit = true;

		// play fuse animation
		sprite.Play("lit");

		// wait until the lit animation finishes
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);

		if (sprite.Animation == "lit")
			Explode();
	}

	public async void Explode()
	{
		// play explosion animation
		sprite.Play("explode");

		// Immediately light nearby explosives / gunpowder
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
		}

		// wait for the explosion animation to finish before removing
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);

		QueueFree();
	}
}
