using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	public int score;
	public int level;
	public List<int> lines;
	public List<int> shapes;

	public GameData()
	{
		lines = new List<int>();
		shapes = new List<int>();
	}

	public void AddLines(int lines)
	{
		this.lines.Add(lines);
	}

	public void AddShape(Shape shape)
	{
		this.shapes.Add((int)shape.shapeType);
	}
}
