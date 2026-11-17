using Acra.Fishs;
using Acra.Scripts;
using Godot;
using System;

public partial class Fish : Area2D
{
    [Export]
    public float health = 100;
    [Export]
    public float hunger = 100;
    [Export]
    public Vector2 food; // Food position
    [Export]
    public Vector2 preadator = new Vector2(0, 0); // Predator position

    // Output
    private float orientationRotation;
    private float speed;

    public FishNetwork FishNetwork { get; set; } = null;

    float timer = 0;
    float delay = 2f;
    public override void _Ready()
    {
        if(FishNetwork is null) 
        {
            FishNetwork = FishNetwork.Init(4);
        }
    }
    public override void _Process(double delta)
    {
        timer += (float)delta;        

        if (timer >= delay)
        {
            hunger -= 10;
            timer = 0;
        }
        if(hunger <= 0)
            QueueFree();

        CheckFood();

        Movement((float)delta);

        var outTuple = FishNetwork.FeedForward(this);
        orientationRotation = outTuple.rotation;
        speed = outTuple.speed * 100;
    }
    public void CheckFood() 
    {
        Spawner spawner = GetParent<Node2D>() as Spawner;
        food = spawner.GetFoodPosition(Position);
    }    

    public void AddFood(int value) 
    {
        if (hunger >= 100)
            return;

        FishNetworkSerializer.SerializeWriter(FishNetwork);

        hunger += value;
    }

    public virtual void Movement(float delta) 
    {
        Rotation = orientationRotation;

        Vector2 direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));

        Position += direction * speed * delta;
    }
}
