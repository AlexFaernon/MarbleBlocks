using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public DoorLeverColor Color { get; private set; }
    public Teleport pairedTeleport;
    public bool sonicJustTeleported;
    public bool jumperJustTeleported;

    public TeleportClass TeleportClass
    {
        get => new() { Color = Color };
        set => SetColor(value);
    }
    public static int Count { get; private set; }

    private void OnEnable()
    {
        Count++;
    }

    private void SetColor(TeleportClass teleportClass)
    {
        Color = teleportClass.Color;
        GetComponent<SpriteRenderer>().color = Color switch
        {
            DoorLeverColor.Red => UnityEngine.Color.red,
            DoorLeverColor.Grey => UnityEngine.Color.gray,
            DoorLeverColor.Blue => UnityEngine.Color.blue,
            DoorLeverColor.Purple => UnityEngine.Color.magenta,
            DoorLeverColor.Green => UnityEngine.Color.green,
            DoorLeverColor.Yellow => UnityEngine.Color.yellow,
            _ => throw new ArgumentOutOfRangeException()
        };
        foreach (var teleport in FindObjectsOfType<Teleport>())
        {
            if (teleport.Color != Color) continue;

            pairedTeleport = teleport;
            teleport.pairedTeleport = this;
            break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Sonic") && !CharacterManager.Sonic.IsMoving && !sonicJustTeleported)
        {
            pairedTeleport.sonicJustTeleported = true;
            other.transform.position = pairedTeleport.transform.position;
        }
        
        if (other.CompareTag("Jumper") && !CharacterManager.Jumper.IsMoving && !jumperJustTeleported)
        {
            pairedTeleport.jumperJustTeleported = true;
            other.transform.position = pairedTeleport.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sonic"))
        {
            sonicJustTeleported = false;
        }
        
        if (other.CompareTag("Jumper"))
        {
            jumperJustTeleported = false;
        }
    }

    private void OnDisable()
    {
        Count--;
    }
}
