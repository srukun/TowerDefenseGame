using System;

public static class IDGenerator
{
    public static string GenerateID()
    {
        return Guid.NewGuid().ToString();
    }
}