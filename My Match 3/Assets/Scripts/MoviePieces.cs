using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles user interactions with the pieces on the match-3 game board.
public class MoviePieces : MonoBehaviour
{
    public static MoviePieces instance;     // Singleton instance for easy global access
    Match3 game;                            // Reference to the Match3 game logic script

    NodePiece moving;                       // The piece currently being moved by the player
    Point newIndex;                         // Target index where the piece will be moved to
    Vector2 mouseStart;                     // Mouse position at the start of the drag

    private void Awake()
    {
        instance = this;                    // Assign singleton instance on creation
    }

    void Start()
    {
        game = GetComponent<Match3>();      // Get the Match3 component attached to the same GameObject
    }

    void Update()
    {
        // If a piece is currently being moved by the player
        if (moving != null)
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart); // Direction from mouse start to current
            Vector2 nDir = dir.normalized;                             // Normalized direction vector
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y)); // Absolute direction for comparison

            newIndex = Point.clone(moving.index); // Start with the current index
            Point add = Point.zero;               // Offset to add to index

            // Only consider movement if the drag distance is significant (greater than 32 pixels)
            if (dir.magnitude > 32)
            {
                // Determine move direction based on dominant axis
                if (aDir.x > aDir.y)
                    add = new Point((nDir.x > 0) ? 1 : -1, 0);  // Move left or right
                else if (aDir.y > aDir.x)
                    add = new Point(0, (nDir.y > 0) ? -1 : 1);  // Move up or down (screen space Y is flipped)
            }

            newIndex.add(add); // Apply the calculated direction to get the new target index

            Vector2 pos = game.getPositionFromPoint(moving.index); // Get current pixel position of the piece
            if (!newIndex.Equals(moving.index))
            {
                // Offset position in the drag direction for visual feedback
                pos += Point.mult(new Point(add.x, -add.y), 16).ToVector();
            }

            moving.MovePositionTo(pos); // Smoothly move piece to the new position
        }
    }

    // Called when the player starts dragging a piece
    public void MovePiece(NodePiece piece)
    {
        if (moving != null) return;           // Don't allow another move while one is in progress
        moving = piece;                       // Set the piece being moved
        mouseStart = Input.mousePosition;     // Record the starting mouse position
        newIndex = Point.clone(moving.index); // Set the target to the current index initially
    }

    // Called when the player releases the dragged piece
    public void DropPiece()
    {
        if (moving == null) return;

        // If the piece was dragged to a new valid position, attempt to swap
        if (!newIndex.Equals(moving.index) && game.isWithinBounds(newIndex))
        {
            game.FlipPieces(moving.index, newIndex, true); // Swap pieces and perform match check
        }
        else
        {
            game.ResetPiece(moving); // Snap piece back to its original position if no valid move
        }

        moving = null; // Clear the moving reference
    }
}
