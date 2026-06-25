using Godot;

public partial class Heart : CharacterBody2D
{
	[Export] public float Speed = 200f;
	[Export] public NodePath BattleBoxPath;

	public int MaxHp = 20;
	public int Hp = 20;

	private Rect2 _bounds;
	private bool _hasBounds = false;

	public override void _Ready()
	{
		if (BattleBoxPath != null && !BattleBoxPath.IsEmpty)
		{
			var box = GetNode<BattleBox>(BattleBoxPath);
			_bounds = box.GetWorldBounds();
			_hasBounds = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = new Vector2(
			Input.GetAxis("ui_left", "ui_right"),
			Input.GetAxis("ui_up", "ui_down")
		);

		if (input.Length() > 1f)
			input = input.Normalized();

		Velocity = input * Speed;
		MoveAndSlide();

		if (_hasBounds)
		{
			Position = new Vector2(
				Mathf.Clamp(Position.X, _bounds.Position.X, _bounds.Position.X + _bounds.Size.X),
				Mathf.Clamp(Position.Y, _bounds.Position.Y, _bounds.Position.Y + _bounds.Size.Y)
			);
		}
	}

	public void TakeDamage(int amount)
	{
		Hp = Mathf.Max(Hp - amount, 0);
		GD.Print($"Heart HP: {Hp}/{MaxHp}");
	}
}
