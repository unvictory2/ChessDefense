using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int x, y;
    public ChessPiece pieceOnTile;

    public void Highlight(bool on)
    {
        // 하이라이트 구현 (예: 머티리얼 색상 변경)
        GetComponent<Renderer>().material.color = on ? Color.yellow : Color.white;
    }
}
