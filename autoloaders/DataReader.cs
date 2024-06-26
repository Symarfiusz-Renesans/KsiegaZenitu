using Godot;
using System;
using System.Collections.Generic;

public partial class DataReader : Node{
	public enum FileTypes{
		General,
		SaveSlot1,
		SaveSlot2,
		SaveSlot3
	}
	
	private FileAccess dataFile;
	public Dictionary<string, string> GeneralDataStorage; 
	public Dictionary<string, string> Slot1DataStorage; 
	public Dictionary<string, string> Slot2DataStorage; 
	public Dictionary<string, string> Slot3DataStorage; 

	public Dictionary<string, string> ChosenSlot;
	public FileTypes ChosenSlotId;

	public override void _Ready(){
		GeneralDataStorage = ReadData(FileTypes.General);
		Slot1DataStorage = ReadData(FileTypes.SaveSlot1);
		Slot2DataStorage = ReadData(FileTypes.SaveSlot2);
		Slot3DataStorage = ReadData(FileTypes.SaveSlot3);
	}

	public Dictionary<string, string> ReadData(FileTypes fileType){
		Dictionary<string, string> dataStorage = new Dictionary<string, string>();
			
		ConnectFile(fileType, FileAccess.ModeFlags.Read);
		GD.Print(dataFile);

		while(!dataFile.EofReached()){
			string line = dataFile.GetLine();
			string[] divide = line.Split(": ");

			if(divide.Length != 2) break;
			dataStorage.Add(divide[0], divide[1]);
		}
		dataFile.Close();

		return dataStorage;
	}

	public bool SaveData(string text){
		try{	
			dataFile.StoreString(text);
			dataFile.Close();
			return true;
		} catch (NullReferenceException){
			GD.Print("przeszło");
			ConnectFile(ChosenSlotId, FileAccess.ModeFlags.Write);
			return false;
		}
	}

	public void ChangeData(string inWhat, string toWhat, FileTypes toWhichFile){
		string text = "";
		Dictionary<string, string> dataStorage = null;

		switch(toWhichFile){
			case FileTypes.General:{
				GeneralDataStorage = ReadData(FileTypes.General); 
				dataStorage = GeneralDataStorage;
				break;
			}
			case FileTypes.SaveSlot1:{
				Slot1DataStorage = ReadData(FileTypes.SaveSlot1);
				dataStorage = Slot1DataStorage;
				break;
			}
			case FileTypes.SaveSlot2:{
				Slot2DataStorage = ReadData(FileTypes.SaveSlot2);
				dataStorage = Slot2DataStorage;
				break;
			}
			case FileTypes.SaveSlot3:{
				Slot3DataStorage = ReadData(FileTypes.SaveSlot3);
				dataStorage = Slot3DataStorage;
				break;
			}
		}

		foreach(KeyValuePair<string,string> entry in dataStorage){
			if(inWhat == entry.Key){
				text += entry.Key + ": " + toWhat +"\n";
			} else {
				text += entry.Key + ": " + entry.Value+"\n";
			}
		}
		ConnectFile(toWhichFile, FileAccess.ModeFlags.Write);
		if(SaveData(text)) ChosenSlot = ReadData(toWhichFile);
	}

	void ConnectFile(FileTypes toWhichFile, FileAccess.ModeFlags action){
		switch (toWhichFile){
			case FileTypes.General:{
				dataFile = FileAccess.Open("res://data/generalData.txt", action);
				break;
			}
			case FileTypes.SaveSlot1:{
				dataFile = FileAccess.Open("res://data/saveSlot1Data.txt", action);
				break;
			}
			case FileTypes.SaveSlot2:{
				dataFile = FileAccess.Open("res://data/saveSlot2Data.txt", action);
				break;
			}
			case FileTypes.SaveSlot3:{
				dataFile = FileAccess.Open("res://data/saveSlot3Data.txt", action);
				break;
			}
		}
	}

	public void ChooseSlot(FileTypes fileTypes){
		switch(fileTypes){
			case FileTypes.SaveSlot1:{
				ChosenSlot = Slot1DataStorage;
				break;
			}
			case FileTypes.SaveSlot2:{
				ChosenSlot = Slot2DataStorage;
				break;
			}
			case FileTypes.SaveSlot3:{
				ChosenSlot = Slot3DataStorage;
				break;
			}
		}
		ChosenSlotId = fileTypes;
	}
}
