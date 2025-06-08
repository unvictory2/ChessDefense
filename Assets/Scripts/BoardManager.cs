using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int boardSize = 8;
    public BoardTile[,] tiles;

    void Awake()
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
}
