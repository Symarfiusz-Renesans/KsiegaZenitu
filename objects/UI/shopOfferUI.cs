using Godot;
using System;

public partial class shopOfferUI : MarginContainer{
	[Signal] public delegate void OnThingBoughtEventHandler();

	[Export] private string ProductsName;
	[Export] private string StorageName;
	[Export] private string StorageCostName;
	[Export] private string Type;
	[Export] private int Value;
	[Export] private string Action;
	[Export] private Image Image;

	DataReader dataReader;

	private Label NameLabel;
	private Label CostLabel;
	private RichTextLabel ActionLabel;
	public Label NumberOfOwned;
	private ColorRect colorRect;
	private TextureRect textureRect;
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		NameLabel = GetNode<Label>("HBoxContainer/VBoxContainer/HBoxContainer/Name");
		CostLabel = GetNode<Label>("HBoxContainer/VBoxContainer/HBoxContainer/Cost");
		ActionLabel = GetNode<RichTextLabel>("HBoxContainer/VBoxContainer/Action");
		colorRect = GetNode<ColorRect>("ColorRect");
		NumberOfOwned = GetNode<Label>("HBoxContainer/Number");
		textureRect = GetNode<TextureRect>("HBoxContainer/TextureRect");

		NameLabel.Text = ProductsName;
		ActionLabel.Text = Action;
		CostLabel.Text = dataReader.ChosenSlot[StorageCostName];
		if(Type == "tool") NumberOfOwned.Text = dataReader.ChosenSlot[StorageName];
		else NumberOfOwned.Text = "";
	}

	public void OnPessed(){
		int amountOfThings = Int32.Parse(dataReader.ChosenSlot[StorageName]);
		int costOfThing = Int32.Parse(dataReader.ChosenSlot[StorageCostName]);

		EmitSignal(SignalName.OnThingBought);
		dataReader.ChangeData(StorageName, (amountOfThings + Value).ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("Money", (Int32.Parse(dataReader.ChosenSlot[StorageCostName])-costOfThing).ToString(), dataReader.ChosenSlotId);
		if(Type == "tool"){
			GD.Print("klil");
			NumberOfOwned.Text = (amountOfThings + Value).ToString();
			dataReader.ChangeData(StorageCostName, (costOfThing+costOfThing/20).ToString(), dataReader.ChosenSlotId);
			CostLabel.Text = costOfThing+costOfThing/20 + "$";
		} else Visible = false;
	}
}
