using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// This class is for hiding the tilemap collider material used for collision between the player and the objects on the map
/// </summary>
public class HideTilemapColliderOnPlay : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    /// <summary>
    /// This method is to hide the tilemap collider material from displaying on the map
    /// </summary>
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = false;
    }
}
