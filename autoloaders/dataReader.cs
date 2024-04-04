using Godot;
using System;
using System.Collections.Generic;

public partial class dataReader : Node
{
	public enum FileTypes{
		General,
		SaveSlot1,
		SaveSlot2,
		SaveSlot3
	}
	
	private FileAccess dataFile;
	public Dictionary<string, string> GeneralDataStorage; 
	public Dictionary<string, string> Slot1lDataStorage; 
	public Dictionary<string, string> Slot2DataStorage; 
	public Dictionary<string, string> Slot3DataStorage; 

	public override void _Ready(){
		GeneralDataStorage = ReadData(FileTypes.General);
		Slot1lDataStorage = ReadData(FileTypes.SaveSlot1);
		Slot2DataStorage = ReadData(FileTypes.SaveSlot2);
		Slot3DataStorage = ReadData(FileTypes.SaveSlot3);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Dictionary<string, string> ReadData(FileTypes fileType){
		Dictionary<string, string> dataStorage = new Dictionary<string, string>();
		ConnectFile(fileType);
		
		while(!dataFile.EofReached()){
			string line = dataFile.GetLine();
			string[] divide = line.Split(": ");

			if(divide.Length != 2) break;

			dataStorage.Add(divide[0], divide[1]);
		}
		return dataStorage;
	}

	public void SaveData(string text){
		dataFile.StoreString(text);
		dataFile.Close();
	}

	public void ChangeData(string what, string toWhat, FileTypes toWhichFile){
		string text = "";
		foreach(KeyValuePair<string,string> entry in GeneralDataStorage){
			if(what == entry.Key){
				text += entry.Key + ": " + toWhat +"\n";
			} else {
				text += entry.Key + ": " + entry.Value+"\n";
			}
		}
		GD.Print(text);

		ConnectFile(toWhichFile);
		SaveData(text);
	}

	public void ConnectFile(FileTypes toWhichFile){
		switch (toWhichFile){
			case FileTypes.General:{
				dataFile = FileAccess.Open("res://data/generalData.txt", FileAccess.ModeFlags.Write);
				break;
			}
			case FileTypes.SaveSlot1:{
				dataFile = FileAccess.Open("res://data/saveSlot1Data.txt", FileAccess.ModeFlags.Write);
				break;
			}
			case FileTypes.SaveSlot2:{
				dataFile = FileAccess.Open("res://data/saveSlot2Data.txt", FileAccess.ModeFlags.Write);
				break;
			}
			case FileTypes.SaveSlot3:{
				dataFile = FileAccess.Open("res://data/saveSlot3Data.txt", FileAccess.ModeFlags.Write);
				break;
			}
		}
	}
}
