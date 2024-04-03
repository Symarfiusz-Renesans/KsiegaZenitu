using Godot;
using System;
using System.Runtime.InteropServices;

public partial class TerminalRealTimeUpdating : Node2D
{
	SubViewport subViewport;
	public override void _Ready(){
		subViewport = GetNode<SubViewport>("SubViewport");
		SaveImage();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		SaveImage();
	}

	private void SaveImage(){
		Image image = subViewport.GetViewport().GetTexture().GetImage();
		string imagePath = "res://2dSprites/terminal.png";
		image.SavePng(imagePath);
	}
}
