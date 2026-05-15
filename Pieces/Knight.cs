namespace Chess
{
    public class Knight : Piece
    {
        public Knight(char initial, string position, bool isWhite)
        {
            PieceInitial = initial;
            Position = position;
            IsWhite = isWhite;
            PointValue = 3;
        }

        private List<string> KnightSquares(int colNum, int rowNum)
        {
            return new List<string>
            {
                Position[1] != '1' && Position[1] != '2' && Position[0] != 'H' ? IntToColumn(colNum + 1) + (rowNum - 2).ToString() : null,
                Position[1] != '1' && Position[1] != '2' && Position[0] != 'A' ? IntToColumn(colNum - 1) + (rowNum - 2).ToString() : null,
                Position[0] != 'G' && Position[0] != 'H' && Position[1] != '1' ? IntToColumn(colNum + 2) + (rowNum - 1).ToString() : null,
                Position[0] != 'A' && Position[0] != 'B' && Position[1] != '1' ? IntToColumn(colNum - 2) + (rowNum - 1).ToString() : null,
                Position[0] != 'H' && Position[1] != '8' && Position[1] != '7' ? IntToColumn(colNum + 1) + (rowNum + 2).ToString() : null,
                Position[0] != 'A' && Position[1] != '8' && Position[1] != '7' ? IntToColumn(colNum - 1) + (rowNum + 2).ToString() : null,
                Position[0] != 'A' && Position[0] != 'B' && Position[1] != '8' ? IntToColumn(colNum - 2) + (rowNum + 1).ToString() : null,
                Position[0] != 'H' && Position[0] != 'G' && Position[1] != '8' ? IntToColumn(colNum + 2) + (rowNum + 1).ToString() : null,
            };
        }

        private List<string> FilterOccupied(string[] blockedSquares, List<string> candidates)
        {
            foreach (string sq in blockedSquares)
            {
                int i = 0;
                foreach (string candidate in candidates)
                {
                    if (sq == candidate) { candidates[i] = null; break; }
                    i++;
                }
            }
            return candidates;
        }

        public override List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());
            return FilterOccupied(occupiedSquares, KnightSquares(colNum, rowNum));
        }

        public override List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());

            List<string> candidates = KnightSquares(colNum, rowNum);

            string[] ownSquares = IsWhite ? whiteSquares : blackSquares;
            string[] enemySquares = IsWhite ? blackSquares : whiteSquares;

            candidates = FilterOccupied(ownSquares, candidates);

            List<string> toEliminate = new List<string>(candidates);
            int index = 0;
            foreach (string candidate in candidates)
            {
                bool remove = true;
                foreach (string sq in enemySquares)
                    if (sq == candidate) { remove = false; break; }
                toEliminate[index] = remove ? null : toEliminate[index];
                index++;
            }

            return toEliminate;
        }
    }
}
