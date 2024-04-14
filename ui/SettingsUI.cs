using Godot;
using System;

public partial class SettingsUI : MarginContainer{

	[Signal] public delegate void onGoBackClickedEventHandler();

	DataReader dataReader;
	private HSlider MaASlider;
	private HSlider MuASlider;
	private HSlider VASlider;
	private Label ChosenLanguage;
	private int ChosenLanguageId;
	private enum AvailableLanguage{
		Polski = 0,
		English = 1,
		Deutsch = 2,
		Italiana = 3
	}
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		MaASlider = GetNode<HSlider>("Settings/MasterAudio/MaASlider");
		MuASlider = GetNode<HSlider>("Settings/MusicAudio/MuASlider");
		VASlider = GetNode<HSlider>("Settings/VFXAudio/VASlider");
		ChosenLanguage = GetNode<Label>("Settings/Language/Language");

		MaASlider.Value = float.Parse(dataReader.GeneralDataStorage["masterAudio"]);
		MuASlider.Value = float.Parse(dataReader.GeneralDataStorage["musicAudio"]);
		VASlider.Value = float.Parse(dataReader.GeneralDataStorage["vfxAudio"]);

		OnMuASliderValueChanged((float)MuASlider.Value);
		OnMaASliderValueChanged((float)MaASlider.Value);
		OnVASliderValueChanged((float)VASlider.Value);

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

	public void OnGoBackPressed(){

		EmitSignal(SignalName.onGoBackClicked);

	}
}
