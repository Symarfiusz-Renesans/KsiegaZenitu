using Godot;
using System;

public partial class UI : CanvasLayer
{
	[Signal] public delegate void rotateCameraEventHandler(float angle = 0);

	DataReader dataReader;

	MarginContainer GameplayInfo;
	MarginContainer Shop;

	Label informationAboutClicks;
	Label informationAboutMoney;
	private int amountOfClicksOnTheSun = 0;
	private int amountOfClicksOnTheEarth = 0;
	private int amountOfMoney = 0;
	private float angle = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");

		GameplayInfo = GetNode<MarginContainer>("HBoxContainer/InfoContainer/GameplayInfo");
		Shop = GetNode<MarginContainer>("HBoxContainer/InfoContainer/Shop");
		
		amountOfClicksOnTheSun = Int32.Parse(dataReader.ChosenSlot["sunClicked"]);
		amountOfClicksOnTheEarth = Int32.Parse(dataReader.ChosenSlot["EarthClicked"]);
		//amountOfMoney = Int32.Parse(dataReader.ChosenSlot["money"]);

		informationAboutClicks = this.GetNode<Label>("HBoxContainer/InfoContainer/DataInfo/Clicks");
		informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
		informationAboutMoney = this.GetNode<Label>("HBoxContainer/InfoContainer/DataInfo/Money");
		informationAboutMoney.Text = "Money: "+amountOfMoney;
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
		GD.Print(amountOfClicksOnTheEarth);
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
			dataReader.ChangeData("sunClicked", amountOfClicksOnTheSun.ToString(), dataReader.ChosenSlotId);
			dataReader.ChangeData("EarthClicked", amountOfClicksOnTheEarth.ToString(), dataReader.ChosenSlotId);
        	GetTree().Quit();
		}
	}
}
