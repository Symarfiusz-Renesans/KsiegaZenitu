using Godot;
using System;

public partial class Earth : Node3D{
	[Signal] public delegate void TheEarthWasClickedEventHandler();
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		RotateY(0.1f * (float)delta);
	}

	public void OnArea3dInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx){
		if(@event is InputEventMouseButton b && b.Pressed && (b.ButtonIndex == MouseButton.Left || b.ButtonIndex == MouseButton.Right)){
			EmitSignal(SignalName.TheEarthWasClicked);
		}
	}
}
