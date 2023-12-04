using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "PieceData")]
public class PieceData : ScriptableObject
{
    public GameObject piece;
    public PieceData upgradePiece;

    public int score;
}
