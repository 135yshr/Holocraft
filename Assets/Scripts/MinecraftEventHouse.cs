using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecraftEventHouse : MonoBehaviour, IMinecraftEventHandler
{
    public void Received(McData json)
    {
        Debug.Log(json);
    }
}
