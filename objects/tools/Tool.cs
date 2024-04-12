using Godot;
using System;

public partial class Tool : Node
{
	[Signal] public delegate void SunIsClickedAutomaticlyEventHandler();
	[Signal] public delegate void EarthIsClickedAutomaticlyEventHandler();
	DataReader dataReader;
	[Export] string ToolsName;
	[Export] string ToolsSpeedName;
	[Export] string ToolsPowerName;
	[Export] string ToolsLocation;

	private int amountOfTools = 0;
	private int speedOfTools = 10;
	private int powerOfTools = 1;
	public override void _Ready(){
		dataReader = GetNode<DataReader>("/root/DataReader");

		ReloadVariables();

		CreateTimers();

	}

	private void ReloadVariables(){
		amountOfTools = Int32.Parse(dataReader.ChosenSlot[ToolsName]);
		speedOfTools = Int32.Parse(dataReader.ChosenSlot[ToolsSpeedName]);
		powerOfTools = Int32.Parse(dataReader.ChosenSlot[ToolsPowerName]);
	}

	private void CreateTimers(){
		for(int i = 0; i < amountOfTools; i++){
            Timer timer = new(){
                Autostart = true,
                WaitTime = speedOfTools
            };
            timer.Timeout += OnTimerTimeout;
			
			AddChild(timer);
		}
	}
	private void DeleteTimers(){
		if(GetChildCount() > 0){
			var children = GetChildren();
			foreach(Node child in children){
				RemoveChild(child);
				child.QueueFree();
			}
		}
	}

	private void OnTimerTimeout(){
		switch(ToolsLocation){
			case "Sun":{
				EmitSignal(SignalName.SunIsClickedAutomaticly);
				break;
			}
		}
		
	}

	private void OnReloadTools(string Name){
		if(Name == ToolsName){
			GD.Print("odebrano");
			ReloadVariables();
			DeleteTimers();
			CreateTimers();
		}
	}

	
}
