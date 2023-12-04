using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Define an event delegate for piece upgrades
    public delegate void PieceUpgradeEventHandler(GameObject piece1, GameObject piece2);

    // Define the event based on the delegate
    public static event PieceUpgradeEventHandler OnPieceUpgraded;

    public delegate void PieceDropEventHandler(GameObject droppedPiece);

    public static event PieceDropEventHandler OnPieceDropped;

    public delegate void GameOverEventHandler();

    public static event GameOverEventHandler OnGameOver;

    public delegate void CleanBoardEventHandler();

    public static event CleanBoardEventHandler OnCleanBoard;

    public delegate void PieceRemovedEventHandler();

    public static event PieceRemovedEventHandler OnPieceRemoved;

    public delegate void ContinueGameEventHandler();

    public static event ContinueGameEventHandler OnContinueGame;

    // Method to trigger the piece upgrade event
    public static void TriggerPieceUpgraded(GameObject piece2, GameObject piece1)
    {
        if (OnPieceUpgraded != null)
        {
            OnPieceUpgraded(piece1, piece2);
        }
    }

    public static void TriggerPieceDropped(GameObject droppedPiece)
    {
        if (OnPieceDropped != null)
        {
            OnPieceDropped(droppedPiece);
        }
    }

    public static void TriggerGameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    public static void TriggerCleanBoard()
    {
        if (OnCleanBoard != null)
        {
            OnCleanBoard();
        }
    }

    public static void TriggerPieceRemoved()
    {
        if (OnPieceRemoved != null)
        {
            OnPieceRemoved();
        }
    }

    public static void TriggerContinueGame()
    {
        if (OnContinueGame != null)
        {
            OnContinueGame();
        }
    }
}