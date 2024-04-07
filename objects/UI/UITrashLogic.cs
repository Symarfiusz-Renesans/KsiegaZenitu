using Godot;
using System;

public partial class UITrashLogic : Node3D
{
	private float speed = 0.5f;
	public override void _PhysicsProcess(double delta){
		GlobalPosition = new Vector3(Position.X, (float)(Position.Y-delta*speed), Position.Z);
		if(GlobalPosition.Y <= -2.5f){
			GD.Print("deleted");
		}
	}
}
