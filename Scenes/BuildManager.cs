using Godot;
using System;
using System.Collections.Generic;

public partial class BuildManager : Node2D
{
	public Dictionary<Vector2, StaticBody2D> occupiedCells = new();
	[Export]
	public TileMapLayer placeableTileMap;
	[Export]
	public TileMapLayer unplaceableTileMap;
	[Export]
	public TileMapLayer wallsTileMap;
	[Export]
	public TileMapLayer destructablesTileMap;
	[Export]
	public PackedScene start;
	[Export]
	public PackedScene gunpowder;
	[Export]
	public PackedScene smallExplosive;
	[Export]
	public PackedScene mediumExplosive;
	[Export]
	public PackedScene errorObject;
	[Export]
	public PackedScene errorObjectMedium;
	public bool isPlaceable;
	public bool placeableStatus;
	public Vector2 snappedWorld;
	public StaticBody2D previewObject;
	public int activeObject;
	public override void _Ready()
	{
		base._Ready();
		// sets the start to gunpowder (0: start, 1: gunpowder, 2: small, 3: medium)
		activeObject = 1;
		ChangePreview();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		previewObject.GlobalPosition = snappedWorld;
		// if the status changes then it looks to update it
		if (GetIsPlaceable() != placeableStatus)
		{
			ChangePreview();
			placeableStatus = !placeableStatus;
		}
		// placing an object
		if (Input.IsActionPressed("right_click"))
		{
			PlaceObject(activeObject);
		}
		// changing to gunpowder
		if (Input.IsActionJustPressed("ui_1"))
		{
			activeObject = 1;
			ChangePreview();
		}
		// changing to small explosive
		if (Input.IsActionJustPressed("ui_2"))
		{
			activeObject = 2;
			ChangePreview();
		}
		// changing to medium explosive
		if (Input.IsActionJustPressed("ui_3"))
		{
			activeObject = 3;
			ChangePreview();
		}
	}

