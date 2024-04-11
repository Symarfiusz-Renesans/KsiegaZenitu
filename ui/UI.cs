using Godot;
using System;
using System.ComponentModel;

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
	private long amountOfMoney = 0;
	private int amountOfTrashBagsToBeSent = 0;
	private int ClicksPower = 1;
	private int CostOfALaunch = 0;
	private int ProfitOfATrashBag = 0;
	private int CapacityOfARocket = 0;


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
		amountOfMoney = long.Parse(dataReader.ChosenSlot["Money"]);
		amountOfTrashBagsToBeSent = Int32.Parse(dataReader.ChosenSlot["AmountOfTrashBags"]);
		ClicksPower = Int32.Parse(dataReader.ChosenSlot["PowerOfClick"]);
		CostOfALaunch = Int32.Parse(dataReader.ChosenSlot["CostOfALaunch"]);
		ProfitOfATrashBag = Int32.Parse(dataReader.ChosenSlot["ProfitOfATrashBag"]);
		CapacityOfARocket = Int32.Parse(dataReader.ChosenSlot["CapacityOfARocket"]);

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
				informationAboutClicks.Visible = true;
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
				break;
			}
			case 90:{
				GameplayInfo.Visible = true;
				Shop.Visible = false;
				informationAboutClicks.Visible = false;
				break;
			}
			case 180:{
				GameplayInfo.Visible = false;
				Shop.Visible = false;
				informationAboutClicks.Visible = true;
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
				break;
			}
			case 270:{
				Shop.Visible = true;
				GameplayInfo.Visible = false;
				informationAboutClicks.Visible = false;
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
		if(amountOfTrashBagsToBeSent != 0){
			amountOfClicksOnTheSun+= ClicksPower;
			amountOfTrashBagsToBeSent -= ClicksPower;
			amountOfMoney += ClicksPower*ProfitOfATrashBag;
			informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
			informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
		} else {
			SendWarning("You've already sent all launched trash! Launch more!");
		}
	}

	private void OnTheEarthWasClicked(){
		if(amountOfMoney != 0){
			amountOfClicksOnTheEarth+= ClicksPower;
			amountOfTrashBagsToBeSent += ClicksPower*CapacityOfARocket;
			amountOfMoney -= ClicksPower*CostOfALaunch;
			informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
			informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
		} else {
			SendWarning("You're broke! Burn some trash!");
		}
	}

	private void SendWarning(string Message){
		Timer WarningsTime = GetNode<Timer>("HBoxContainer/InfoContainer/Warning/WarningsTime");
		WarningsTime.Start();
		informationAboutWarnings.Visible = true;
		informationAboutWarnings.Text = "You've already burnt all launched trash! Launch more!";
	}
	private void OnWarningsTimeTimeout(){
		informationAboutWarnings.Visible = false;
		informationAboutWarnings.Text = "";
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
		ChangeVariables();
	}

	public string MoneySymbols(long Money){
		const double thousand = 1000;
		const double million = thousand*thousand;
		const double billion = million*thousand;
		const double trillion = billion*thousand;

		if(Money/trillion >= 1) return (Money/trillion).ToString("0.00")+"T";
		else if (Money/billion >= 1) return (Money/billion).ToString("0.00")+"B";
		else if (Money/million >= 1) return (Money/million).ToString("0.00")+"M";
		else if(Money/thousand >= 1) return (Money/thousand).ToString("0.00")+"K";
		else return (Money/thousand).ToString("0.0");
	}
	public void ChangeVariables(){
			dataReader.ChangeData("SunClicked", amountOfClicksOnTheSun.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("EarthClicked", amountOfClicksOnTheEarth.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("Money", amountOfMoney.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("AmountOfTrashBags", amountOfTrashBagsToBeSent.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("PowerOfClick", ClicksPower.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("CostOfALaunch", CostOfALaunch.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("ProfitOfATrashBag", ProfitOfATrashBag.ToString(), dataReader.ChosenSlotId);
	}
	public void OnThingBought(bool wasBought){
		if(wasBought){		
			amountOfMoney = long.Parse(dataReader.ChosenSlot["Money"]);
			informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
		} else {
			SendWarning("You don't have enough money!");
		}
	}
}
