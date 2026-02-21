using Godot;
using System;

public partial class BuildManager : Node2D
{
    [Export]
    public TileMapLayer tileMap;
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
        if(GetIsPlaceable() != placeableStatus)
        {
            ChangePreview();
            placeableStatus = !placeableStatus;
        }
        // placing an object
        if (Input.IsActionJustPressed("left_click"))
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
            switch (activeObject)
            {
                case 1:
                    var spawnedGunpowder = gunpowder.Instantiate<Gunpowder>();
                    spawnedGunpowder.GlobalPosition = snappedWorld;
                    GetTree().CurrentScene.AddChild(spawnedGunpowder);
                    spawnedGunpowder.ForceUpdateTransform();
                    break;
                case 2:
                    var spawnedSmallExplosive = smallExplosive.Instantiate<SmallExplosive>();
                    spawnedSmallExplosive.GlobalPosition = snappedWorld;
                    GetTree().CurrentScene.AddChild(spawnedSmallExplosive);
                    spawnedSmallExplosive.ForceUpdateTransform();
                    break;
                case 3:
                    var spawnedMediumExplosive = mediumExplosive.Instantiate<MediumExplosive>();
                    spawnedMediumExplosive.GlobalPosition = snappedWorld;
                    GetTree().CurrentScene.AddChild(spawnedMediumExplosive);
                    spawnedMediumExplosive.ForceUpdateTransform();
                    break;
                default:
                    // can change to allow start to be placeable
                    var spawnedStart = start.Instantiate<Start>();
                    spawnedStart.GlobalPosition = snappedWorld;
                    GetTree().CurrentScene.AddChild(spawnedStart);
                    spawnedStart.ForceUpdateTransform();
                    break;
            }
        } 
    }

    public bool GetIsPlaceable()
    {
        // Gets the mouse position and finds the global location
        Vector2 mouseWorld = GetGlobalMousePosition();
        Vector2 localPos = tileMap.ToLocal(mouseWorld);
        Vector2I cell = tileMap.LocalToMap(localPos);
        Vector2 snappedLocal = tileMap.MapToLocal(cell);
        snappedWorld = tileMap.ToGlobal(snappedLocal);
        // checks the source Id of the tilemap and looks for correct one
        int sourceId = tileMap.GetCellSourceId(cell);
        // -1 is nothing, 1 is maptiles, 0 is placeable tiles
        if(sourceId == -1 || sourceId == 1)
        {
            return false;
        } 
        else
        {
            return true;
        }
    }

    public void ChangePreview()
    {
        // if placable will show the active object else will show errorObject
        if(GetIsPlaceable())
        {
            switch (activeObject)
            {
                case 1:
                    previewObject?.QueueFree();
                    previewObject = gunpowder.Instantiate<StaticBody2D>();
                    GetTree().CurrentScene.AddChild(previewObject);
                    previewObject.CollisionLayer = 0;
                    previewObject.CollisionMask = 0;
                    break;
                case 2:
                    previewObject?.QueueFree();
                    previewObject = smallExplosive.Instantiate<StaticBody2D>();
                    GetTree().CurrentScene.AddChild(previewObject);
                    previewObject.CollisionLayer = 0;
                    previewObject.CollisionMask = 0;
                    break;
                case 3:
                    previewObject?.QueueFree();
                    previewObject = mediumExplosive.Instantiate<StaticBody2D>();
                    GetTree().CurrentScene.AddChild(previewObject);
                    previewObject.CollisionLayer = 0;
                    previewObject.CollisionMask = 0;
                    break;
                default:
                    previewObject?.QueueFree();
                    previewObject = start.Instantiate<StaticBody2D>();
                    GetTree().CurrentScene.AddChild(previewObject);
                    previewObject.CollisionLayer = 0;
                    previewObject.CollisionMask = 0;
                    break;
            }
        } 
        else
        {
            previewObject?.QueueFree();
            previewObject = errorObject.Instantiate<StaticBody2D>();
            GetTree().CurrentScene.AddChild(previewObject);
        }
    }
}
