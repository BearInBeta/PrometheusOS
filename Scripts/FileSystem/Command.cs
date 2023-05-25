using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public string command;
    public Action action;
    public string description;

    public Command (string command, Action action, string description)
    {
        this.command = command;
        this.action = action;
        this.description = description;
    }
}
