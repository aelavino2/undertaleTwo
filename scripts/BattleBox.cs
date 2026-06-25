using Godot;

public partial class BattleBox : Node2D
{
	[Export] public NodePath InnerPath; // путь до ColorRect "Inner"

	private ColorRect _inner;

	public override void _Ready()
	{
		_inner = GetNode<ColorRect>(InnerPath);
	}

	// Возвращает границы арены в глобальных 2D координатах
	public Rect2 GetWorldBounds()
	{
		Vector2 globalTopLeft = _inner.GetGlobalTransform().Origin;
		Vector2 size = _inner.Size;
		return new Rect2(globalTopLeft, size);
	}
}
