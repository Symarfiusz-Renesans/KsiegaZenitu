using Godot;
using System;

public partial class UI : CanvasLayer
{
	[Signal] public delegate void rotateCameraEventHandler(float angle = 0);

	DataReader dataReader;
	//Loaded Nodes
	MarginContainer GameplayInfo;
	MarginContainer Shop;

	Label informationAboutClicks;
	Label informationAboutMoney;
	Label informationAboutWarnings;

	Timer AutomaticSave;
	//Loaded variables
	private int amountOfClicksOnTheSun = 0;
	private int amountOfClicksOnTheEarth = 0;
	private int amountOfMoney = 0;
	private int amountOfTrashBagsToBeSent = 0;

	//The angle of a screen
	private float angle = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		GameplayInfo = GetNode<MarginContainer>("HBoxContainer/InfoContainer/GameplayInfo");
		Shop = GetNode<MarginContainer>("HBoxContainer/InfoContainer/Shop");

		AutomaticSave = GetNode<Timer>("AutomaticSave");
		
		amountOfClicksOnTheSun = Int32.Parse(dataReader.ChosenSlot["SunClicked"]);
		amountOfClicksOnTheEarth = Int32.Parse(dataReader.ChosenSlot["EarthClicked"]);
		amountOfMoney = Int32.Parse(dataReader.ChosenSlot["Money"]);
		amountOfTrashBagsToBeSent = Int32.Parse((dataReader.ChosenSlot["AmountOfTrashBags"]));

		GD.Print(amountOfMoney);

		informationAboutClicks = this.GetNode<Label>("HBoxContainer/InfoContainer/DataInfo/Clicks");
		informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
		informationAboutMoney = this.GetNode<Label>("HBoxContainer/InfoContainer/DataInfo/Money");
		informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
		informationAboutWarnings = GetNode<Label>("HBoxContainer/InfoContainer/Warning");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		switch(angle){
			case 0:{
				GameplayInfo.Visible = false;
				Shop.Visible = false;
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
				break;
			}
			case 90:{
				GameplayInfo.Visible = true;
				Shop.Visible = false;
				break;
			}
			case 180:{
				GameplayInfo.Visible = false;
				Shop.Visible = false;
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
				break;
			}
			case 270:{
				Shop.Visible = true;
				GameplayInfo.Visible = false;
				break;
			}
		}

		if(angle != 90){
			GameplayInfo.Visible = false;
		} 
		if(angle != 270){
			Shop.Visible = false;
		}
		if(angle == 0 || angle == 180){
			GameplayInfo.Visible = false;
			Shop.Visible = false;
		}
	}

	private void OnTheSunWasClicked(){
		amountOfClicksOnTheSun++;
		informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
	}

	private void OnTheEarthWasClicked(){
		amountOfClicksOnTheEarth++;
		informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
	}

	private void OnToTheRightInputEvent(){
		EmitSignal(SignalName.rotateCamera, -90);
	}
	
	private void OnToTheLeftInputEvent(){
		EmitSignal(SignalName.rotateCamera, 90);
	}

	private async void OnCamerasAngleChanged(float angle){
		this.angle = angle;	
	}

	public override void _Notification(int what){
    	if (what == NotificationWMCloseRequest){
			OnPauseMenuSaveProgress();
        	GetTree().Quit();
		}
	}

	public void OnAutomaticSaveTimeout(){
		OnPauseMenuSaveProgress();
		GD.Print("zapisano!");
	}

	public void OnPauseMenuSaveProgress(){
			dataReader.ChangeData("SunClicked", amountOfClicksOnTheSun.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("EarthClicked", amountOfClicksOnTheEarth.ToString(), dataReader.ChosenSlotId);
	}

	public string MoneySymbols(int Money){
		const long thousand = 1000;
		const long million = thousand*thousand;
		const long billion = million*thousand;
		const long trillion = billion*thousand;

		if(Money/trillion >= 1) return (Money/thousand)+"T";
		else if (Money/billion >= 1) return (Money/billion)+"B";
		else if (Money/million >= 1) return (Money/billion)+"M";
		else if(Money/thousand >= 1) return (Money/thousand)+"K";
		else return (Money/thousand).ToString();
	}
}
