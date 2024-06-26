using Godot;
using System;
using System.ComponentModel;

public partial class UI : CanvasLayer
{
	[Signal] public delegate void rotateCameraEventHandler(float angle = 0);
	[Signal] public delegate void reloadToolsEventHandler(string Name);
	//[Signal] public delegate void SunUpdateEventHandler();


	DataReader dataReader;
	//Loaded Nodes
	MarginContainer GameplayInfo;
	MarginContainer Shop;

	Label informationAboutClicks;
	Label informationAboutMoney;
	Label informationAboutWarnings;
	//Loaded variables
	private int amountOfClicksOnTheSun = 0;
	private int amountOfClicksOnTheEarth = 0;
	private long amountOfMoney = 0;
	private int amountOfTrashBagsToBeSent = 0;
	private int ManualClicksPower = 1;
	private int AutomaticalClicksPower = 1;
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
		
		ReloadVariables();

		informationAboutClicks = this.GetNode<Label>("HBoxContainer/InfoContainer/DataInfo/Clicks");
		informationAboutClicks.Text = Tr("CLICKS")+": "+amountOfClicksOnTheSun;
		informationAboutMoney = this.GetNode<Label>("HBoxContainer/InfoContainer/DataInfo/Money");
		informationAboutMoney.Text = Tr("MONEY")+": "+MoneySymbols(amountOfMoney);
		informationAboutWarnings = GetNode<Label>("HBoxContainer/InfoContainer/Warning");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		informationAboutClicks.Visible = false;
		GameplayInfo.Visible = false;
		
		switch(angle){
			case 0:{
				Shop.Visible = false;
				informationAboutClicks.Visible = true;
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
				break;
			}
			case 90:{
				Shop.Visible = true;
				break;
			}
			case 180:{
				Shop.Visible = false;
				informationAboutClicks.Visible = true;
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
				break;
			}
			case 270:{
				GameplayInfo.Visible = true;
				break;
			}
		}
	}

	private void OnTheSunWasClicked(){
		bool actionSuccessful = false;
		for(int i = ManualClicksPower; i > 0; i--){
			if(amountOfTrashBagsToBeSent >= i && !actionSuccessful){
				amountOfClicksOnTheSun+= i;
				amountOfTrashBagsToBeSent -= i;
				amountOfMoney += i*ProfitOfATrashBag;
				informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
				actionSuccessful = true;
			} else {
				break;
			}
		}
		if(!actionSuccessful){
			SendWarning("You've already sent all launched trash! Launch more!");
		}
	}

	private void OnTheEarthWasClicked(){
		bool actionSuccessful = false;
		for(int i = ManualClicksPower; i > 0; i--){
			if(amountOfMoney - i*CostOfALaunch >= 0 && !actionSuccessful){
				amountOfClicksOnTheEarth+= i;
				amountOfTrashBagsToBeSent += i*CapacityOfARocket;
				amountOfMoney -= i*CostOfALaunch;
				informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
				actionSuccessful = true;
			} else {
				break;
			}
		}
		if(!actionSuccessful){
			SendWarning("You don't have enough money! Burn some trash!");
		}
	}
	public void OnEarthIsClickedAutomaticly(int power){
		bool actionSuccessful = false;
		for(int i = power; i > 0; i--){
			if(amountOfMoney - i*CostOfALaunch >= 0 && !actionSuccessful){
				amountOfClicksOnTheEarth+= i;
				amountOfTrashBagsToBeSent += i*CapacityOfARocket;
				amountOfMoney -= i*CostOfALaunch;
				informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheEarth;
				actionSuccessful = true;
			} else {
				break;
			}
		}	
	}
	public void OnSunIsClickedAutomaticly(int power){
		bool actionSuccessful = false;
		for(int i = power; i > 0; i--){
			if(amountOfTrashBagsToBeSent >= i && !actionSuccessful){
				amountOfClicksOnTheSun+= i;
				amountOfTrashBagsToBeSent -= i;
				amountOfMoney += i*ProfitOfATrashBag;
				informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
				informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
				actionSuccessful = true;
			} else {
				break;
			}
		}
	}

	private void SendWarning(string Message){
		Timer WarningsTime = GetNode<Timer>("HBoxContainer/InfoContainer/Warning/WarningsTime");
		WarningsTime.Start();
		informationAboutWarnings.Visible = true;
		informationAboutWarnings.Text = Message;
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

	private void OnCamerasAngleChanged(float angle){
		this.angle = angle;	
	}

	public override void _Notification(int what){
    	if (what == NotificationWMCloseRequest){
			ChangeVariables();
        	GetTree().Quit();
		}
	}

	public void OnPauseMenuSaveProgress(){
		ChangeVariables();
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
		else return (Money/thousand).ToString("0.0");
	}
	public void ChangeVariables(){
		dataReader.ChangeData("SunClicked", amountOfClicksOnTheSun.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("EarthClicked", amountOfClicksOnTheEarth.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("Money", amountOfMoney.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("AmountOfTrashBags", amountOfTrashBagsToBeSent.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("PowerOfManualClick", ManualClicksPower.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("PowerOfAutomaticalClick", AutomaticalClicksPower.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("CostOfALaunch", CostOfALaunch.ToString(), dataReader.ChosenSlotId);
		dataReader.ChangeData("ProfitOfATrashBag", ProfitOfATrashBag.ToString(), dataReader.ChosenSlotId);
	}
	public void ReloadVariables(){
		amountOfClicksOnTheSun = Int32.Parse(dataReader.ChosenSlot["SunClicked"]);
		amountOfClicksOnTheEarth = Int32.Parse(dataReader.ChosenSlot["EarthClicked"]);
		amountOfMoney = long.Parse(dataReader.ChosenSlot["Money"]);
		amountOfTrashBagsToBeSent = Int32.Parse(dataReader.ChosenSlot["AmountOfTrashBags"]);
		ManualClicksPower = Int32.Parse(dataReader.ChosenSlot["PowerOfManualClick"]);
		AutomaticalClicksPower = Int32.Parse(dataReader.ChosenSlot["PowerOfAutomaticalClick"]);
		CostOfALaunch = Int32.Parse(dataReader.ChosenSlot["CostOfALaunch"]);
		ProfitOfATrashBag = Int32.Parse(dataReader.ChosenSlot["ProfitOfATrashBag"]);
		CapacityOfARocket = Int32.Parse(dataReader.ChosenSlot["CapacityOfARocket"]);
	}
	public void OnThingBought(bool wasBought, string Name){
		if(wasBought){		
			amountOfMoney = long.Parse(dataReader.ChosenSlot["Money"]);
			informationAboutMoney.Text = "Money: "+MoneySymbols(amountOfMoney);
			ReloadVariables();
			EmitSignal(SignalName.reloadTools, Name);
		} else {
			if (Name == "noMoney") SendWarning("You don't have enough money!");
			else SendWarning("You won't have enough money for another launch!");
		}
		// if(Name == "CoronaTrashBin"){
		// 	EmitSignal(SignalName.SunUpdate);
		// }
	}
	public void OnBeforeThingIsBought(){
		ChangeVariables();
	}
}
