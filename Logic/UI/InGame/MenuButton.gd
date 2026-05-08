extends Button

@export var menu: Control

func _on_pressed():
	menu.visible = not menu.visible 
