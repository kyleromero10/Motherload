using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    public Camera2D camera;
    public int zoomCount = 0;
    public override void _Ready()
    {
        base._Ready();
        camera = GetChild<Camera2D>(0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Vector2 myInputAxis = new Vector2(
                Input.GetAxis("player_left", "player_right"),
                Input.GetAxis("player_up", "player_down")
            );
        Velocity = myInputAxis.Normalized() * 300;
        if (Input.IsActionJustPressed("player_zoom_in"))
        {
            zoomCount++;
            if(zoomCount > 2)
                zoomCount = 2;
            switch (zoomCount)
            {
                case 0:
                    camera.Zoom = new Vector2 (1f, 1f);
                    break;
                case 1:
                    camera.Zoom = new Vector2 (2f, 2f);
                    break;
                case 2:
                    camera.Zoom = new Vector2 (3f, 3f);
                    break;
                case -1:
                    camera.Zoom = new Vector2 (0.5f, 0.5f);
                    break;
                case -2:
                    camera.Zoom = new Vector2 (0.25f, 0.25f);
                    break;
                default:
                    camera.Zoom = new Vector2 (1f, 1f);
                    break;
            }
        }
        if (Input.IsActionJustPressed("player_zoom_out"))
        {
            zoomCount--;
            if(zoomCount < -2)
                zoomCount = -2;
            switch (zoomCount)
            {
                case 0:
                    camera.Zoom = new Vector2 (1f, 1f);
                    break;
                case 1:
                    camera.Zoom = new Vector2 (2f, 2f);
                    break;
                case 2:
                    camera.Zoom = new Vector2 (3f, 3f);
                    break;
                case -1:
                    camera.Zoom = new Vector2 (0.5f, 0.5f);
                    break;
                case -2:
                    camera.Zoom = new Vector2 (0.25f, 0.25f);
                    break;
                default:
                    camera.Zoom = new Vector2 (1f, 1f);
                    break;
            }
        }
        MoveAndSlide();
    }
}
