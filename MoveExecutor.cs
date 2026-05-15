using System.Collections;

namespace Chess
{
    internal partial class Program
    {
        public static ArrayList TryMovePawn(string input)
            => isWhiteTurn ? PawnMoveTemplate(input, whitePawns) : PawnMoveTemplate(input, blackPawns);

        public static ArrayList TryCapturePawn(string input, string rawInput)
            => isWhiteTurn ? CaptureTemplate(input, whitePawns, rawInput) : CaptureTemplate(input, blackPawns, rawInput);

        public static ArrayList TryMovePiece(string input, PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Rook   => isWhiteTurn ? PieceMoveTemplate(input, whiteRooks)   : PieceMoveTemplate(input, blackRooks),
                PieceType.Knight => isWhiteTurn ? PieceMoveTemplate(input, whiteKnights) : PieceMoveTemplate(input, blackKnights),
                PieceType.Bishop => isWhiteTurn ? PieceMoveTemplate(input, whiteBishops) : PieceMoveTemplate(input, blackBishops),
                PieceType.Queen  => isWhiteTurn ? PieceMoveTemplate(input, whiteQueen)   : PieceMoveTemplate(input, blackQueen),
                _                => isWhiteTurn ? PieceMoveTemplate(input, whiteKing)    : PieceMoveTemplate(input, blackKing),
            };
        }

        public static ArrayList TryMovePieceSpecificFile(string input, PieceType pieceType, List<string> ambiguous, string rawInput)
        {
            return pieceType switch
            {
                PieceType.Rook   => isWhiteTurn ? PieceMoveSpecificFileTemplate(input, whiteRooks,   ambiguous, rawInput) : PieceMoveSpecificFileTemplate(input, blackRooks,   ambiguous, rawInput),
                PieceType.Knight => isWhiteTurn ? PieceMoveSpecificFileTemplate(input, whiteKnights, ambiguous, rawInput) : PieceMoveSpecificFileTemplate(input, blackKnights, ambiguous, rawInput),
                PieceType.Queen  => isWhiteTurn ? PieceMoveSpecificFileTemplate(input, whiteQueen,   ambiguous, rawInput) : PieceMoveSpecificFileTemplate(input, blackQueen,   ambiguous, rawInput),
                _                => isWhiteTurn ? PieceMoveSpecificFileTemplate(input, whiteBishops, ambiguous, rawInput) : PieceMoveSpecificFileTemplate(input, blackBishops, ambiguous, rawInput),
            };
        }

        public static ArrayList TryMovePieceSpecificRank(string input, PieceType pieceType, List<string> ambiguous, string rawInput)
        {
            return pieceType switch
            {
                PieceType.Rook   => isWhiteTurn ? PieceMoveSpecificRankTemplate(input, whiteRooks,   ambiguous, rawInput) : PieceMoveSpecificRankTemplate(input, blackRooks,   ambiguous, rawInput),
                PieceType.Knight => isWhiteTurn ? PieceMoveSpecificRankTemplate(input, whiteKnights, ambiguous, rawInput) : PieceMoveSpecificRankTemplate(input, blackKnights, ambiguous, rawInput),
                PieceType.Queen  => isWhiteTurn ? PieceMoveSpecificRankTemplate(input, whiteQueen,   ambiguous, rawInput) : PieceMoveSpecificRankTemplate(input, blackQueen,   ambiguous, rawInput),
                _                => isWhiteTurn ? PieceMoveSpecificRankTemplate(input, whiteBishops, ambiguous, rawInput) : PieceMoveSpecificRankTemplate(input, blackBishops, ambiguous, rawInput),
            };
        }

