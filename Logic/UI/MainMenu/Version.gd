extends LinkButton

@onready var request: HTTPRequest = $HTTPRequest

func _ready():
	text = "v" + ProjectSettings.get_setting("application/config/version")
	
	request.request_completed.connect(_on_request_completed)
	
	request.request("https://api.github.com/repos/wdfeer/roguedefense/releases/latest")

func _on_request_completed(result, response_code, headers, body):
	if result != HTTPRequest.RESULT_SUCCESS:
		return
	
	var json = JSON.parse_string(body.get_string_from_utf8())
	var latest_version: String = json["tag_name"]
	if latest_version != text:
		print("Found a newer version " + latest_version + " (current: " + text + ")")
		add_theme_color_override("font_color", Color.DARK_RED)
