extends TextureRect

var speed: float = 0.02 * (randf() - 0.5)

var offset: Vector2 = Vector2(randf(), 0)

func _process(delta):
	offset.x += delta * speed
	
	material.set("shader_param/offset", offset)
