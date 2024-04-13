using Godot;
using System;

public partial class TrashCreator : Node
{
	[Export] private PackedScene trash;
	[Export] private PackedScene[] allPossibleTrash;

	private Timer timer;

	private const int MaxXPosition = 5;
	private const int MinXPosition = -5;
	private const int MaxZPosition = -1;
	private const int MinZPosition = -10;

	public override void _Ready(){
		timer = GetNode<Timer>("Timer");
		GD.Randomize();
		OnTimeout();
	}
	
	public void OnTimeout(){
		CreateTrash();
		timer.WaitTime = GD.Randf()/2;
		timer.Start();
	}

	private void CreateTrash(){
		Node3D spawnedTrash = trash.Instantiate<Node3D>();

		AddChild(spawnedTrash);
		spawnedTrash.AddChild(allPossibleTrash[0].Instantiate());

		spawnedTrash.GlobalPosition = new Vector3(
			GD.RandRange(MinXPosition, MaxXPosition),
			5f,
			GD.RandRange(MinZPosition, MaxZPosition)
		);
		spawnedTrash.GlobalRotation = new Vector3(
			Mathf.DegToRad(GD.RandRange(0,360)),
			Mathf.DegToRad(GD.RandRange(0,360)),
			Mathf.DegToRad(GD.RandRange(0,360))
		);
		GD.Print(spawnedTrash.GlobalPosition);


		//PackedScene spawnedTrash = allPossibleTrash[(int)GD.Randi() % allPossibleTrash.Count -1];
		//Node3D trashInstance = spawnedTrash.Instantiate<Node3D>();
	}
}
