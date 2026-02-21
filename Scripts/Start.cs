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

    public override void _Process(double delta)
    {
        base._Process(delta);
        // when space is pressed it will light the start node
        if (Input.IsActionJustPressed("ui_start"))
        {
            Lit();
        }
    }


    public async void Lit()
    {
        // force recheck on overlapping areas
        lightRadius.Monitoring = false;
        lightRadius.Monitoring = true;
        // slight start delay
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
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
            if (node == this)
                continue;
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
