using Acra.Fishs;
using Acra.Net;
using Acra.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public partial class Spawner : Node2D
{
    private Random random = new Random();
    Vector2 screenSize => GetViewport().GetVisibleRect().Size;

    private float timer = 0.0f;
    private const float delay = 2.0f;

    public Vector2 foodPos;

    public int gen = 0;

    private float mutationMin = -0.1f;
    private float mutationMax = 0.1f;

    PackedScene food => GD.Load("res://Maps/Food.tscn") as PackedScene;
    PackedScene fishP => GD.Load("res://Maps/Fish.tscn") as PackedScene;

    int actualFrame;

    public override void _Ready()
    {
        SpawnFood();
        SpawnX(80, 30, false, true, true);
    }

    public void SpawnX(
    int total = 80,
    float spacing = 30f,
    bool useSavedFish = true,
    bool randomize = true,
    bool randomPosition = true )
    {
        actualFrame = 0;

        if (!useSavedFish)
            gen = 0;

        var rand = new Random();

        for (int i = 0; i < total; i++)
        {
            Area2D fishInstance;

            if (useSavedFish)
            {
                var newFish = fishP.Instantiate<Fish>();
                newFish.FishNetwork = FishNetworkSerializer.DeserializeFromFile();

                if (randomize)
                {
                    newFish.FishNetwork.Randomize(mutationMin, mutationMax);
                }

                fishInstance = newFish;
            }
            else
            {
                fishInstance = fishP.Instantiate<Area2D>();
            }

            float x = randomPosition ? (float)rand.NextDouble() * screenSize.X : (i % 10) * spacing;
            float y = randomPosition ? (float)rand.NextDouble() * screenSize.Y : (i / 10) * spacing;
            float rot = rand.Next(0, 360);

            fishInstance.Rotation = rot;
            fishInstance.Position = new Vector2(x, y);
            AddChild(fishInstance);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        timer += (float)delta;

        actualFrame++;

        if(actualFrame > 30)
        {
            PickedFood(null);
        }

        if (timer >= delay)
        {
            timer = 0;
        }

        if (gen == 500)
        {
            mutationMin = -0.001f;
            mutationMax = 0.001f;
        }
        if (gen == 2000)
        {
            mutationMin = -0.0001f;
            mutationMax = 0.0001f;
        }
        if (gen == 8000)
        {
            mutationMin = -0.00001f;
            mutationMax = 0.00001f;
        }
    }
    public void SpawnFood()
    {
        int rX = random.Next(0, (int)screenSize.X);
        int rY = random.Next(0, (int)screenSize.Y);

        Vector2 vector = new Vector2(rX, rY);

        var f = food.Instantiate<Area2D>();
        foodPos = vector;
        f.Position = vector;

        AddChild(f);
    }

    public Vector2 GetFoodPosition(Vector2 fishPosition) => foodPos - fishPosition;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key && key.Pressed)
        {
            if (key.Keycode == Key.H)
            {
                PickedFood(null);
            }
            if (key.Keycode == Key.K)
            {
                SpawnX(100, 30, false, true);
            }
        }
    }
    public void PickedFood(Fish fish)
    {
        if (fish is not null)
        {
            FishNetworkSerializer.SerializeWriter(fish.FishNetwork);
            gen++;
        }              

        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        SpawnFood();
        SpawnX();        
    }
}
