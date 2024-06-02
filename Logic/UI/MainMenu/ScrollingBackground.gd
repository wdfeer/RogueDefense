extends TextureRect

var speed: float = (0.01 * randf() + 0.005) * (1 if randi_range(0, 1) == 0 else -1)

var offset: Vector2 = Vector2(randf(), 0)

func _process(delta):
	offset.x += delta * speed
	
	var shader: ShaderMaterial = material
	shader.set_shader_parameter("offset", offset)
