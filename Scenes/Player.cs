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
	
	[Export]
	public double CoyoteTime {get; set;} = 0;
	public const double CoyoteTimeMax = 0.5;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		
		JumpVelocity = ((2 * JumpHeight) / JumpTimeToPeak) * -1;
		JumpGravity = ((-2 * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1;
		FallGravity = ((-3 * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1;
		CoyoteTime = 0.3;
		
		GD.Print($"JumpVelocity {JumpVelocity}");
		GD.Print($"JumpGravity {JumpGravity}");
		GD.Print($"FallGravity {FallGravity}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PhysicsProcess(delta);
		NormalizePosition();
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
	}
	
	private void PhysicsProcess(double delta)
	{
		velocity.Y += GetGravity() * (float)delta;
		velocity.X = GetInputVelocity() * Speed * (float)delta;
		
		if (Input.IsActionPressed("jump") && (IsOnFloor() || CoyoteTime > 0))
			Jump();
		
		Velocity = velocity;
		MoveAndSlide();
		velocity = Velocity;
		
		if(IsOnFloor())
		{
			CoyoteTime = CoyoteTimeMax;
		}
		else
		{
			CoyoteTime -= delta;
			if(CoyoteTime < 0)
			{
				CoyoteTime = 0;
			}
		}
	}
	
	private float GetGravity()
	{
		return velocity.Y < 0 ? JumpGravity : FallGravity;
	}
	
	private void Jump()
	{
		CoyoteTime = 0;
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
