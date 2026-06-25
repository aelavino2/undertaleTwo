using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 150.0f;
	private AnimatedSprite2D _animatedSprite;
	private Vector2 _lastDirection = Vector2.Down;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		GD.Print("Доступные анимации:");
		foreach (var anim in _animatedSprite.SpriteFrames.GetAnimationNames())
		{
			GD.Print(" - " + anim);
		}
	}

	private void UpdateAnimation(Vector2 direction)
	{
		if (direction != Vector2.Zero)
			_lastDirection = direction;

		string prefix = direction == Vector2.Zero ? "idle" : "walk";
		string suffix;

		if (Mathf.Abs(_lastDirection.X) > Mathf.Abs(_lastDirection.Y))
			suffix = _lastDirection.X > 0 ? "right" : "left";
		else
			suffix = _lastDirection.Y > 0 ? "down" : "up";

		_animatedSprite.Play(new StringName($"{prefix}_{suffix}"));
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;

		if (Input.IsActionPressed("ui_right")) direction.X += 1;
		if (Input.IsActionPressed("ui_left"))  direction.X -= 1;
		if (Input.IsActionPressed("ui_down"))  direction.Y += 1;
		if (Input.IsActionPressed("ui_up"))    direction.Y -= 1;

		Velocity = direction.Normalized() * Speed;
		MoveAndSlide();
		UpdateAnimation(direction);
	}
}
