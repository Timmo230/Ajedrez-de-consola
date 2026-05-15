using static Chess.Program;

namespace Chess
{
    public class Pawn : Piece
    {
        public string EnPassantSquare { get; set; }

        public Pawn(char initial, string position, bool isWhite)
        {
            PieceInitial = initial;
            Position = position;
            IsWhite = isWhite;
            PointValue = 1;
        }

        public override List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());
            bool twoSteps = false;

            List<string> validSquares = new List<string>();

            if (IsWhite)
            {
                validSquares.Add(IntToColumn(colNum).ToString() + (rowNum + 1));
                if (Position[1] == '2') { validSquares.Add(IntToColumn(colNum).ToString() + (rowNum + 2)); twoSteps = true; }
                else validSquares.Add(null);
            }
            else
            {
                validSquares.Add(IntToColumn(colNum).ToString() + (rowNum - 1));
                if (Position[1] == '7') { validSquares.Add(IntToColumn(colNum).ToString() + (rowNum - 2)); twoSteps = true; }
                else validSquares.Add(null);
            }

            foreach (string sq in occupiedSquares)
            {
                if (validSquares[0] == sq)
                {
                    validSquares[0] = null;
                    if (twoSteps) validSquares[1] = null;
                    break;
                }
                if (twoSteps && validSquares[1] == sq)
                    validSquares[1] = null;
            }

            return validSquares;
        }

        public override List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());
            bool captureRight = false;
            bool captureLeft = false;

            List<string> validSquares = new List<string>();

            if (IsWhite)
            {
                validSquares.Add(Position[0] != 'H' ? IntToColumn(colNum + 1).ToString() + (rowNum + 1) : null);
                validSquares.Add(Position[0] != 'A' ? IntToColumn(colNum - 1).ToString() + (rowNum + 1) : null);

                foreach (string sq in blackSquares)
                {
                    if (validSquares[0] == null && validSquares[1] == null) return null;
                    else if (validSquares[0] == sq) captureRight = true;
                    else if (validSquares[1] == sq) captureLeft = true;
                }
            }
            else
            {
                validSquares.Add(Position[0] != 'H' ? IntToColumn(colNum + 1).ToString() + (rowNum - 1) : null);
                validSquares.Add(Position[0] != 'A' ? IntToColumn(colNum - 1).ToString() + (rowNum - 1) : null);

                foreach (string sq in whiteSquares)
                {
                    if (validSquares[0] == null && validSquares[1] == null) return null;
                    else if (validSquares[0] == sq) captureRight = true;
                    else if (validSquares[1] == sq) captureLeft = true;
                }
            }

            if (!captureRight) validSquares[0] = null;
            if (!captureLeft) validSquares[1] = null;

            return validSquares;
        }

        public bool ReachedPromotion()
        {
            if (IsWhite && Position[1] == '8') return true;
            if (!IsWhite && Position[1] == '1') return true;
            return false;
        }
    }
}
