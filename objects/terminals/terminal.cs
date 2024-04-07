using Godot;
using System;
using System.Data;

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
	}

	public override void _Process(double delta){
		if(Input.IsActionJustPressed("Enter")){
			string command = commandLine.Text;
			if(command != ""){
				commandLine.Text = "";
				terminalsComputing.Text += "\n"+command+"\n";
				ComputeCommand(command);
			}
		}
	}

	private void ComputeCommand(string command){
		string[] commandsBody = command.StripEdges().ToLower().Split(" ");
		switch(commandsBody[0]){
			case "rsrch":
			case "rs":
				ResearchCommand(commandsBody);
				break;
		}

	}

	private void ResearchCommand(string[] commandsBody){
		if(commandsBody.Length == 1){
			terminalsComputing.Text += "\n[color=red]Command incomplete.\nFor help add hint to the command.[/color]\n";
		} else switch(commandsBody[1]){
			case "unavb":
			case "avb":
			case "fin":
			case "uway":

				break;
		}
	}

	


}
