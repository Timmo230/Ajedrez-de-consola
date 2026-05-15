namespace Chess
{
    public class Queen : SlidingPiece
    {
        public Queen(char initial, string position, bool isWhite)
        {
            PieceInitial = initial;
            Position = position;
            IsWhite = isWhite;
            PointValue = 9;
        }

        public override List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            List<string> squares = new List<string>();
            squares.AddRange(BishopValidSquares(occupiedSquares, blackSquares, whiteSquares));
            squares.AddRange(RookValidSquares(occupiedSquares, blackSquares, whiteSquares));
            return squares;
        }

        public override List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            List<string> squares = new List<string>();
            squares.AddRange(BishopCaptures(occupiedSquares, blackSquares, whiteSquares));
            squares.AddRange(RookCaptures(occupiedSquares, blackSquares, whiteSquares));
            return squares;
        }
    }
}
