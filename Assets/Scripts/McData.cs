using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class McData
{
    public string action;
    public Pos pos;
}

[Serializable]
public class Pos
{
    public int x;
    public int y;
    public int z;
}