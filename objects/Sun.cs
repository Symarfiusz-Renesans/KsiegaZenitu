using Godot;
using System;

public partial class Sun : Node3D
{
	Transform3D transform = new Transform3D();
	Vector3 axis = new Vector3(1,0,0);
	[Export] const float rotation = 30f;
	public override void _Ready(){
		this.Scale = new Vector3(4,4,4);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		RotateY(0.1f * (float)delta);
	}
}
