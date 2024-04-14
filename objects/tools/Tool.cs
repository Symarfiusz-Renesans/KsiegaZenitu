using Godot;
using System;

public partial class Tool : Node
{
	[Signal] public delegate void SunIsClickedAutomaticlyEventHandler(int power);
	[Signal] public delegate void EarthIsClickedAutomaticlyEventHandler(int power);
	DataReader dataReader;
	[Export] string ToolsName;
	[Export] string ToolsSpeedName;
	[Export] string ToolsPowerName;
	[Export] string ToolsLocation;

	private int amountOfTools = 0;
	private int speedOfTools = 10;
	private int powerOfTools = 1;
	private int powerOfAutomaticalClick = 1;
	public override void _Ready(){
		dataReader = GetNode<DataReader>("/root/DataReader");

		ReloadVariables();

		CreateTimers();

	}

	private void ReloadVariables(){
		//dataReader.ChosenSlot =dataReader.ReadData(dataReader.ChosenSlotId);

		amountOfTools = Int32.Parse(dataReader.ChosenSlot[ToolsName]);
		speedOfTools = Int32.Parse(dataReader.ChosenSlot[ToolsSpeedName]);
		powerOfTools = Int32.Parse(dataReader.ChosenSlot[ToolsPowerName]);
		GD.Print(speedOfTools);
		powerOfAutomaticalClick = Int32.Parse(dataReader.ChosenSlot["PowerOfAutomaticalClick"]);
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
		int amountOfClicks = powerOfTools * powerOfAutomaticalClick;
		
		switch(ToolsLocation){
			case "Sun":{
				EmitSignal(SignalName.SunIsClickedAutomaticly, amountOfClicks);
				break;
			}
			case "Earth":{
				EmitSignal(SignalName.EarthIsClickedAutomaticly, amountOfClicks);
				break;
			}
		}
		
	}

	private void OnReloadTools(string Name){
		GD.Print("odebrano");
		GD.Print(Name);
		if(Name == ToolsName || Name=="All"){
			ReloadVariables();
			DeleteTimers();
			CreateTimers();
		}
	}

	
}
