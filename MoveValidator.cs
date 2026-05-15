namespace Chess
{
    internal partial class Program
    {
        public static bool ValidateMove(Piece piece, string targetPosition)
        {
            string previousPosition = piece.Position;

            capturedPieceCopy = null;
            foreach (Piece p in allPieces)
                if (p != null && p.Position == targetPosition) { capturedPieceCopy = p; break; }

            piece.Position = targetPosition;

            bool allowed = isWhiteTurn
                ? whiteKing[0].IsInCheck(BlackAttackedSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares()))
                : blackKing[0].IsInCheck(WhiteAttackedSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares()));

            if (!allowed) { piece.Position = previousPosition; return false; }
            return true;
        }

        public static bool ValidateMove2(Piece piece, string targetPosition)
        {
            string previousPosition = piece.Position;
            piece.Position = targetPosition;

            bool allowed = isWhiteTurn
                ? whiteKing[0].IsInCheck(BlackAttackedSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares()))
                : blackKing[0].IsInCheck(WhiteAttackedSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares()));

            piece.Position = previousPosition;
            if (!allowed) return false;
            return true;
        }

        public static bool IsCheckmate(string[] occupied, string[] blackOccupied, string[] whiteOccupied)
        {
            List<string> enemyAttacked;
            King king;

            if (isWhiteTurn)
            {
                enemyAttacked = BlackAttackedSquares(occupied, blackOccupied, whiteOccupied);
                enemyAttacked.RemoveAll(n => n == "" || n == null || int.TryParse(n, out _));
                king = whiteKing[0];
            }
            else
            {
                enemyAttacked = WhiteAttackedSquares(occupied, blackOccupied, whiteOccupied);
                enemyAttacked.RemoveAll(n => n == "" || n == null || int.TryParse(n, out _));
                king = blackKing[0];
            }

            if (king.IsInCheck(enemyAttacked)) return true;

            if (isWhiteTurn)
            {
                if (CheckmateCheck(whiteKing,    occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whitePawns,   occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteRooks,   occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteKnights, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteBishops, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteQueen,   occupied, blackOccupied, whiteOccupied)) return true;
            }
            else
            {
                if (CheckmateCheck(blackKing,    occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackPawns,   occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackRooks,   occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackKnights, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackBishops, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackQueen,   occupied, blackOccupied, whiteOccupied)) return true;
            }

            return false;
        }

        public static bool CheckmateCheck<T>(T[] piecesToCheck, string[] occupied, string[] blackOccupied, string[] whiteOccupied) where T : Piece
        {
            List<string> enemyAttacked = null;

            if (piecesToCheck.GetType().ToString() == "Chess.King[]")
            {
                enemyAttacked = piecesToCheck[0].IsWhite
                    ? BlackAttackedSquares(occupied, blackOccupied, whiteOccupied)
                    : WhiteAttackedSquares(occupied, blackOccupied, whiteOccupied);
                enemyAttacked.RemoveAll(n => n == "" || n == null || int.TryParse(n, out _));
            }

            foreach (T piece in piecesToCheck)
            {
                if (piece == null) continue;
                List<string> candidates = piece.ValidSquares(occupied, blackOccupied, whiteOccupied, enemyAttacked);
                candidates.AddRange(piece.Capture(occupied, blackOccupied, whiteOccupied, enemyAttacked));

                foreach (string sq in candidates)
                    if (sq != null && ValidateMove2(piece, sq)) return true;
            }

            return false;
        }

        public static List<string> WhiteAttackedSquares(string[] occupied, string[] blackOccupied, string[] whiteOccupied)
        {
            List<string> attacked = new List<string>();

            foreach (Piece p in whitePieces)
            {
                if (p == null || p == capturedPieceCopy) continue;
                var squares = p.ValidSquares(occupied, null, null, null);
                if (squares != null) attacked.AddRange(squares);
                squares = p.Capture(occupied, blackOccupied, whiteOccupied, null);
                if (squares != null) attacked.AddRange(squares);
            }

            var kingSquares = whiteKing[0].AttackedSquares();
            if (kingSquares != null) attacked.AddRange(kingSquares);

            attacked.RemoveAll(item => item == null);
            return attacked;
        }

        public static List<string> BlackAttackedSquares(string[] occupied, string[] blackOccupied, string[] whiteOccupied)
        {
            List<string> attacked = new List<string>();

            foreach (Piece p in blackPieces)
            {
                if (p == null || p == capturedPieceCopy) continue;
                var squares = p.ValidSquares(occupied, null, null, null);
                if (squares != null) attacked.AddRange(squares);
                squares = p.Capture(occupied, blackOccupied, whiteOccupied, null);
                if (squares != null) attacked.AddRange(squares);
            }

            var kingSquares = blackKing[0].AttackedSquares();
            if (kingSquares != null) attacked.AddRange(kingSquares);

            attacked.RemoveAll(item => item == null);
            return attacked;
        }
    }
}
