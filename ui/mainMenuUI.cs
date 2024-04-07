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
	private Label ChosenLanguage;
	private enum AvailableLanguage{
		Polski = 0,
		English = 1,
		Deutsch = 2,
		Italiana = 3
	}
	private int ChosenLanguageId;
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		TitleCard = GetNode<VBoxContainer>("MainMenu/TitleCard");
		SaveSlots = GetNode<VBoxContainer>("MainMenu/SaveSlots");
		Settings = GetNode<MarginContainer>("MainMenu/SettingsContainer");
		MaASlider = GetNode<HSlider>("MainMenu/SettingsContainer/Settings/MasterAudio/MaASlider");
		MuASlider = GetNode<HSlider>("MainMenu/SettingsContainer/Settings/MusicAudio/MuASlider");
		VASlider = GetNode<HSlider>("MainMenu/SettingsContainer/Settings/VFXAudio/VASlider");
		ChosenLanguage = GetNode<Label>("MainMenu/SettingsContainer/Settings/Language/Language");

		MaASlider.Value = float.Parse(dataReader.GeneralDataStorage["masterAudio"]);
		MuASlider.Value = float.Parse(dataReader.GeneralDataStorage["musicAudio"]);
		VASlider.Value = float.Parse(dataReader.GeneralDataStorage["vfxAudio"]);

		switch (dataReader.GeneralDataStorage["defaultLanguage"]){
			case "pl":
				ChosenLanguage.Text = AvailableLanguage.Polski.ToString();
				ChosenLanguageId = 0;
				break;
			case "en":
				ChosenLanguage.Text = AvailableLanguage.English.ToString();
				ChosenLanguageId = 1;
				break;
			case "de":
				ChosenLanguage.Text = AvailableLanguage.Deutsch.ToString();
				ChosenLanguageId = 2;
				break;
			case "it":
				ChosenLanguage.Text = AvailableLanguage.Italiana.ToString();
				ChosenLanguageId = 3;
				break;
		}
		TranslationServer.SetLocale(dataReader.GeneralDataStorage["defaultLanguage"]);
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
		GD.Print("Work in progress!");
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
	public void OnPreviousLanguagePressed(){
		GD.Print(((AvailableLanguage)ChosenLanguageId).ToString());
		if(ChosenLanguageId <= 0) ChosenLanguageId = Enum.GetNames(typeof(AvailableLanguage)).Length-1;
		else ChosenLanguageId--;
		GD.Print(ChosenLanguageId);

		ChosenLanguage.Text = ((AvailableLanguage)ChosenLanguageId).ToString();
		setLanguage();
	}
	public void OnNextLanguagePressed(){
		GD.Print(Enum.GetNames(typeof(AvailableLanguage)).Length-1);
		if(ChosenLanguageId >= (Enum.GetNames(typeof(AvailableLanguage)).Length-1)) ChosenLanguageId = 0;
		else ChosenLanguageId++;
		GD.Print(ChosenLanguageId);

		ChosenLanguage.Text = ((AvailableLanguage)ChosenLanguageId).ToString();
		setLanguage();
	}
	public void toTheMainScene(){
		GetTree().ChangeSceneToFile("res://scenes/MainScene.tscn");
	}
	private void setLanguage(){

		string ChosenLanguage = null;

		switch((AvailableLanguage)ChosenLanguageId){
			case AvailableLanguage.Polski:
				ChosenLanguage = "pl";
				break;
			case AvailableLanguage.English:
				ChosenLanguage = "en";
				break;
			case AvailableLanguage.Deutsch:
				ChosenLanguage = "de";
				break;
			case AvailableLanguage.Italiana:
				ChosenLanguage = "it";
				break;
		}

		TranslationServer.SetLocale(ChosenLanguage);
		dataReader.ChangeData("defaultLanguage", ChosenLanguage, DataReader.FileTypes.General);
				
	}
}
