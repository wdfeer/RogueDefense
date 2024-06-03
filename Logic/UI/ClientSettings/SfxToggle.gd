extends CheckButton

func _on_toggled(toggled_on):
	AudioServer.set_bus_mute(AudioServer.get_bus_index("SFX"), !toggled_on)
