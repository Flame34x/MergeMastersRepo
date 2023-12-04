using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
    
    private bool isRight = false;

    public void OnClick()
    {
        if (isRight)
        {
            GameManager.Instance.MovePiece(GameManager.Instance.selectedPieceClone, Vector2.right);
        }
        else
        {
            GameManager.Instance.MovePiece(GameManager.Instance.selectedPieceClone, Vector2.left);
        }

    }
}

