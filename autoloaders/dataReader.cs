using Godot;
using System;
using System.Collections.Generic;

public partial class dataReader : Node
{
	
	private FileAccess dataFile;
	public Dictionary<string, string> GeneralDataStorage; 
	public override void _Ready(){
		dataFile = FileAccess.Open("res://data/generalData.txt", FileAccess.ModeFlags.Read);
		GeneralDataStorage = ReadData(dataFile);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Dictionary<string, string> ReadData(FileAccess file){
		Dictionary<string, string> dataStorage = new Dictionary<string, string>();
		
		while(!file.EofReached()){
			string line = file.GetLine();
			string[] divide = line.Split(": ");
			if(divide.Length != 2) break;
			dataStorage.Add(divide[0], divide[1]);
		}
		return dataStorage;
	}
}
