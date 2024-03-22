using Godot;
using System;

public partial class Sun : Node3D
{
	[Signal] public delegate void TheSunWasClickedEventHandler();

	[Export] Area3D clickablaArea;
	public override void _Ready(){
		this.Scale = new Vector3(4,4,4);
		clickablaArea = this.GetNode<Area3D>("ClickableArea");
		//clickablaArea.Connect("input_event",Callable.From(() => ));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		RotateY(0.1f * (float)delta);
	}

	private void OnArea3dInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx){
		if(@event is InputEventMouseButton b && b.Pressed && b.ButtonIndex == MouseButton.Left)
				EmitSignal(SignalName.TheSunWasClicked);
	}
}
