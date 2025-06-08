using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public int boardSize = 8;
    public BoardTile[,] tiles;
    public King mainKing; // 유일한 킹 오브젝트
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
                    Debug.LogError($"타일 오브젝트를 찾을 수 없습니다: {tileName}");
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

    // 모든 기물 등록 메서드
    public void RegisterPiece(ChessPiece piece)
    {
        _allPieces.Add(piece);
        if (piece is King king) mainKing = king;
    }

    // 기물 제거 메서드
    public void UnregisterPiece(ChessPiece piece)
    {
        _allPieces.Remove(piece);
        if (piece == mainKing) mainKing = null;
    }

    // 타일 유효성 검사
    public bool IsValidTile(int x, int y)
    {
        return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
    }

    // 타일 가져오기
    public BoardTile GetTile(int x, int y)
    {
        return IsValidTile(x, y) ? tiles[x, y] : null;
    }
}
