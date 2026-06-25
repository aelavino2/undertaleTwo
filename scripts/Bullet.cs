using Godot;

public partial class Bullet : Area2D
{
	[Export] public float Speed = 150f;
	[Export] public Vector2 Direction = Vector2.Right;
	[Export] public int Damage = 1;
	[Export] public float Lifetime = 5f;

	// Новое: настройки хоминга
	[Export] public bool HomingEnabled = true;
	[Export] public float TurnSpeed = 2.0f; // радиан/сек — насколько резко доворачивает на цель. Маленькое значение = плавный поворот, легче уклониться
	[Export] public NodePath TargetPath; // путь до Heart, назначим из BulletSpawner

	private float _timer = 0f;
	private Node2D _target;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;

		if (TargetPath != null && !TargetPath.IsEmpty)
			_target = GetNode<Node2D>(TargetPath);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (HomingEnabled && _target != null && IsInstanceValid(_target))
		{
			Vector2 desiredDirection = (_target.GlobalPosition - GlobalPosition).Normalized();
			// Плавно поворачиваем текущее направление в сторону цели
			float currentAngle = Direction.Angle();
			float desiredAngle = desiredDirection.Angle();
			float newAngle = Mathf.LerpAngle(currentAngle, desiredAngle, TurnSpeed * (float)delta);
			Direction = Vector2.Right.Rotated(newAngle);
		}

		Position += Direction.Normalized() * Speed * (float)delta;

		_timer += (float)delta;
		if (_timer >= Lifetime)
			QueueFree();
	}

	private void OnAreaEntered(Area2D area)
	{
		TryDamage(area);
	}

	private void OnBodyEntered(Node2D body)
	{
		TryDamage(body);
	}

	private void TryDamage(Node node)
	{
		if (node is Heart heart)
		{
			heart.TakeDamage(Damage);
			QueueFree();
		}
	}
}
