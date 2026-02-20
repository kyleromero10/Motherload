using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Start : StaticBody2D
{
    [Export]
    public Area2D lightRadius;
    public bool lit = false;

    [Export]
    public CompressedTexture2D litTexture;
    public override void _Ready()
    {
        // on start it will light
        // start will be the only part of gunpowder that is present at the start of a level
        base._Ready();
        Lit();
    }

    public async void Lit()
    {
        // delay for waiting for now
        await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);
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
            // small explosives
            else if(node is SmallExplosive smallExplosive)
            {
                if(!smallExplosive.lit)
                    smallExplosive.Lit();
            }
        }
        // deletes itself because it was causing lag by staying in the world
        QueueFree();
    }    
    
}
