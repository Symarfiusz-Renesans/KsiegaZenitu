using Godot;
using System;

public partial class MainMenuUI : CanvasLayer
{

	DataReader dataReader;
	private VBoxContainer TitleCard;
	private VBoxContainer SaveSlots;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		TitleCard = GetNode<VBoxContainer>("MainMenu/TitleCard");
		SaveSlots = GetNode<VBoxContainer>("MainMenu/SaveSlots");

		if (dataReader.GeneralDataStorage["defaultLanguage"] == "notSpec"){
			GetTree().ChangeSceneToFile("res://objects/terminals/terminal.tscn");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayPressed(){
		TitleCard.Visible = false;
		SaveSlots.Visible = true;
	}
	public void OnSettingsPressed(){
		
	}
	public void OnCreatorsPressed(){

	}
	public void OnExitPressed(){
		GetTree().Quit();
	}
	public void OnGoBackPressed(){
		TitleCard.Visible = true;
		SaveSlots.Visible = false;
	}
	public void OnSaveSlot1Pressed(){
		dataReader.ChooseSlot(DataReader.FileTypes.SaveSlot1);
		toTheMainScene();
	}
	public void OnSaveSlot2Pressed(){
		dataReader.ChooseSlot(DataReader.FileTypes.SaveSlot1);
		toTheMainScene();
	}
	public void OnSaveSlot3Pressed(){
		dataReader.ChooseSlot(DataReader.FileTypes.SaveSlot1);
		toTheMainScene();
	}
	public void toTheMainScene(){
		GetTree().ChangeSceneToFile("res://scenes/mainScene.tscn");
	}
}
