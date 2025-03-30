using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceObject : MonoBehaviour
{
    private Player player;

    Vector2 startPosition;
    List<int> targetSquares;

    public Player Player
    {
        set
        {
            player = value;
        }
    }


    private void OnMouseDown()
    {
        startPosition = transform.position;
        int startSquare = Helpers.LocationToSquare(transform.position);
        targetSquares = player.GetLegalTargetSquares(startSquare);
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    private void OnMouseUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Out of bounds
        if (transform.position.x < 0 || transform.position.x > 8 || transform.position.y < 0 || transform.position.y > 8)
        {
            transform.position = startPosition;
        }

        int targetSquare = Helpers.LocationToSquare(transform.position);

        if (targetSquares.Contains(targetSquare))
        {
            // Legal move
            Vector2 targetPosition = Helpers.SquareToLocation(targetSquare);
            transform.position = targetPosition;

            // Make the move
            int startSquare = Helpers.LocationToSquare(startPosition);
            player.MakeMove(startSquare, targetSquare);
        }
        else
        {
            // Illegal move
            transform.position = startPosition;
        }

    }

}
