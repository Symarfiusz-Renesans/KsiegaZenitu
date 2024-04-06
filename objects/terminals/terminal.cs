using Godot;
using System;

public partial class Terminal : Control{

	DataReader dataReader;

	private string defaultLanguage = "notSpec";
	LineEdit commandLine;
	RichTextLabel terminalsComputing;
	public override void _Ready(){
		commandLine = GetNode<LineEdit>("VBoxContainer/commandLine");
		terminalsComputing = GetNode<RichTextLabel>("VBoxContainer/terminalsComputing");
		commandLine.GrabFocus();

		dataReader = (DataReader)GetNode("/root/DataReader");
		defaultLanguage = dataReader.GeneralDataStorage["defaultLanguage"];
		if(defaultLanguage != "notSpec"){
			TranslationServer.SetLocale(defaultLanguage);
			GetTree().ChangeSceneToFile("res://scenes/mainMenu.tscn");
		}
	}

	public override void _Process(double delta){
		if(Input.IsActionJustPressed("Enter")){
			string command = commandLine.Text;
			if(command != ""){
				commandLine.Text = "";
				terminalsComputing.Text += "\n"+command;
				ComputeCommand(command);
			}
		}
	}

	private void ComputeCommand(string command){
		string[] commandsBody = command.StripEdges().ToLower().Split(" ");
		if(defaultLanguage == "notSpec") ChooseLanguage(commandsBody);
	}

	private async void ChooseLanguage(string[] commandsBody){
		switch(commandsBody[0]){
			case "pl":{
				terminalsComputing.Text += "\n[color=green]> Język pomyślnie ustawiony![/color]\n";
				TranslationServer.SetLocale(commandsBody[0]);
				defaultLanguage = commandsBody[0];
				break;
			}
			case "en":{
				terminalsComputing.Text += "\n[color=green]> Language successfuly set![/color]\n";
				TranslationServer.SetLocale(commandsBody[0]);
				defaultLanguage = commandsBody[0];
				break;
			}
			case "de":{
				terminalsComputing.Text += "\n[color=green]> Die Sprache wurde erfolgreich eingestellt![/color]\n";
				TranslationServer.SetLocale(commandsBody[0]);
				defaultLanguage = commandsBody[0];
				break;
			}
			case "it":{
				terminalsComputing.Text += "\n[color=green]> La lingua è stata selezionata con successo![/color]\n";
				TranslationServer.SetLocale(commandsBody[0]);
				defaultLanguage = commandsBody[0];
				break;
			}
			default:{
				terminalsComputing.Text += "\n[color=red]> Before anything else, please, choose preffered language.[/color]\n";
				break;
			}
		}
		if (defaultLanguage != ""){
			terminalsComputing.Text += "[color=green]> "+Tr("THANKS_FOR_COOPERATION")+"	[/color]\n";
			GD.Print("przed");
			dataReader.ChangeData("defaultLanguage", defaultLanguage, DataReader.FileTypes.General);
			GD.Print("po");
			await ToSignal(GetTree().CreateTimer(3f), "timeout");
			GetTree().ChangeSceneToFile("res://scenes/mainMenu.tscn");
		}
	}


}
