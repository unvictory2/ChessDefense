using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public int boardSize = 8;
    public BoardTile[,] tiles;
    public King mainKing; // ������ ŷ ������Ʈ
    private List<ChessPiece> _allPieces = new List<ChessPiece>();

    void Awake()
    {
        Instance = this;
        InitializeTiles();
    }

    void InitializeTiles()
    {
        tiles = new BoardTile[boardSize, boardSize];

        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                string tileName = $"Tile_{x}_{y}";
                GameObject tileObj = GameObject.Find(tileName);

                if (tileObj == null)
                {
                    Debug.LogError($"Ÿ�� ������Ʈ�� ã�� �� �����ϴ�: {tileName}");
                    continue;
                }

                BoardTile tile = tileObj.GetComponent<BoardTile>();
                if (tile == null)
                {
                    tile = tileObj.AddComponent<BoardTile>();
                }

                tile.x = x;
                tile.y = y;
                tiles[x, y] = tile;
            }
        }
    }

    // ��� �⹰ ��� �޼���
    public void RegisterPiece(ChessPiece piece)
    {
        _allPieces.Add(piece);
        if (piece is King king) mainKing = king;
    }

    // �⹰ ���� �޼���
    public void UnregisterPiece(ChessPiece piece)
    {
        _allPieces.Remove(piece);
        if (piece == mainKing) mainKing = null;
    }

    // Ÿ�� ��ȿ�� �˻�
    public bool IsValidTile(int x, int y)
    {
        return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
    }

    // Ÿ�� ��������
    public BoardTile GetTile(int x, int y)
    {
        return IsValidTile(x, y) ? tiles[x, y] : null;
    }
}
