using Godot;
using System;

public partial class UI : CanvasLayer
{
	Label information;
	private int amountOfClicks = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		information = this.GetNode<Label>("Label");
		information.Text = "Clicks: 0";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnTheSunWasClicked(){
		amountOfClicks++;
		information.Text = "Clicks:"+amountOfClicks;
	}
}
