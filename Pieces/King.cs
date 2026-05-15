namespace Chess
{
    public class King : Piece
    {
        public bool CanCastle { get; set; }
        public bool InCheck { get; set; }

        public King(char initial, string position, bool isWhite)
        {
            PieceInitial = initial;
            Position = position;
            IsWhite = isWhite;
            CanCastle = true;
            InCheck = false;
        }

        private List<string> KingSquares(int colNum, int rowNum)
        {
            return new List<string>
            {
                Position[0] != 'A' ? IntToColumn(colNum - 1) + rowNum.ToString() : null,
                Position[0] != 'H' ? IntToColumn(colNum + 1) + rowNum.ToString() : null,
                Position[1] != '1' ? IntToColumn(colNum) + (rowNum - 1).ToString() : null,
                Position[1] != '8' ? IntToColumn(colNum) + (rowNum + 1).ToString() : null,
                Position[0] != 'A' && Position[1] != '1' ? IntToColumn(colNum - 1) + (rowNum - 1).ToString() : null,
                Position[0] != 'A' && Position[1] != '8' ? IntToColumn(colNum - 1) + (rowNum + 1).ToString() : null,
                Position[0] != 'H' && Position[1] != '1' ? IntToColumn(colNum + 1) + (rowNum - 1).ToString() : null,
                Position[0] != 'H' && Position[1] != '8' ? IntToColumn(colNum + 1) + (rowNum + 1).ToString() : null,
            };
        }

        private List<string> FilterBlocked(string[]? blocked1, List<string>? blocked2, List<string> candidates)
        {
            if (blocked1 != null)
                foreach (string sq in blocked1)
                {
                    int i = 0;
                    foreach (string c in candidates) { if (sq == c) { candidates[i] = null; break; } i++; }
                }

            if (blocked2 != null)
                foreach (string sq in blocked2)
                {
                    int i = 0;
                    foreach (string c in candidates) { if (sq == c) { candidates[i] = null; break; } i++; }
                }

            return candidates;
        }

        public override List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());
            return FilterBlocked(occupiedSquares, attackedSquares, KingSquares(colNum, rowNum));
        }

        public override List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());

            List<string> candidates = KingSquares(colNum, rowNum);

            string[] ownSquares = IsWhite ? whiteSquares : blackSquares;
            string[] enemySquares = IsWhite ? blackSquares : whiteSquares;

            candidates = FilterBlocked(ownSquares, attackedSquares, candidates);

            List<string> copy = new List<string>(candidates);
            int index = 0;
            foreach (string candidate in copy)
            {
                bool remove = true;
                foreach (string sq in enemySquares)
                    if (sq == candidate) { remove = false; break; }
                candidates[index] = remove ? null : candidates[index];
                index++;
            }

            return candidates;
        }

        public List<string> AttackedSquares()
            => KingSquares(ColumnToInt(Position[0]), int.Parse(Position[1].ToString()));

        public bool IsInCheck(List<string> enemyAttackedSquares)
        {
            foreach (string sq in enemyAttackedSquares)
                if (sq == Position) return false;
            return true;
        }
    }
}
