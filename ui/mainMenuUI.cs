using Godot;
using System;

public partial class mainMenuUI : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayPressed(){
		GetTree().ChangeSceneToFile("res://scenes/mainScene.tscn");
	}
	public void OnSettingsPressed(){

	}
	public void OnCreatorsPressed(){

	}
	public void OnExitPressed(){
		GetTree().Quit();
	}
}
