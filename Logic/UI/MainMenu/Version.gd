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
	var latest_version = json["tag_name"]
	var current_version = text
	if is_version_greater(latest_version, current_version):
		print("Found a newer version " + latest_version + " (current: " + text + ")")
		$Underline.visible = true
		$Underline.modulate = Color.RED

func is_version_greater(v1: String, v2: String):
	if v1 == v2:
		return false
	
	var numbers1 = split_version(v1)
	var numbers2 = split_version(v2)
	
	for i in range(len(numbers1)):
		if numbers1[i] < numbers2[i]:
			return false
	
	return true

func split_version(version: String) -> Array :
	return (version.substr(1).split(".") as Array).map(func (str): return int(str))
