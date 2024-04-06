using Godot;
using System;

public partial class AudioManager : Node
{
	AudioStreamPlayer MusicPlayer;
	AudioStreamPlayer VFXPlayer;
	public override void _Ready(){
		MusicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
		VFXPlayer = GetNode<AudioStreamPlayer>("VFXPlayer");
	}
	}
}