	public void PlaceObject(int activeObject)
	{
		// if placable spot then places the active object
		if(GetIsPlaceable())
		{
			// makes sure the object can replace others without stacking
			if(activeObject == 3){
				StaticBody2D overlappingObject;
				StaticBody2D overlappingObject2nd;
				StaticBody2D overlappingObject3rd;
				StaticBody2D overlappingObject4th;
				if(occupiedCells.ContainsKey(snappedWorld))
				{
					overlappingObject = occupiedCells[snappedWorld];
					// will not replace start or gold
					if(overlappingObject is Start || overlappingObject is GoldVein || overlappingObject is CaveSupport)
					{
						return;
					}
					if(overlappingObject is MediumExplosive mediumExplosive)
					{
						occupiedCells.Remove(mediumExplosive.snappedWorld2nd);
						occupiedCells.Remove(mediumExplosive.snappedWorld3rd);
						occupiedCells.Remove(mediumExplosive.snappedWorld4th);
					}
					occupiedCells.Remove(snappedWorld);
					overlappingObject.QueueFree();
				}
				if(occupiedCells.ContainsKey(new Vector2 (snappedWorld.X - 64, snappedWorld.Y)))
				{
					overlappingObject2nd = occupiedCells[new Vector2 (snappedWorld.X - 64, snappedWorld.Y)];
					// will not replace start or gold
					if(overlappingObject2nd is Start || overlappingObject2nd is GoldVein || overlappingObject2nd is CaveSupport)
					{
						return;
					}
					if(overlappingObject2nd is MediumExplosive mediumExplosive)
					{
						occupiedCells.Remove(mediumExplosive.snappedWorld2nd);
						occupiedCells.Remove(mediumExplosive.snappedWorld3rd);
						occupiedCells.Remove(mediumExplosive.snappedWorld4th);
					}
					occupiedCells.Remove(new Vector2 (snappedWorld.X - 64, snappedWorld.Y));
					overlappingObject2nd.QueueFree();
				}
				if(occupiedCells.ContainsKey(new Vector2 (snappedWorld.X - 64, snappedWorld.Y - 64)))
				{
					overlappingObject3rd = occupiedCells[new Vector2 (snappedWorld.X - 64, snappedWorld.Y - 64)];
					// will not replace start or gold
					if(overlappingObject3rd is Start || overlappingObject3rd is GoldVein || overlappingObject3rd is CaveSupport)
					{
						return;
					}
					if(overlappingObject3rd is MediumExplosive mediumExplosive)
					{
						occupiedCells.Remove(mediumExplosive.snappedWorld2nd);
						occupiedCells.Remove(mediumExplosive.snappedWorld3rd);
						occupiedCells.Remove(mediumExplosive.snappedWorld4th);
					}
					occupiedCells.Remove(new Vector2 (snappedWorld.X - 64, snappedWorld.Y - 64));
					overlappingObject3rd.QueueFree();
				}
				if(occupiedCells.ContainsKey(new Vector2 (snappedWorld.X, snappedWorld.Y- 64)))
				{
					overlappingObject4th = occupiedCells[new Vector2 (snappedWorld.X, snappedWorld.Y - 64)];
					// will not replace start or gold
					if(overlappingObject4th is Start || overlappingObject4th is GoldVein || overlappingObject4th is CaveSupport)
					{
						return;
					}
					if(overlappingObject4th is MediumExplosive mediumExplosive)
					{
						occupiedCells.Remove(mediumExplosive.snappedWorld2nd);
						occupiedCells.Remove(mediumExplosive.snappedWorld3rd);
						occupiedCells.Remove(mediumExplosive.snappedWorld4th);
					}
					occupiedCells.Remove(new Vector2 (snappedWorld.X, snappedWorld.Y - 64));
					overlappingObject4th.QueueFree();
				}
			}
			else
			{
				if(occupiedCells.ContainsKey(snappedWorld))
				{
					var overlappingObject = occupiedCells[snappedWorld];
					// will not replace start or gold
					if(overlappingObject is Start || overlappingObject is GoldVein || overlappingObject is CaveSupport)
					{
						return;
					}
					if(overlappingObject is MediumExplosive mediumExplosive)
					{
						occupiedCells.Remove(mediumExplosive.snappedWorld2nd);
						occupiedCells.Remove(mediumExplosive.snappedWorld3rd);
						occupiedCells.Remove(mediumExplosive.snappedWorld4th);
					}
					occupiedCells.Remove(snappedWorld);
					overlappingObject.QueueFree();
				}
			}
			switch (activeObject)
			{
				case 1:
					var spawnedGunpowder = gunpowder.Instantiate<Gunpowder>();
					spawnedGunpowder.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(spawnedGunpowder);
					spawnedGunpowder.ForceUpdateTransform();
					occupiedCells.Add(snappedWorld, spawnedGunpowder);
					spawnedGunpowder.snappedWorld = snappedWorld;
					AudioManager.I?.PlayGunpowderPlace();
					break;
				case 2:
					var spawnedSmallExplosive = smallExplosive.Instantiate<SmallExplosive>();
					spawnedSmallExplosive.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(spawnedSmallExplosive);
					spawnedSmallExplosive.ForceUpdateTransform();
					occupiedCells.Add(snappedWorld, spawnedSmallExplosive);
					spawnedSmallExplosive.snappedWorld = snappedWorld;
					AudioManager.I?.PlayBombPlace();
					break;
				case 3:
					var spawnedMediumExplosive = mediumExplosive.Instantiate<MediumExplosive>();
					spawnedMediumExplosive.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(spawnedMediumExplosive);
					spawnedMediumExplosive.ForceUpdateTransform();
					spawnedMediumExplosive.snappedWorld = snappedWorld;
					spawnedMediumExplosive.snappedWorld2nd = new Vector2 (snappedWorld.X - 64, snappedWorld.Y);
					spawnedMediumExplosive.snappedWorld3rd = new Vector2 (snappedWorld.X - 64, snappedWorld.Y - 64);
					spawnedMediumExplosive.snappedWorld4th = new Vector2 (snappedWorld.X, snappedWorld.Y - 64);
					occupiedCells.Add(spawnedMediumExplosive.snappedWorld, spawnedMediumExplosive);
					occupiedCells.Add(spawnedMediumExplosive.snappedWorld2nd, spawnedMediumExplosive);
					occupiedCells.Add(spawnedMediumExplosive.snappedWorld3rd, spawnedMediumExplosive);
					occupiedCells.Add(spawnedMediumExplosive.snappedWorld4th, spawnedMediumExplosive);
					AudioManager.I?.PlayBombPlace();
					break;
				default:
					// can change to allow start to be placeable
					var spawnedStart = start.Instantiate<Start>();
					spawnedStart.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(spawnedStart);
					spawnedStart.ForceUpdateTransform();
					occupiedCells.Add(snappedWorld, spawnedStart);
					break;
			}
		} 
	}

