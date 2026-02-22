using Godot;
using System;
using System.IO;

public partial class GameMaster : Node
{

	public int goldCollected;
	public int totalHealth = 3;
	
	public override void _Ready()
	{
		base._Ready();
		// connects the level end signal to the function that will end the level
		GetNode<GoldVein>("/root/Level/GoldVein").Connect("LevelEnd", Callable.From(EndLevel));
		foreach(Node node in GetTree().GetNodesInGroup("Support"))
		{
			node.Connect("LevelFailed", Callable.From(FailedLevel));
		}

		goldCollected = 0;

	}

	public void EndLevel()
	{
		// changes the scene to the win scene
		GD.Print("Level Ended");
		
	}

	public void FailedLevel()
	{
		// changes the scene to the failed scene
		GD.Print("Level Failed");
		
	}
}
