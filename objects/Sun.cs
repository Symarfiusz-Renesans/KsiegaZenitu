using Godot;
using System;

public partial class Sun : Node3D
{
	[Export] Area3D clickablaArea;
	public override void _Ready(){
		this.Scale = new Vector3(4,4,4);
		clickablaArea = this.GetNode<Area3D>("ClickableArea");
		clickablaArea.Connect("input_event",Callable.From(() => ));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		RotateY(0.1f * (float)delta);
	}

	private void onClick(){

	}
}
