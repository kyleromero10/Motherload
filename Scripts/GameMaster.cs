using Godot;
using System;
using System.Data;
using System.IO;

public partial class GameMaster : Node
{
	[Export] public Control UI;
	[Export] public Control winScreen;
	[Export] public Control loseScreen;

	public int goldCollected;
	public int totalHealth = 3;
	
	public override void _Ready()
	{
		base._Ready();
		//UNCOMMENT BELOW TO START BGM
		AudioManager.I?.PlayStageBgm();
		// connects the level end signal to the function that will end the level
		GetNode<GoldVein>("/root/Level/GoldVein").Connect("LevelEnd", Callable.From(EndLevel));
		foreach(Node node in GetTree().GetNodesInGroup("Support"))
		{
			node.Connect("LevelFailed", Callable.From(FailedLevel));
		}

		goldCollected = 0;

	}

	public void updateLabels()
	{
		UI.GetNode<Label>("GoldCollected").Text = "Gold Collected: " + goldCollected;
		UI.GetNode<Label>("SupportHealth").Text = "Support Health: " + totalHealth;
	}

	public void EndLevel()
	{
		// changes the scene to the win scene
		UI.GetNode<Label>("SupportHealth").Text = "Support Health: " + 0;
		winScreen.GetNode<Label>("Label2").Text = "Gold Collected: " + goldCollected;
		winScreen.Visible = true;
		
		GD.Print("Level Ended");
		AudioManager.I?.PlayVictory();
		AudioManager.I?.StopBgm();
	}

	public void FailedLevel()
	{
		// changes the scene to the failed scene
		loseScreen.Visible = true;
		GD.Print("Level Failed");
		
	}
}
