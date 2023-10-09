extends Button

@onready var menu: Control = $"../Menu"

func _on_pressed():
	menu.visible = not menu.visible 
