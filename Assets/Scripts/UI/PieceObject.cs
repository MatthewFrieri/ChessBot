using System.Collections.Generic;
using UnityEngine;

public class PieceObject : MonoBehaviour
{

    Vector2 startPosition;
    List<int> targetSquares;

    GameObject captureIndicator;
    GameObject moveIndicator;
    List<GameObject> activeIndicators = new List<GameObject>();


    private void Awake()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        captureIndicator = gameManager.captureIndicator;
        moveIndicator = gameManager.moveIndicator;
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;
        int startSquare = Helpers.LocationToSquare(transform.position);
        targetSquares = Player.GetLegalTargetSquares(startSquare);

        foreach (int targetSquare in targetSquares)
        {
            if (Board.PieceAt(targetSquare) == Piece.None)
            {
                GameObject indicator = Instantiate(moveIndicator, Helpers.SquareToLocation(targetSquare), Quaternion.identity);
                activeIndicators.Add(indicator);
            }
            else
            {
                GameObject indicator = Instantiate(captureIndicator, Helpers.SquareToLocation(targetSquare), Quaternion.identity);
                activeIndicators.Add(indicator);
            }
        }
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    private void OnMouseUp()
    {
        int startSquare = Helpers.LocationToSquare(startPosition);
        int targetSquare = Helpers.LocationToSquare(transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // // TODO Selecting not dragging
        // if (startSquare == targetSquare)
        // {
        //     transform.position = Helpers.SquareToLocation(targetSquare);
        //     isSelected = true;
        //     return;
        // }

        // Remove indicators
        foreach (GameObject indicator in activeIndicators)
        {
            Destroy(indicator);
        }
        activeIndicators = new List<GameObject>();


        // Out of bounds
        if (transform.position.x < 0 || transform.position.x > 8 || transform.position.y < 0 || transform.position.y > 8)
        {
            transform.position = startPosition;
        }


        if (targetSquares.Contains(targetSquare))
        {
            // Legal move
            Vector2 targetPosition = Helpers.SquareToLocation(targetSquare);
            transform.position = targetPosition;

            Player.MakeMove(startSquare, targetSquare);
        }
        else
        {
            // Illegal move
            transform.position = startPosition;
        }

    }

}
