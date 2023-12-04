using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMove : MonoBehaviour
{
    public bool isMoving;
    public bool isRight;

    private void Update()
    {
        if (isMoving)
        {
            if (isRight)
            {
                GameManager.Instance.MovePiece(GameManager.Instance.selectedPieceClone, Vector3.right);
            }
            else
            {
                GameManager.Instance.MovePiece(GameManager.Instance.selectedPieceClone, Vector3.left);
            }
        }
    }

    public void EnterMovePiece(bool _isRight)
    {
        isMoving = true;
        isRight = _isRight; // Simplified assignment
        Debug.Log("EnterMovePiece: " + isRight);
    }

    public void ExitMovePiece()
    {
        isMoving = false;
        Debug.Log("ExitMovePiece");
    }
}