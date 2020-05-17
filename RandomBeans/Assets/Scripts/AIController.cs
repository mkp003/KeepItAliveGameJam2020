using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public delegate bool Choice();
    public delegate void Command();

    AIController left;
    AIController right;
    Choice choiceMade;
    Command commandTaken;

    public void AIContoller()
    {
        left = null;
        right = null;
        choiceMade = null;
        commandTaken = null;
    }

    public void SetChoice(Choice choice)
    {
        choiceMade = choice;
    }

    public void SetCommand(Command command)
    {
        commandTaken = command;
    }

    public void SetLeft(AIController l)
    {
        left = l;
    }

    public void SetRight(AIController r)
    {
        right = r;
    }

    public bool Choose()
    {
        return choiceMade();
    }

    public void goLeft()
    {
        left.Traverse();
    }

    public void goRight()
    {
        right.Traverse();
    }

    public void Traverse()
    {
        if (commandTaken != null)
        {
            commandTaken();
        }
        else if (Choose())
        {
            goLeft();
        }
        else
        {
            goRight();
        }
    }
}
