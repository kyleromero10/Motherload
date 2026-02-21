using Godot;
using System;

public partial class GameMaster : Node
{

    public int goldCollected;
    
    public override void _Ready()
    {
        base._Ready();
        // connects the level end signal to the function that will end the level
        GetNode<GoldVein>("Level/GoldVein").Connect("LevelEnd", Callable.From(EndLevel));

        goldCollected = 0;
    }

    public void EndLevel()
    {
        // changes the scene to the win scene
        
    }

}
