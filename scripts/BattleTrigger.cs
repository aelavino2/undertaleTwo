using Godot;

public partial class BattleTrigger : Area2D
{
	[Export] public string BattleScenePath = "res://scenes/Battle.tscn";
	private bool _triggered = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (_triggered) return;

		if (body.Name == "Player" || body is CharacterBody2D)
		{
			_triggered = true;
			GD.Print("Игрок зашёл в зону боя — запускаем Battle!");
			GetTree().ChangeSceneToFile(BattleScenePath);
		}
	}
}
