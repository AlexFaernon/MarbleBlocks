using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public DoorLeverColor Color { get; private set; }
    public Teleport pairedTeleport;
    public bool sonicJustTeleported;
    public bool jumperJustTeleported;
    public static readonly Dictionary<DoorLeverColor, int> CountByColors = new()
    {
        { DoorLeverColor.Purple, 0 },
        { DoorLeverColor.Green , 0 },
        { DoorLeverColor.Blue , 0 }
    };

    public TeleportClass TeleportClass
    {
        get => new() { Color = Color };
        set => SetColor(value);
    }

    public static int Count => CountByColors.Sum(pair => pair.Value);

    private void SetColor(TeleportClass teleportClass)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        Color = teleportClass.Color;
        spriteRenderer.sprite = Resources.Load<Sprite>($"Teleports/{Color}");
        CountByColors[Color]++;
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
        if (pairedTeleport is null) return;
        
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
        CountByColors[Color]--;
    }
}
