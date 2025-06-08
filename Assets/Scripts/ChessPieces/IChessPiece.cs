public interface IChessPiece
{
    BoardTile CurrentTile { get; set; }
    void ShowMoveOptions(BoardManager board);
    void MoveTo(BoardTile targetTile);
}