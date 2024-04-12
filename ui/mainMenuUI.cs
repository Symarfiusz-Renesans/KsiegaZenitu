using Godot;
using System;

public partial class MainMenuUI : CanvasLayer
{

	DataReader dataReader;
	private VBoxContainer TitleCard;
	private VBoxContainer SaveSlots;
	private MarginContainer Settings;
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		TitleCard = GetNode<VBoxContainer>("MainMenu/TitleCard");
		SaveSlots = GetNode<VBoxContainer>("MainMenu/SaveSlots");
		Settings = GetNode<MarginContainer>("MainMenu/SettingsContainer");
	}

	public void OnPlayPressed(){
		TitleCard.Visible = false;
		SaveSlots.Visible = true;
	}
	public void OnSettingsPressed(){
		TitleCard.Visible = false;
		Settings.Visible = true;		
	}
	public void OnGoBackPressed(){
		TitleCard.Visible = true;
		SaveSlots.Visible = false;
	}
	public void OnSettingsContainerOnGoBackClicked(){
		TitleCard.Visible = true;
		Settings.Visible = false;		
	}
	public void OnCreatorsPressed(){
		GD.Print("Work in progress!");
	}
	public void OnExitPressed(){
		GetTree().Quit();
	}
	public void OnSaveSlot1Pressed(){
		dataReader.ChooseSlot(DataReader.FileTypes.SaveSlot1);
		toTheMainScene();
	}
	public void OnSaveSlot2Pressed(){
		dataReader.ChooseSlot(DataReader.FileTypes.SaveSlot2);
		toTheMainScene();
	}
	public void OnSaveSlot3Pressed(){
		dataReader.ChooseSlot(DataReader.FileTypes.SaveSlot3);
		toTheMainScene();
	}
	public void toTheMainScene(){
		GetTree().ChangeSceneToFile("res://scenes/MainScene.tscn");
	}

}
