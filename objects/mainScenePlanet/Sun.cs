using Godot;
using System;

public partial class Sun : Node3D{
	DataReader dataReader;
	[Signal] public delegate void TheSunWasClickedEventHandler();

	private Area3D clickablaArea;

	private Node CoronaTrashBinsPlace;
	private PackedScene CTBScene = GD.Load<PackedScene>("res://objects/tools/CoronaTrashBin.tscn");
	private int amountOfCTBs = 0;

	public override void _Ready(){
		dataReader = GetNode<DataReader>("/root/DataReader");
		clickablaArea = GetNode<Area3D>("ClickableArea");
		// CoronaTrashBinsPlace = GetNode<Node>("CoronaTrashBins");
		// amountOfCTBs = Int32.Parse(dataReader.ChosenSlot["CoronaTrashBin"]);

		// CreateCTBs();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		RotateY(-0.1f * (float)delta);
		
	}

	private void OnArea3dInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx){
		if(@event is InputEventMouseButton b && b.Pressed && (b.ButtonIndex == MouseButton.Left || b.ButtonIndex == MouseButton.Right)){
			EmitSignal(SignalName.TheSunWasClicked);
		}
	}

	// private void OnUiSunUpdate(){

	// }

	// private void CreateCTBs(){
	// 	for(int i = 0; i < amountOfCTBs; i++){
	// 		var CTBInstance = CTBScene.Instantiate();
			
	// 		AddChild(CTBInstance);
	// 	}
	// }
}
