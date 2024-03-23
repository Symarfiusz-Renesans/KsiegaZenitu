using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
	private float rotate = 0;
	private float howMuchWasRotated;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		if(rotate != 0){
			RotateY(rotate * (float)delta);
			howMuchWasRotated -= rotate * (float)delta;
			if((rotate > 0 && howMuchWasRotated <= 0) || (rotate < 0 && howMuchWasRotated >= 0)){				
				RotateY(-howMuchWasRotated);
				rotate = 0;
				howMuchWasRotated = 0;
			}
		}
	}

	private void OnRotateCamera(float angle){
		rotate = angle;
		howMuchWasRotated = angle;
	}

}
