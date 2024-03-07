> Project made with GoDot 4.2

# Jump
GoDot: Exemple of 2D game with jump calculation


The whole point of this project is the Jump calculation

Also, one thing to note is the usage of `Velocity` and not `Position`:
With `Velocity`, the `MoveAndSlide` will detect colisions and make the `IsOnFloor()` work propely.

``` c#
using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public int Speed = 15000; // (pixels/sec).
	public float JumpHeight = 150f;
	public float JumpTimeToPeak = 0.5f;
	public float JumpTimeToDescent = 0.4f;
	
	public float JumpVelocity;
	public float JumpGravity;
	public float FallGravity;
	
	public Vector2 ScreenSize;
	public Vector2 velocity = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		
		JumpVelocity = ((2 * JumpHeight) / JumpTimeToPeak) * -1;
		JumpGravity = ((-2 * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1;
		FallGravity = ((-2 * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PhysicsProcess(delta);
		NormalizePosition();
	}
	
	public void Start(Vector2 position)
	{
		GD.Print("Player Start");
		Position = position;
	}
	
	private Vector2 GetPlayerMovement()
	{
		var velocity = Vector2.Zero;
		
		if (Input.IsActionPressed("move_right"))
			velocity.X += 1;

		if (Input.IsActionPressed("move_left"))
			velocity.X -= 1;

		if (Input.IsActionPressed("jump"))
		{
			velocity.Y -= 1;
		} 
		else if (!IsOnFloor()) {
			velocity.Y += 1;
		}
			
		if (Input.IsActionPressed("duck"))
			velocity.Y += 1;
			
		return velocity;
	}
	
	private void PhysicsProcess(double delta)
	{
		velocity.Y += GetGravity() * (float)delta;
		velocity.X = GetInputVelocity() * Speed * (float)delta;
		
		if (Input.IsActionPressed("jump") && IsOnFloor())
			Jump();
		
		Velocity = velocity;
		MoveAndSlide();
		velocity = Velocity;
	}
	
	private float GetGravity()
	{
		return velocity.Y < 0 ? JumpGravity : FallGravity;
	}
	
	private void Jump()
	{
		velocity.Y = JumpVelocity;
	}
	
	private float GetInputVelocity()
	{
		if (Input.IsActionPressed("move_right"))
			return 1;

		if (Input.IsActionPressed("move_left"))
			return -1;
			
		return 0;
	}
	
	private void NormalizePosition()
	{
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}

```
