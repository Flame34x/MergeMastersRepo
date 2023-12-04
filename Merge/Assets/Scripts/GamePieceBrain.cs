using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePieceBrain : MonoBehaviour
{
    public PieceData piece;
    public PieceData upgradePiece;

    public Vector3 targetScale;

    public bool canMerge = true;
    public bool isDropped = false;

    private void Awake()
    {
        StartCoroutine(spawnDelay());
        targetScale = gameObject.transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Piece")
        {
            GamePieceBrain otherPiece = collision.gameObject.GetComponent<GamePieceBrain>();

            if (otherPiece.isDropped && isDropped)
            {
                if (canMerge)
                {
                    if (otherPiece.GetComponent<GamePieceBrain>().canMerge == true)
                    {
                        if (otherPiece != null && otherPiece.piece.piece == piece.piece)
                        {
                            if (upgradePiece != null)
                            {
                                EventManager.TriggerPieceUpgraded(collision.gameObject, gameObject);
                            }
                            else
                            {
                                return; // Pieces don't match, do nothing
                            }

                        }
                        else
                        {
                            return; // Pieces don't match, do nothing
                        }
                    }
                    else
                    {
                        return; // Pieces don't match, do nothing
                    }
                }
                else
                {
                    return; // Pieces don't match, do nothing
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Piece")
        {
            GamePieceBrain otherPiece = collision.gameObject.GetComponent<GamePieceBrain>();

            if (otherPiece.isDropped && isDropped)
            {
                if (canMerge)
                {
                    if (otherPiece.GetComponent<GamePieceBrain>().canMerge == true)
                    {
                        if (otherPiece != null && otherPiece.piece.piece == piece.piece)
                        {
                            if (upgradePiece != null)
                            {
                                EventManager.TriggerPieceUpgraded(collision.gameObject, gameObject);
                            }
                            else
                            {
                                return; // Pieces don't match, do nothing
                            }

                        }
                        else
                        {
                            return; // Pieces don't match, do nothing
                        }
                    }
                    else
                    {
                        return; // Pieces don't match, do nothing
                    }
                }
                else
                {
                    return; // Pieces don't match, do nothing
                }
            }
        }
    }


    private IEnumerator spawnDelay()
    {
        yield return new WaitForSeconds(0.1f);
        canMerge = true;
    }
}
