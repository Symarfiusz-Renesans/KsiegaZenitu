using Godot;
using System;

public partial class terminal : Control{

	private string defaultLanguage = "";
	LineEdit commandLine;
	RichTextLabel terminalsComputing;
	public override void _Ready(){
		commandLine = GetNode<LineEdit>("VBoxContainer/commandLine");
		terminalsComputing = GetNode<RichTextLabel>("VBoxContainer/terminalsComputing");
		commandLine.GrabFocus();
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
		if(defaultLanguage == "") ChooseLanguage(commandsBody);
	}

	private void ChooseLanguage(string[] commandsBody){
		GD.Print(commandsBody[0]);
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
			terminalsComputing.Text += "[color=green]> Thank you for cooperation.[/color]\n";
		}
	}


}
