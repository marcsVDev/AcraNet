using Godot;
using System;

public partial class Food : Area2D
{
    private CollisionShape2D collisionShape;

    private void PlayerCollide(Area2D otherArea) 
    {
        Fish fish = (Fish)otherArea;
        fish.AddFood(40);

        if (fish is null)
            GD.Print("É null");

        Spawner spawner = GetParent<Spawner>();

        spawner.CallDeferred("PickedFood", fish);

        QueueFree();
    }
}