	public bool GetIsPlaceable()
	{
		// Gets the mouse position and finds the global location
		Vector2 mouseWorld = GetGlobalMousePosition();
		Vector2 localPos = placeableTileMap.ToLocal(mouseWorld);
		Vector2I cell = placeableTileMap.LocalToMap(localPos);
		Vector2 snappedLocal = placeableTileMap.MapToLocal(cell);
		snappedWorld = placeableTileMap.ToGlobal(snappedLocal);
		// checks the source Id of the tilemap and looks for correct one
		// medium has to check all 4 boxes
		if (activeObject == 3)
		{
			int placeableSourceId = placeableTileMap.GetCellSourceId(cell);
			int unplacableSourceId = unplaceableTileMap.GetCellSourceId(cell);
			int wallsSourceId = wallsTileMap.GetCellSourceId(cell);
			int destructablesSourceId = destructablesTileMap.GetCellSourceId(cell);
			if (placeableSourceId == -1 || unplacableSourceId == 0 || wallsSourceId == 0 || destructablesSourceId == 1)
			{
				return false;
			}
			else
			{
				int placeableSourceId2nd = placeableTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y));
				int unplacableSourceId2nd = unplaceableTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y));
				int wallsSourceId2nd = wallsTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y));
				int destructablesSourceId2nd = destructablesTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y));
				if (placeableSourceId2nd == -1 || unplacableSourceId2nd == 0 || wallsSourceId2nd == 0 || destructablesSourceId2nd == 0)
				{
					return false;
				}
				else
				{
					int placeableSourceId3rd = placeableTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y - 1));
					int unplacableSourceId3rd = unplaceableTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y - 1));
					int wallsSourceId3rd = wallsTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y - 1));
					int destructablesSourceId3rd = destructablesTileMap.GetCellSourceId(new Vector2I(cell.X - 1, cell.Y - 1));
					if (placeableSourceId3rd == -1 || unplacableSourceId3rd == 0 || wallsSourceId3rd == 0 || destructablesSourceId3rd == 0)
					{
						return false;
					}
					else
					{
						int placeableSourceId4th = placeableTileMap.GetCellSourceId(new Vector2I(cell.X, cell.Y - 1));
						int unplacableSourceId4th = unplaceableTileMap.GetCellSourceId(new Vector2I(cell.X, cell.Y - 1));
						int wallsSourceId4th = wallsTileMap.GetCellSourceId(new Vector2I(cell.X, cell.Y - 1));
						int destructablesSourceId4th = destructablesTileMap.GetCellSourceId(new Vector2I(cell.X, cell.Y - 1));
						if (placeableSourceId4th == -1 || unplacableSourceId4th == 0 || wallsSourceId4th == 0 || destructablesSourceId4th == 0)
						{
							return false;
						}
						else
						{
							return true;
						}
					}
				}
			}
		}
		else
		{
			int placeableSourceId = placeableTileMap.GetCellSourceId(cell);
			int unplacableSourceId = unplaceableTileMap.GetCellSourceId(cell);
			int wallsSourceId = wallsTileMap.GetCellSourceId(cell);
			int destructablesSourceId = destructablesTileMap.GetCellSourceId(cell);
			if (placeableSourceId == -1 || unplacableSourceId == 0 || wallsSourceId == 0 || destructablesSourceId == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}

	public void ChangePreview()
	{
		// if placable will show the active object else will show errorObject
		if (GetIsPlaceable())
		{
			switch (activeObject)
			{
				case 1:
					previewObject?.QueueFree();
					previewObject = gunpowder.Instantiate<StaticBody2D>();
					previewObject.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(previewObject);
					previewObject.CollisionLayer = 0;
					previewObject.CollisionMask = 0;
					break;
				case 2:
					previewObject?.QueueFree();
					previewObject = smallExplosive.Instantiate<StaticBody2D>();
					previewObject.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(previewObject);
					previewObject.CollisionLayer = 0;
					previewObject.CollisionMask = 0;
					break;
				case 3:
					previewObject?.QueueFree();
					previewObject = mediumExplosive.Instantiate<StaticBody2D>();
					previewObject.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(previewObject);
					previewObject.CollisionLayer = 0;
					previewObject.CollisionMask = 0;
					break;
				default:
					previewObject?.QueueFree();
					previewObject = start.Instantiate<StaticBody2D>();
					previewObject.GlobalPosition = snappedWorld;
					GetTree().CurrentScene.AddChild(previewObject);
					previewObject.CollisionLayer = 0;
					previewObject.CollisionMask = 0;
					break;
			}
		}
		else
		{
			if (activeObject == 3)
			{
				previewObject?.QueueFree();
				previewObject = errorObjectMedium.Instantiate<StaticBody2D>();
				previewObject.GlobalPosition = snappedWorld;
				GetTree().CurrentScene.AddChild(previewObject);
			}
			else
			{
				previewObject?.QueueFree();
				previewObject = errorObject.Instantiate<StaticBody2D>();
				previewObject.GlobalPosition = snappedWorld;
				GetTree().CurrentScene.AddChild(previewObject);
			}
		}
	}
}
