using Godot;
using System;

public partial class UI : CanvasLayer
{
	[Signal] public delegate void rotateCameraEventHandler(float angle = 0);

	DataReader dataReader;

	Label informationAboutClicks;
	private int amountOfClicksOnTheSun = 0;
	private int amountOfClicksOnTheEarth = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		dataReader = (DataReader)GetNode("/root/DataReader");
		
		amountOfClicksOnTheSun = Int32.Parse(dataReader.ChosenSlot["sunClicked"]);
		amountOfClicksOnTheEarth = Int32.Parse(dataReader.ChosenSlot["EarthClicked"]);

		informationAboutClicks = this.GetNode<Label>("HBoxContainer/clicks");
		informationAboutClicks.Text = "Clicks: "+amountOfClicksOnTheSun;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
}
