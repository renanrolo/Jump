using Godot;
using System;

public partial class Main : Node2D
{
	private Player player;
	private Label playerX;
	private Label playerY;
	private Label playerCoyoteTime;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		playerX = GetNode<Label>("VelocityX");
		playerY = GetNode<Label>("VelocityY");
		playerCoyoteTime = GetNode<Label>("CoyoteTimeLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//playerX.Text = "X: " + player.Velocity.X;
		playerX.Text = $"X: {player.Velocity.X}";
		playerY.Text = $"Y: {player.Velocity.Y}";
		playerCoyoteTime.Text = $"Coyote Time: {player.CoyoteTime}";
	}
}
