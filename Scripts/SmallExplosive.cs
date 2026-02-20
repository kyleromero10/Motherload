using Godot;
using System;

public partial class SmallExplosive : StaticBody2D
{
    [Export]
    public Area2D explosionRadius;
    public bool lit = false;

    [Export]
    public CompressedTexture2D litTexture;
    [Export]
    public CompressedTexture2D explodedTexture;

    public async void Lit()
    {
        // Mark that it has been lit and change its texture
        lit = true;
        GetChild<Sprite2D>(1).Texture = litTexture;
        // wait to explode
        await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
        Explode();
        GetChild<Sprite2D>(1).Texture = explodedTexture;
    }

    public async void Explode()
    {
        // checks each node in its radius and lights the specific ones
        foreach(StaticBody2D node in explosionRadius.GetOverlappingBodies())
        {
            // Gunpowder trails
            if(node is Gunpowder gunpowder)
            {
                if(!gunpowder.lit)
                    gunpowder.Lit();
            }
            // small explosives
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
