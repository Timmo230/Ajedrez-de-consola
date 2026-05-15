namespace Chess
{
    public class SlidingPiece : Piece
    {
        protected List<string> RookValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares)
        {
            int colNum = ColumnToInt(Position[0]);
            int rowNum = int.Parse(Position[1].ToString());

            List<string> validSquares = new List<string>();
            int[] colCandidates = new int[7];
            int[] rowCandidates = new int[7];
            List<int> captureCoords = new List<int>();

            bool proceed;
            int counter;

            // Left
            proceed = true;
            counter = 0;
            int checkCol = ColumnToInt(Position[0]) - 1;
            while (checkCol != 0 && proceed)
            {
                char l = IntToColumn(checkCol);
                foreach (string sq in occupiedSquares)
                    if (l.ToString() + Position[1] == sq) { proceed = false; break; }

                if (proceed) { colCandidates[counter++] = checkCol--; }
                else { captureCoords.Add(checkCol--); captureCoords.Add(int.Parse(Position[1].ToString())); }
            }

            // Right
            proceed = true;
            checkCol = ColumnToInt(Position[0]) + 1;
            while (checkCol != 9 && proceed)
            {
                char l = IntToColumn(checkCol);
                foreach (string sq in occupiedSquares)
                    if (l.ToString() + Position[1] == sq) { proceed = false; break; }

                if (proceed) { colCandidates[counter++] = checkCol++; }
                else { captureCoords.Add(checkCol++); captureCoords.Add(int.Parse(Position[1].ToString())); }
            }

            // Down
            proceed = true;
            counter = 0;
            int checkRow = int.Parse(Position[1].ToString()) - 1;
            while (checkRow != 0 && proceed)
            {
                foreach (string sq in occupiedSquares)
                    if (Position[0] + checkRow.ToString() == sq) { proceed = false; break; }

                if (proceed) { rowCandidates[counter++] = checkRow--; }
                else { captureCoords.Add(ColumnToInt(Position[0])); captureCoords.Add(checkRow--); }
            }

            // Up
            proceed = true;
            checkRow = int.Parse(Position[1].ToString()) + 1;
            while (checkRow != 9 && proceed)
            {
                foreach (string sq in occupiedSquares)
                    if (Position[0] + checkRow.ToString() == sq) { proceed = false; break; }

                if (proceed) { rowCandidates[counter++] = checkRow++; }
                else { captureCoords.Add(ColumnToInt(Position[0])); captureCoords.Add(checkRow++); }
            }

            foreach (int i in colCandidates)
                if (i != 0) validSquares.Add(IntToColumn(i).ToString() + Position[1]);

            foreach (int i in rowCandidates)
                if (i != 0) validSquares.Add(Position[0] + i.ToString());

            captureCoords.ForEach(n => validSquares.Add(n.ToString()));

            return validSquares;
        }

        protected List<string> BishopValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares)
        {
            List<string> validSquares = new List<string>();
            List<int> captureCoords = new List<int>();

            bool proceed;
            int checkCol, checkRow;

            // Up-right
            proceed = true;
            checkCol = ColumnToInt(Position[0]) + 1;
            checkRow = int.Parse(Position[1].ToString()) + 1;
            while (checkCol < 9 && checkRow < 9 && proceed)
            {
                char l = IntToColumn(checkCol);
                foreach (string sq in occupiedSquares)
                    if (l.ToString() + checkRow == sq) { proceed = false; break; }

                if (proceed) { validSquares.Add(IntToColumn(checkCol) + checkRow.ToString()); checkCol++; checkRow++; }
                else { captureCoords.Add(checkCol++); captureCoords.Add(checkRow++); }
            }

            // Down-right
            proceed = true;
            checkCol = ColumnToInt(Position[0]) + 1;
            checkRow = int.Parse(Position[1].ToString()) - 1;
            while (checkCol < 9 && checkRow > 0 && proceed)
            {
                char l = IntToColumn(checkCol);
                foreach (string sq in occupiedSquares)
                    if (l.ToString() + checkRow == sq) { proceed = false; break; }

                if (proceed) { validSquares.Add(IntToColumn(checkCol) + checkRow.ToString()); checkCol++; checkRow--; }
                else { captureCoords.Add(checkCol++); captureCoords.Add(checkRow--); }
            }

            // Up-left
            proceed = true;
            checkCol = ColumnToInt(Position[0]) - 1;
            checkRow = int.Parse(Position[1].ToString()) + 1;
            while (checkCol > 0 && checkRow < 9 && proceed)
            {
                char l = IntToColumn(checkCol);
                foreach (string sq in occupiedSquares)
                    if (l.ToString() + checkRow == sq) { proceed = false; break; }

                if (proceed) { validSquares.Add(IntToColumn(checkCol) + checkRow.ToString()); checkCol--; checkRow++; }
                else { captureCoords.Add(checkCol--); captureCoords.Add(checkRow++); }
            }

            // Down-left
            proceed = true;
            checkCol = ColumnToInt(Position[0]) - 1;
            checkRow = int.Parse(Position[1].ToString()) - 1;
            while (checkCol > 0 && checkRow > 0 && proceed)
            {
                char l = IntToColumn(checkCol);
                foreach (string sq in occupiedSquares)
                    if (l.ToString() + checkRow == sq) { proceed = false; break; }

                if (proceed) { validSquares.Add(IntToColumn(checkCol) + checkRow.ToString()); checkCol--; checkRow--; }
                else { captureCoords.Add(checkCol--); captureCoords.Add(checkRow--); }
            }

            captureCoords.ForEach(n => validSquares.Add(n.ToString()));

            return validSquares;
        }

        protected List<string> BishopCaptures(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares)
        {
            List<string> squares = BishopValidSquares(occupiedSquares, blackSquares, whiteSquares);
            squares.RemoveAll(n => int.TryParse(n, out _) == false);

            List<string> captures = new List<string>();
            for (int i = 0; i < squares.Count; i++)
            {
                string result = IsWhite
                    ? BishopCaptureTemplate(blackSquares, int.Parse(squares[i]), int.Parse(squares[i + 1]))
                    : BishopCaptureTemplate(whiteSquares, int.Parse(squares[i]), int.Parse(squares[i + 1]));
                captures.Add(result);
                i++;
            }
            return captures;
        }

        private string BishopCaptureTemplate(string[] enemySquares, int col, int row)
        {
            foreach (string sq in enemySquares)
                if (sq != null && IntToColumn(col).ToString().ToLower() + row == sq.ToLower())
                    return IntToColumn(col).ToString().ToUpper() + row;
            return "";
        }

        protected List<string> RookCaptures(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares)
        {
            List<string> squares = RookValidSquares(occupiedSquares, blackSquares, whiteSquares);

            int moveCount = 0;
            squares.ForEach(n => { if (!int.TryParse(n, out _)) moveCount++; });

            List<string> captures = new List<string>();
            for (int i = moveCount; i < squares.Count; i++)
            {
                string result = IsWhite
                    ? RookCaptureTemplate(blackSquares, int.Parse(squares[i]), int.Parse(squares[i + 1]))
                    : RookCaptureTemplate(whiteSquares, int.Parse(squares[i]), int.Parse(squares[i + 1]));
                captures.Add(result);
                i++;
            }
            return captures;
        }

        private string RookCaptureTemplate(string[] enemySquares, int col, int row)
        {
            foreach (string sq in enemySquares)
                if (sq != null && IntToColumn(col).ToString().ToLower() + row == sq.ToLower())
                    return IntToColumn(col).ToString().ToUpper() + row;
            return "";
        }
    }
}
