using Godot;
using System;

public partial class shopOfferUI : MarginContainer{
	[Signal] public delegate void OnThingBoughtEventHandler(bool wasBought);

	[Export] private string ProductsName;
	[Export] private string StorageName;
	[Export] private string StorageCostName;
	[Export] private string Type;
	[Export] private string Action;
	[Export] private CompressedTexture2D Image;

	DataReader dataReader;
	int amountOfThings = 0;
	int costOfThing = 0;

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
			CostLabel.Text = costOfThing.ToString();
			textureRect.Texture = Image;
			if(Type == "tool") NumberOfOwned.Text = amountOfThings.ToString();
			else NumberOfOwned.Text = "";
		}
	}

	public void OnPessed(){
		if(Int32.Parse(dataReader.ChosenSlot["Money"]) >= costOfThing){
			dataReader.ChangeData(StorageName, (amountOfThings + 1).ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("Money", (Int32.Parse(dataReader.ChosenSlot["Money"])-costOfThing).ToString(), dataReader.ChosenSlotId);
			if(Type == "tool"){
				NumberOfOwned.Text = (amountOfThings + 1).ToString();
				dataReader.ChangeData(StorageCostName, (costOfThing+costOfThing/20).ToString(), dataReader.ChosenSlotId);
				CostLabel.Text = costOfThing+costOfThing/20 + "$";
			} else Visible = false;
			ReloadVariables();
			EmitSignal(SignalName.OnThingBought, true);
		} else {
			EmitSignal(SignalName.OnThingBought, false);
		}
	}

	public void ReloadVariables(){
		costOfThing = Int32.Parse(dataReader.ChosenSlot[StorageCostName]);
		amountOfThings = Int32.Parse(dataReader.ChosenSlot[StorageName]);
	}
}
