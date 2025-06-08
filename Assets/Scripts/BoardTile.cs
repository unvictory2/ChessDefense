using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int x, y;
    public ChessPiece pieceOnTile;

    public void Highlight(bool on)
    {
        // ���̶���Ʈ ���� (��: ��Ƽ���� ���� ����)
        GetComponent<Renderer>().material.color = on ? Color.yellow : Color.white;
    }
}
