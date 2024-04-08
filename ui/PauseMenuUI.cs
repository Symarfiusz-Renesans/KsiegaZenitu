using Godot;
using System;

public partial class PauseMenuUI : CanvasLayer
{
	[Signal] public delegate void SaveProgressEventHandler();
	private Button ContinueButton;
	private Button SaveButton;
	private Button SettingsButton;
	private Button ExitButton;
	private MarginContainer MainOptions;
	private MarginContainer Settings;

	private bool isVisible = false;

	public override void _Ready(){
		Visible = isVisible;

		ContinueButton = GetNode<Button>("PauseMenu/MainOptionsContainer/VBoxContainer/ContinueButton");
		SaveButton = GetNode<Button>("PauseMenu/MainOptionsContainer/VBoxContainer/SaveButton");
		SettingsButton = GetNode<Button>("PauseMenu/MainOptionsContainer/VBoxContainer/SettingsButton");
		ExitButton = GetNode<Button>("PauseMenu/MainOptionsContainer/VBoxContainer/ExitButton");

		MainOptions = GetNode<MarginContainer>("PauseMenu/MainOptionsContainer");
		Settings = GetNode<MarginContainer>("PauseMenu/SettingsContainer");
	}

	public override void _Process(double delta){
		if(Input.IsActionJustPressed("Pause")){
			Visible = !isVisible;
		}
	}

	public void OnContinueButtonPressed(){
		this.Visible = false;
	}

	public void OnSaveButtonPressed(){
		EmitSignal(SignalName.SaveProgress);
	}

	public void OnSettingsButtonPressed(){
		MainOptions.Visible = false;
		Settings.Visible = true;
	}

	public void OnSettingsContainerOnGoBackClicked(){
		MainOptions.Visible = true;
		Settings.Visible = false;
	}

	public void OnExitButtonPressed(){
		OnSaveButtonPressed();
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}
}
