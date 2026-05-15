namespace Chess
{
    public class Bishop : SlidingPiece
    {
        public Bishop(char initial, string position, bool isWhite)
        {
            PieceInitial = initial;
            Position = position;
            IsWhite = isWhite;
            PointValue = 3;
        }

        public override List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
            => BishopValidSquares(occupiedSquares, blackSquares, whiteSquares);

        public override List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
            => BishopCaptures(occupiedSquares, blackSquares, whiteSquares);
    }
}
