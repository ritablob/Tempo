using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes commands, such as attack moves. Closely working with InputHandling
/// </summary>
public abstract class Command
{
    public abstract void Execute(Actor actor, bool buttonDown = true);
}
public class Kick : Command
{
    public override void Execute(Actor actor,bool buttonDown = true)
    {
        if (buttonDown)
        {
            // do kick animation
            // if collision with opponent, send attack event
        }
    }
}
