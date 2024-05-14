extends GPUParticles2D

# Called when the node enters the scene tree for the first time.
func _ready():
	if OS.get_model_name() == "Android":
		amount /= 4
