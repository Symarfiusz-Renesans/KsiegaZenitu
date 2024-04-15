using Godot;
using System;

public partial class Camera3D : Godot.Camera3D{
	[Signal] public delegate void TheAngleChangedEventHandler(float angle);
	private float rotation = 0;
	private float rotateTo = 0;
	private const float speedOfRotation = 1;
	public override void _Ready()
	{
	}

	// Prowizoryczne obroty w przyszłości dodaj animację
	public override void _PhysicsProcess(double delta){
		if(rotateTo != 0){
			RotateY(Mathf.DegToRad(rotateTo));
			rotateTo = 0;
			EmitSignal(SignalName.TheAngleChanged, whatAngle());
		}
	}

	private void OnRotateCamera(float angle){
		rotateTo = angle;
		rotation += angle;
	}

	private float whatAngle(){
		GD.Print((rotation >= 0)?rotation % 360: 360 + rotation % 360);
		return (rotation >= 0)?rotation % 360: 360 + rotation % 360;
	}

}
