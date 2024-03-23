using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
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
		}
	}

	private void OnRotateCamera(float angle){
		if (rotation == rotateTo){
			GD.Print(angle);
			rotateTo = angle;
		}
	}

}
