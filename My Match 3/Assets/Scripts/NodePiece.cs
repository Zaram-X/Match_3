using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Represents a single tile (or node) in the Match-3 grid.
// Handles appearance, position tracking, and interaction events.
public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;              // Type or value of the piece (used for matching)
    public Point index;            // Position of the piece in the grid

    [HideInInspector]
    public Vector2 pos;           // Target position in UI space

    [HideInInspector]
    public RectTransform rect;    // Cached reference to RectTransform for UI movement

    bool updating;                // Indicates if the piece is currently updating/moving
    Image img;                    // Cached reference to the UI image component

    // Initializes the piece with value, position and sprite
    public void Initialize(int v, Point p, Sprite piece)
    {
        img = GetComponent<Image>();            // Get Image component for changing sprite
        rect = GetComponent<RectTransform>();   // Get RectTransform for UI positioning

        value = v;                              // Assign value
        SetIndex(p);                            // Set initial position
        img.sprite = piece;                     // Set visual appearance
    }

    // Sets the grid index and resets visual position
    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();     // Set correct anchored position in the UI
        UpdateName();        // Rename object for easier debugging in hierarchy
    }

    // Calculates and sets the anchored UI position based on the grid index
    public void ResetPosition()
    {
        pos = new Vector2(32 + (64 * index.x), -32 - (64 * index.y));
        // 32px padding and 64px spacing assumed between tiles
    }

    // Moves the piece gradually by a vector amount (used for animations)
    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }

    // Moves the piece smoothly toward a target position
    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    // Checks if the piece is close enough to its target position and updates its movement
    public bool UpdatePiece()
    {
        if (Vector3.Distance(rect.anchoredPosition, pos) < 1)
        {
            MovePosition(pos);     // Move toward target if not already very close
            updating = true;
            return true;           // Still updating
        }
        else
        {
            rect.anchoredPosition = pos;  // Snap to final position
            updating = false;
            return false;          // Done updating
        }
    }

    // Updates the GameObject's name for easier scene debugging
    void UpdateName()
    {
        transform.name = "Node [" + index.x + "," + index.y + "]";
    }

    // Called when the player presses down on the piece (start of drag)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (updating) return;                     // Don't allow selection while moving
        MoviePieces.instance.MovePiece(this);     // Tell the controller that this piece is being dragged
    }

    // Called when the player releases the piece (end of drag)
    public void OnPointerUp(PointerEventData eventData)
    {
        MoviePieces.instance.DropPiece();         // Finalize move attempt
    }
}
