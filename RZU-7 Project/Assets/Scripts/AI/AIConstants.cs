﻿/// <summary>
/// Holds the constant values for the AI system
/// </summary>
public class AIConstants
{
    public static readonly float TargetDistanceToFindNewPath = 5f;
    public static readonly float DistanceToRemovePoint = 1f;
    public static readonly float DistanceToAttack = 2f;

    public enum PatrolCycleDirection
    {
        forwards, backwards
    }

    // Helper name keys
    public static readonly string generalHelperKey = "General";
    public static readonly string movementHelperKey = "Movement";
}