        public static ArrayList TryCapturePiece(string input, PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Rook   => isWhiteTurn ? CaptureTemplate(input, whiteRooks,   "") : CaptureTemplate(input, blackRooks,   ""),
                PieceType.Knight => isWhiteTurn ? CaptureTemplate(input, whiteKnights, "") : CaptureTemplate(input, blackKnights, ""),
                PieceType.Bishop => isWhiteTurn ? CaptureTemplate(input, whiteBishops, "") : CaptureTemplate(input, blackBishops, ""),
                PieceType.Queen  => isWhiteTurn ? CaptureTemplate(input, whiteQueen,   "") : CaptureTemplate(input, blackQueen,   ""),
                _                => isWhiteTurn ? CaptureTemplate(input, whiteKing,    "") : CaptureTemplate(input, blackKing,    ""),
            };
        }

        public static ArrayList TryCaptureSpecificFile(string input, PieceType pieceType, List<string> ambiguous, string rawInput)
        {
            return pieceType switch
            {
                PieceType.Rook   => isWhiteTurn ? CaptureSpecificFileTemplate(input, whiteRooks,   ambiguous, rawInput) : CaptureSpecificFileTemplate(input, blackRooks,   ambiguous, rawInput),
                PieceType.Knight => isWhiteTurn ? CaptureSpecificFileTemplate(input, whiteKnights, ambiguous, rawInput) : CaptureSpecificFileTemplate(input, blackKnights, ambiguous, rawInput),
                PieceType.Queen  => isWhiteTurn ? CaptureSpecificFileTemplate(input, whiteQueen,   ambiguous, rawInput) : CaptureSpecificFileTemplate(input, blackQueen,   ambiguous, rawInput),
                _                => isWhiteTurn ? CaptureSpecificFileTemplate(input, whiteBishops, ambiguous, rawInput) : CaptureSpecificFileTemplate(input, blackBishops, ambiguous, rawInput),
            };
        }

        public static ArrayList TryCaptureSpecificRank(string input, PieceType pieceType, List<string> ambiguous, string rawInput)
        {
            return pieceType switch
            {
                PieceType.Rook   => isWhiteTurn ? CaptureSpecificRankTemplate(input, whiteRooks,   ambiguous, rawInput) : CaptureSpecificRankTemplate(input, blackRooks,   ambiguous, rawInput),
                PieceType.Knight => isWhiteTurn ? CaptureSpecificRankTemplate(input, whiteKnights, ambiguous, rawInput) : CaptureSpecificRankTemplate(input, blackKnights, ambiguous, rawInput),
                _                => new ArrayList(),
            };
        }

        public static bool TryCastle(string input, King[] king)
        {
            string[] occupied = AllOccupiedSquares();
            string[] white = WhiteOccupiedSquares();
            string[] black = BlackOccupiedSquares();

            List<string> enemyAttacked = king[0].IsWhite
                ? BlackAttackedSquares(occupied, black, white)
                : WhiteAttackedSquares(occupied, black, white);

            List<string> combined = new List<string>();
            combined.AddRange(occupied); combined.AddRange(white); combined.AddRange(black); combined.AddRange(enemyAttacked);

            Func<string, string, string, Rook, bool> check = (cmd, sq1, sq2, rook) =>
            {
                if (input != cmd || !rook.CanCastle) return false;
                foreach (string sq in combined)
                    if (sq == sq1 || sq == sq2) return false;
                rook.Position = sq1;
                king[0].Position = sq2;
                return true;
            };

            if (king[0].IsWhite && king[0].CanCastle)
            {
                if (input == "0-0"   && check("0-0",   "F1", "G1", whiteRooks[1])) { whiteRooks[1].CanCastle = false; whiteKing[0].CanCastle = false; return true; }
                if (input == "0-0-0" && check("0-0-0", "D1", "C1", whiteRooks[0])) { whiteRooks[0].CanCastle = false; whiteKing[0].CanCastle = false; return true; }
            }
            else if (!king[0].IsWhite && king[0].CanCastle)
            {
                if (input == "0-0"   && check("0-0",   "F8", "G8", blackRooks[1])) { blackRooks[1].CanCastle = false; blackKing[0].CanCastle = false; return true; }
                if (input == "0-0-0" && check("0-0-0", "D8", "C8", blackRooks[0])) { blackRooks[0].CanCastle = false; blackKing[0].CanCastle = false; return true; }
            }

            return false;
        }

        public static ArrayList PawnMoveTemplate(string input, Pawn[] pawns)
        {
            string[] occupied = AllOccupiedSquares();
            string[] black = BlackOccupiedSquares();
            string[] white = WhiteOccupiedSquares();

            foreach (Pawn pawn in pawns)
            {
                if (pawn == null) continue;
                List<string> valid = pawn.ValidSquares(occupied, black, white, null);

                foreach (string sq in valid)
                {
                    if (sq == input)
                    {
                        if (valid[1] != null && isWhiteTurn  && valid[1] == sq) pawn.EnPassantSquare = sq[0] + "3";
                        else if (valid[1] != null && !isWhiteTurn && valid[1] == sq) pawn.EnPassantSquare = sq[0] + "6";
                        return new ArrayList { pawn, input };
                    }
                }
            }
            return null;
        }

        public static ArrayList PieceMoveTemplate<T>(string input, T[] pieces) where T : Piece
        {
            string[] occupied = AllOccupiedSquares();
            string[] black = BlackOccupiedSquares();
            string[] white = WhiteOccupiedSquares();

            List<string> enemyAttacked = null;
            if (pieces.GetType().ToString() == "Chess.King[]")
                enemyAttacked = pieces[0].IsWhite
                    ? BlackAttackedSquares(occupied, black, white)
                    : WhiteAttackedSquares(occupied, black, white);

            foreach (T piece in pieces)
            {
                if (piece == null) continue;
                List<string> valid = piece.ValidSquares(occupied, black, white, enemyAttacked);
                if (valid == null) continue;
                foreach (string sq in valid)
                    if (sq == input) return new ArrayList { piece, input };
            }
            return null;
        }

        public static ArrayList PieceMoveSpecificFileTemplate<T>(string input, T[] pieces, List<string> ambiguous, string rawInput) where T : Piece
        {
            foreach (T piece in pieces)
                foreach (string sq in ambiguous)
                    if (sq == input && rawInput[1].ToString().ToUpper() == piece.Position[0].ToString().ToUpper())
                        return new ArrayList { piece, input };
            return null;
        }

        public static ArrayList PieceMoveSpecificRankTemplate<T>(string input, T[] pieces, List<string> ambiguous, string rawInput) where T : Piece
        {
            foreach (T piece in pieces)
                foreach (string sq in ambiguous)
                    if (sq == input && rawInput[1].ToString() == piece.Position[1].ToString())
                        return new ArrayList { piece, input };
            return null;
        }

        public static ArrayList CaptureTemplate<T>(string input, T[] pieces, string rawInput) where T : Piece
        {
            string[] occupied = AllOccupiedSquares();

            if (pieces.GetType().ToString() == "Chess.Pawn[]")
            {
                string[] white = WhiteOccupiedSquaresForPawn();
                string[] black = BlackOccupiedSquaresForPawn();

                foreach (T piece in pieces)
                {
                    if (piece == null || rawInput[0].ToString().ToUpper() != piece.Position[0].ToString()) continue;
                    List<string> valid = piece.Capture(occupied, black, white, null);
                    if (valid == null) continue;
                    foreach (string sq in valid)
                    {
                        if (sq == input)
                        {
                            if (black[16] == input || white[16] == input)
                                RemoveEnPassantPawn(input);
                            return new ArrayList { piece, input };
                        }
                    }
                }
            }
            else
            {
                string[] white = WhiteOccupiedSquares();
                string[] black = BlackOccupiedSquares();

                List<string> enemyAttacked = null;
                if (pieces.GetType().ToString() == "Chess.King[]")
                    enemyAttacked = pieces[0].IsWhite
                        ? BlackAttackedSquares(occupied, black, white)
                        : WhiteAttackedSquares(occupied, black, white);

                foreach (T piece in pieces)
                {
                    if (piece == null) continue;
                    List<string> valid = piece.Capture(occupied, black, white, enemyAttacked);
                    if (valid == null) continue;
                    foreach (string sq in valid)
                        if (sq == input) return new ArrayList { piece, input };
                }
            }

            return null;
        }

        public static ArrayList CaptureSpecificFileTemplate<T>(string input, T[] pieces, List<string> ambiguous, string rawInput) where T : Piece
        {
            foreach (T piece in pieces)
                foreach (string sq in ambiguous)
                    if (sq == input && piece.Position[0].ToString() == rawInput[1].ToString().ToUpper())
                        return new ArrayList { piece, input };
            return null;
        }

        public static ArrayList CaptureSpecificRankTemplate<T>(string input, T[] pieces, List<string> ambiguous, string rawInput) where T : Piece
        {
            foreach (T piece in pieces)
                foreach (string sq in ambiguous)
                    if (sq == input && piece.Position[1].ToString() == rawInput[1].ToString().ToUpper())
                        return new ArrayList { piece, input };
            return null;
        }
    }
}
