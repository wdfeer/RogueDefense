extends Sprite2D

const ROTATION_SPEED = 3
func _process(delta):
	rotate(delta * ROTATION_SPEED)
