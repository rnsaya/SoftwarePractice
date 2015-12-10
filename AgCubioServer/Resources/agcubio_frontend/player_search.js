function searchForPlayer()
{
	var playerName = document.getElementById("input_player").value;
	var playerUrl = "/games?player=" + playerName;
	location.href = playerUrl;
}