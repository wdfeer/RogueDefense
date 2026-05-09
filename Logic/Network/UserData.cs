namespace RogueDefense.Logic.Network;

public class UserData
{
	public int id;
	public string name;
	public int ability;
	public int[] augmentPoints;

	public UserData(int id, string name, int ability, int[] augments)
	{
		this.id = id;
		this.name = name;
		this.ability = ability;
		augmentPoints = augments;
	}
}
