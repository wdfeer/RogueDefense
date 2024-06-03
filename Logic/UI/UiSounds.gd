# Taken from https://codeberg.org/Liblast/Liblast (GPL v3) with modifications

# This class scans the scene to and connects signals from UI elements to audio stream playrs it creates
# This is easy and efficient, unlike replacing every UI element and creating dozens of AudioStreamPlayer nodes

extends Node

@export var root_path : NodePath

# create audio player instances
@onready var sounds = {
	&"UI_Hover" : AudioStreamPlayer.new(),
	&"UI_Click" : AudioStreamPlayer.new(),
	}


func _ready() -> void:
	assert(root_path != null, "Empty root path for UI Sounds!")

	# set up audio stream players and load sound files
	for i in sounds.keys():
		sounds[i].stream = load("res://Assets/SFX/" + str(i) + ".wav")
		# assign output mixer bus
		sounds[i].bus = &"SFX"
		# add them to the scene tree
		add_child(sounds[i])

	# connect signals to the method that plays the sounds
	install_sounds(get_node(root_path))


func install_sounds(node: Node) -> void:
	for i in node.get_children():
		if i is Button or i is TextureButton:
			i.mouse_entered.connect( ui_sfx_play.bind(&"UI_Hover") )
			i.pressed.connect( ui_sfx_play.bind(&"UI_Click") )
		elif i is LineEdit:
			i.mouse_entered.connect( ui_sfx_play.bind(&"UI_Hover") )
#			i.text_submitted.connect( ui_sfx_play.bind(&"UI_Accept").unbind(1) )
#			i.text_change_rejected.connect( ui_sfx_play.bind(&"UI_Cancel").unbind(1) )
#			i.text_changed.connect( ui_sfx_play.bind(&"UI_Message").unbind(1) )

		# recursion
		install_sounds(i)


func ui_sfx_play(sound : String) -> void:
	var player: AudioStreamPlayer = sounds[sound]
	if player.is_inside_tree():
		player.play()
