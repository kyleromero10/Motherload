using Godot;
using System;

public partial class Gunpowder : StaticBody2D
{
    [Export]
    public Area2D lightRadius;
    public bool lit = false;

    [Export]
    public CompressedTexture2D litTexture;
    public async void Lit()
    {
        // Mark that it has been lit and change its texture
        lit = true;
        GetChild<Sprite2D>(1).Texture = litTexture;
        // wait then light nearby
        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        LightNearby();
    }

    public void LightNearby()
    {
        // checks each node in its radius and lights the specific ones
        foreach(StaticBody2D node in lightRadius.GetOverlappingBodies())
        {
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
