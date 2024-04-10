using Godot;
using System;

public partial class shopOfferUI : MarginContainer{
	[Export] private string ProductsName;
	[Export] private string Description;
	[Export] private string Action;

	private Label NameLabel;
	private RichTextLabel DescriptionLabel;
	private RichTextLabel ActionLabel;
	public override void _Ready(){
		NameLabel = GetNode<Label>("HBoxContainer/VBoxContainer/Name");
		DescriptionLabel = GetNode<RichTextLabel>("HBoxContainer/VBoxContainer/Description");
		ActionLabel = GetNode<RichTextLabel>("HBoxContainer/VBoxContainer/Action");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPessed(){

	}
}
