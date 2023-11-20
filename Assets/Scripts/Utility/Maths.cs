using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System;
using Unity;


public static class Maths //Maybe have Maths as namespace, then classes for e.g. number generation ect ect
{
    //Create instance of C# random class for us to use (Would use Unit equiv but I wan't to handle this in a static class for abstraction, hence no inheriting from monobehaviour, could do some monobehaviour injection, but this method should be fine!)
    private static Random random = new Random();

    private static Vector2 cachedBoundsX; //Will be used to cache bounds for generating random co-ords
    private static Vector2 cachedBoundsY;
    private static Vector2 cachedBoundsZ;

    public static Vector3 GetRandomPosition3D()
    {
        //May want to move this out of function as it will be set every time we use this function!
        double minX = cachedBoundsX.X;
        double maxX = cachedBoundsX.Y;

        double minY = cachedBoundsY.X;
        double maxY = cachedBoundsY.Y;

        double minZ = cachedBoundsZ.X;
        double maxZ = cachedBoundsZ.Y;

        double rangeX = maxX - minX;
        double rangeY = maxY - minY;
        double rangeZ = maxZ - minZ;

        //Calculate random points based off cached bounds
        double sample = random.NextDouble();
        double scaledX = (sample * rangeX) + minX;
        double scaledY = (sample * rangeY) + minY;
        double scaledZ = (sample * rangeZ) + minZ;

        //Cast to float so they are more readable if viewing in inspector / debugging
        float fX = (float)scaledX;
        float fY = (float)scaledY;
        float fZ = (float)scaledZ;

        Vector3 randomPoint = new Vector3(fX, fY, fZ);


        return randomPoint;
    }

    //another for random position 2d i.e spawn, as it must start from the botttom, there for we will j in unity vector swamp this Y value to Z value 
    public static Vector2 GetSpawnPosition2D()
    {
        //May want to move this out of function as it will be set every time we use this function!
        double minX = cachedBoundsX.X;
        double maxX = cachedBoundsX.Y;

        double minZ = cachedBoundsZ.X;
        double maxZ = cachedBoundsZ.Y;

        double rangeX = maxX - minX;
        double rangeZ = maxZ - minZ;

        //Calculate our random points based off cached bounds
        double sample = random.NextDouble();
        double scaledX = (sample * rangeX) + minX;
        double scaledZ = (sample * rangeZ) + minZ;

        //Cast to float so they are more readable if viewing in inspector / debugging
        float fX = (float)scaledX;
        float fZ = (float)scaledZ;

        Vector2 randomPoint = new Vector2(fX, fZ);

        return randomPoint;

    }

    public static int CalculateBirdShotScore(float timeOnScreen,float speed)
    {
        int score = (int)(500 / timeOnScreen * speed); //Very basic sum, should make it a bit more fancy, but it works for now...

        return score;
    }

    public static float CalculateBirdSpeed(int currentRound)
    {

        //May want to move this out of function as it will be set every time we use this function!
        double minSpeed = 2 + (0.35 * currentRound);
        double maxSpeed = 4 + (0.35 * currentRound);

        
        double range = maxSpeed - minSpeed;

        //Calculate our random points based off cached bounds
        double sample = random.NextDouble();
        double scaledSpeed = (sample * range) + minSpeed;
        

        //Cast to float so they are more readable if viewing in inspector / debugging
        float speedFloat = (float)scaledSpeed;


        float speed = speedFloat;

        return speed;
    }

    public static void SetBounds(Vector2 x, Vector2 y, Vector2 z) //Cached bounds done on scene or game start, x will represent min and y will represent max value for each bounds param
    {
        cachedBoundsX.X = x.X;
        cachedBoundsX.Y = x.Y;

        cachedBoundsY.X = y.X;
        cachedBoundsY.Y = y.Y;

        cachedBoundsZ.X = z.X;
        cachedBoundsZ.Y = z.Y;
    }
}
