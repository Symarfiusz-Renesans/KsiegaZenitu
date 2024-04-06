using Godot;
using System;

public partial class MainMenuUI : CanvasLayer
{

	DataReader dataReader;
	private VBoxContainer TitleCard;
	private VBoxContainer SaveSlots;
	private MarginContainer Settings;
	private HSlider MaASlider;
	private HSlider MuASlider;
	private HSlider VASlider;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		TitleCard = GetNode<VBoxContainer>("MainMenu/TitleCard");
		SaveSlots = GetNode<VBoxContainer>("MainMenu/SaveSlots");
		Settings = GetNode<MarginContainer>("MainMenu/SettingsContainer");
		MaASlider = GetNode<HSlider>("MainMenu/SettingsContainer/Settings/MasterAudio/MaASlider");
		MuASlider = GetNode<HSlider>("MainMenu/SettingsContainer/Settings/MusicAudio/MuASlider");
		VASlider = GetNode<HSlider>("MainMenu/SettingsContainer/Settings/VFXAudio/VASlider");

		GD.Print(float.Parse(dataReader.GeneralDataStorage["masterAudio"]));
		MaASlider.Value = float.Parse(dataReader.GeneralDataStorage["masterAudio"]);
		MuASlider.Value = float.Parse(dataReader.GeneralDataStorage["musicAudio"]);
		VASlider.Value = float.Parse(dataReader.GeneralDataStorage["vfxAudio"]);

		if (dataReader.GeneralDataStorage["defaultLanguage"] == "notSpec"){
			GetTree().ChangeSceneToFile("res://objects/terminals/terminal.tscn");
		}
	}

	public void OnPlayPressed(){
		TitleCard.Visible = false;
		SaveSlots.Visible = true;
	}
	public void OnSettingsPressed(){
		TitleCard.Visible = false;
		Settings.Visible = true;		
	}
	public void OnCreatorsPressed(){

	}
	public void OnExitPressed(){
		GetTree().Quit();
	}
	public void OnGoBackPressed(){
		Settings.Visible = false;
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
	public void OnMuASliderValueChanged(float Value){
		dataReader.ChangeData("musicAudio", Value.ToString("0.00"), DataReader.FileTypes.General);
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(Value));
	}
	public void OnMaASliderValueChanged(float Value){
		dataReader.ChangeData("masterAudio", Value.ToString("0.00"), DataReader.FileTypes.General);
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(Value));
	}
	public void OnVASliderValueChanged(float Value){
		dataReader.ChangeData("vfxAudio", Value.ToString("0.00"), DataReader.FileTypes.General);
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("VFX"), Mathf.LinearToDb(Value));
	}
	public void toTheMainScene(){
		GetTree().ChangeSceneToFile("res://scenes/mainScene.tscn");
	}
}
