using Godot;
using System;

public partial class LabelGen : Label
{
    private Spawner spawner;

    public override void _Ready()
    {
        spawner = GetParent().GetNode<Spawner>("Spawner");
    }

    public override void _Process(double delta)
    {
        Text = spawner.gen.ToString();
    }
}
