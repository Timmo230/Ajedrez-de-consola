namespace Chess
{
    public class Rook : SlidingPiece
    {
        public bool CanCastle { get; set; }

        public Rook(char initial, string position, bool isWhite)
        {
            PieceInitial = initial;
            Position = position;
            IsWhite = isWhite;
            CanCastle = true;
            PointValue = 5;
        }

        public override List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
            => RookValidSquares(occupiedSquares, blackSquares, whiteSquares);

        public override List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            List<string> squares = ValidSquares(occupiedSquares, blackSquares, whiteSquares, null);

            int moveCount = 0;
            squares.ForEach(n => { if (!int.TryParse(n, out _)) moveCount++; });

            List<string> captures = new List<string>();
            for (int i = moveCount; i < squares.Count; i++)
            {
                string result = IsWhite
                    ? CaptureTemplate(blackSquares, int.Parse(squares[i]), int.Parse(squares[i + 1]))
                    : CaptureTemplate(whiteSquares, int.Parse(squares[i]), int.Parse(squares[i + 1]));
                captures.Add(result);
                i++;
            }
            return captures;
        }

        public string CaptureTemplate(string[] enemySquares, int col, int row)
        {
            foreach (string sq in enemySquares)
                if (sq != null && IntToColumn(col).ToString().ToLower() + row == sq.ToLower())
                    return IntToColumn(col).ToString().ToUpper() + row;
            return "";
        }
    }
}
