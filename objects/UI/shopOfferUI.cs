using Godot;
using System;

public partial class shopOfferUI : MarginContainer{
	[Signal] public delegate void OnThingBoughtEventHandler(bool wasBought, string name);
	[Signal] public delegate void BeforeThingIsBoughtEventHandler();

	[Export] private string WhatUpgrades;
	[Export] private string ProductsName;
	[Export] private string StorageName;
	[Export] private string StorageCostName;
	[Export] private string Type;
	[Export] private string Action;
	[Export] private int Value;
	[Export] private string UpgradedValue;
	[Export] private CompressedTexture2D Image;

	DataReader dataReader;
	int amountOfThings = 0;
	int amountOfTrash = 0;
	int costOfThing = 0;
	int costOfLaunch = 0;
	long Money = 0;
	private Label NameLabel;
	private Label CostLabel;
	private RichTextLabel ActionLabel;
	public Label NumberOfOwned;
	private ColorRect colorRect;
	private TextureRect textureRect;
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		ReloadVariables();

		if(Type == "upgrade" && amountOfThings == 1){
			Visible = false;
		} else {

			NameLabel = GetNode<Label>("HBoxContainer/VBoxContainer/HBoxContainer/Name");
			CostLabel = GetNode<Label>("HBoxContainer/VBoxContainer/HBoxContainer/Cost");
			ActionLabel = GetNode<RichTextLabel>("HBoxContainer/VBoxContainer/Action");
			colorRect = GetNode<ColorRect>("ColorRect");
			NumberOfOwned = GetNode<Label>("HBoxContainer/Number");
			textureRect = GetNode<TextureRect>("HBoxContainer/TextureRect");

			NameLabel.Text = ProductsName;
			ActionLabel.Text = Action;
			CostLabel.Text = MoneySymbols(costOfThing).ToString();
			textureRect.Texture = Image;
			if(Type == "tool") NumberOfOwned.Text = amountOfThings.ToString();
			else NumberOfOwned.Text = "";
		}
	}

	public void OnPessed(){
		EmitSignal(SignalName.BeforeThingIsBought);
		ReloadVariables();
		GD.Print(Money);
		if((Money >= costOfThing && amountOfTrash > 0)||(Money >= costOfThing+costOfLaunch && amountOfTrash == 0)){
			GD.Print(Money-costOfThing);
			dataReader.ChangeData("Money", (Money-costOfThing).ToString(), dataReader.ChosenSlotId);
			if(Type == "tool"){
				dataReader.ChangeData(StorageName, (amountOfThings + 1).ToString(), dataReader.ChosenSlotId);
				NumberOfOwned.Text = (amountOfThings + 1).ToString();
				dataReader.ChangeData(StorageCostName, (costOfThing+costOfThing/20).ToString(), dataReader.ChosenSlotId);
				CostLabel.Text = MoneySymbols(costOfThing+costOfThing/20).ToString();
			} else {
				Visible = false;
				dataReader.ChangeData(StorageName, 1.ToString(), dataReader.ChosenSlotId);
				dataReader.ChangeData(UpgradedValue, Value.ToString(), dataReader.ChosenSlotId);
			}
			dataReader.ChosenSlot = dataReader.ReadData(dataReader.ChosenSlotId);
			ReloadVariables();
			EmitSignal(SignalName.OnThingBought, true, WhatUpgrades);
		} else {
			EmitSignal(SignalName.OnThingBought, false, ( amountOfTrash > 0)? "noMoney" :"noTrash");
		}
	}

	public void ReloadVariables(){
		costOfThing = Int32.Parse(dataReader.ChosenSlot[StorageCostName]);
		amountOfThings = Int32.Parse(dataReader.ChosenSlot[StorageName]);
		costOfLaunch = Int32.Parse(dataReader.ChosenSlot["CostOfALaunch"]);
		Money = long.Parse(dataReader.ChosenSlot["Money"]);
		amountOfTrash = Int32.Parse(dataReader.ChosenSlot["AmountOfTrashBags"]);
	}

	public string MoneySymbols(long Money){
		const double thousand = 1000;
		const double million = thousand*thousand;
		const double billion = million*thousand;
		const double trillion = billion*thousand;

		if(Money/trillion >= 1) return (Money%trillion == 0) ? (Money/trillion).ToString()+"T" : (Money/trillion).ToString("0.00")+"T";
		else if (Money/billion >= 1) return (Money%billion == 0) ? (Money/billion).ToString()+"B" : (Money/billion).ToString("0.00")+"B";
		else if (Money/million >= 1) return (Money%million == 0) ? (Money/million).ToString()+"M" : (Money/million).ToString("0.00")+"M";
		else if(Money/thousand >= 1) return (Money%thousand == 0) ? (Money/thousand).ToString()+"K" : (Money/thousand).ToString("0.00")+"K";
		else return (Money).ToString();
	}
}
