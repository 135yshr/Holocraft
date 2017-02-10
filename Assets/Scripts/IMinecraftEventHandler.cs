using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMinecraftEventHandler : IEventSystemHandler
{
    void Received(McData json);
}
