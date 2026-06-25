using Godot;

public partial class BulletSpawner : Node2D
{
	[Export] public PackedScene BulletScene;
	[Export] public NodePath BattleBoxPath;
	[Export] public NodePath HeartPath; // НОВОЕ: путь до Heart
	[Export] public float SpawnInterval = 1.0f;

	private BattleBox _box;
	private Heart _heart;
	private Timer _timer;
	private RandomNumberGenerator _rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		_box = GetNode<BattleBox>(BattleBoxPath);
		_heart = GetNode<Heart>(HeartPath);

		_timer = new Timer();
		_timer.WaitTime = SpawnInterval;
		_timer.Autostart = true;
		_timer.Timeout += SpawnBullet;
		AddChild(_timer);
	}

	private void SpawnBullet()
	{
		if (BulletScene == null) return;

		Rect2 bounds = _box.GetWorldBounds();
		int side = _rng.RandiRange(0, 3);
		Vector2 spawnPos, targetPoint;

		switch (side)
		{
			case 0: // сверху
				spawnPos = new Vector2(_rng.RandfRange(bounds.Position.X, bounds.Position.X + bounds.Size.X), bounds.Position.Y - 50);
				targetPoint = new Vector2(spawnPos.X, bounds.Position.Y + bounds.Size.Y / 2);
				break;
			case 1: // снизу
				spawnPos = new Vector2(_rng.RandfRange(bounds.Position.X, bounds.Position.X + bounds.Size.X), bounds.Position.Y + bounds.Size.Y + 50);
				targetPoint = new Vector2(spawnPos.X, bounds.Position.Y + bounds.Size.Y / 2);
				break;
			case 2: // слева
				spawnPos = new Vector2(bounds.Position.X - 50, _rng.RandfRange(bounds.Position.Y, bounds.Position.Y + bounds.Size.Y));
				targetPoint = new Vector2(bounds.Position.X + bounds.Size.X / 2, spawnPos.Y);
				break;
			default: // справа
				spawnPos = new Vector2(bounds.Position.X + bounds.Size.X + 50, _rng.RandfRange(bounds.Position.Y, bounds.Position.Y + bounds.Size.Y));
				targetPoint = new Vector2(bounds.Position.X + bounds.Size.X / 2, spawnPos.Y);
				break;
		}

		var bullet = BulletScene.Instantiate<Bullet>();
		bullet.GlobalPosition = spawnPos;
		bullet.Direction = (targetPoint - spawnPos).Normalized();

		GetTree().CurrentScene.AddChild(bullet);

		// НОВОЕ: указываем пуле, за кем гнаться
		bullet.TargetPath = bullet.GetPathTo(_heart);
	}
}
