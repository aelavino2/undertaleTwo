using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 150.0f;

	private AnimatedSprite2D _animatedSprite;

public override void _Ready()
{
	_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
	// Выведем все доступные анимации в консоль
	GD.Print("Доступные анимации:");
	foreach (var anim in _animatedSprite.SpriteFrames.GetAnimationNames())
	{
		GD.Print(" - " + anim);
	}
}

private void UpdateAnimation(Vector2 direction)
{
	if (direction == Vector2.Zero)
	{
		_animatedSprite.Play(new StringName("idle_down"));
	}
	else
	{
		_animatedSprite.Play(new StringName("walk_down"));
	}
}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;

		if (Input.IsActionPressed("ui_right"))
			direction.X += 1;
		if (Input.IsActionPressed("ui_left"))
			direction.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			direction.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			direction.Y -= 1;

		Velocity = direction.Normalized() * Speed;
		MoveAndSlide();

		UpdateAnimation(direction);
	}
}
