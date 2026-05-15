using System.Collections;
using System.Text.RegularExpressions;

namespace Chess
{
    internal partial class Program
    {
        public static ArrayList ParseInput(string input)
        {
            input = input.Replace('Q', 'D').Replace('R', 'T').Replace('B', 'A').Replace('N', 'C').Replace('K', 'R');

            ArrayList result = new ArrayList();
            string position = "";
            PieceType? pieceType = null;
            bool? capture = null;
            bool specificFile = false;
            bool specificRank = false;

            string[] patterns =
            {
                "^[TCAD][a-h][a-h].*[1-8]$", "^[TCAD][1-8][a-h].*[1-8]$",
                "^[TCAD][a-h]x[a-h].*[1-8]$", "^[TCAD][1-8]x[a-h].*[1-8]$",
                "^[a-h][1-8]", "0-0", "0-0-0", "^[TCADR][a-h].*[1-8]$",
                "^[a-h]x[a-h][1-8]$", "^[TCADR]x[a-h][1-8]$"
            };

            Match match = null;
            int matchIndex = 0;
            foreach (string pattern in patterns)
            {
                match = Regex.Match(input, pattern);
                if (match.Success) break;
                matchIndex++;
            }

            if (!match.Success) return null;

            Func<char, PieceType> getPieceType = c => c switch
            {
                'T' => PieceType.Rook,
                'C' => PieceType.Knight,
                'A' => PieceType.Bishop,
                'D' => PieceType.Queen,
                _ => PieceType.King
            };

            MoveType[] moveTypes =
            {
                MoveType.MoveSpecificFile, MoveType.MoveSpecificRank,
                MoveType.CaptureSpecificFile, MoveType.CaptureSpecificRank,
                MoveType.MovePawn, MoveType.Castling, MoveType.Castling,
                MoveType.MovePiece, MoveType.CaptureWithPawn, MoveType.CaptureWithPiece
            };

            MoveType moveType = moveTypes[matchIndex];
            PieceType initialPiece = getPieceType(input[0]);

            List<string> ambiguousMoves = null;
            bool? fileOrRank = null;

            if (initialPiece == PieceType.Rook)
            {
                ambiguousMoves = isWhiteTurn ? FindAmbiguousMoves(whiteRooks) : FindAmbiguousMoves(blackRooks);
                if (ambiguousMoves != null)
                    foreach (string pos in ambiguousMoves)
                        if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            fileOrRank = isWhiteTurn ? DifferentFiles(whiteRooks) : DifferentFiles(blackRooks);
            }
            else if (initialPiece == PieceType.Knight)
            {
                ambiguousMoves = isWhiteTurn ? FindAmbiguousMoves(whiteKnights) : FindAmbiguousMoves(blackKnights);
                if (ambiguousMoves != null)
                    foreach (string pos in ambiguousMoves)
                        if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            fileOrRank = isWhiteTurn ? DifferentFiles(whiteKnights) : DifferentFiles(blackKnights);
            }
            else if (initialPiece == PieceType.Queen)
            {
                ambiguousMoves = isWhiteTurn ? FindAmbiguousMoves(whiteQueen) : FindAmbiguousMoves(blackQueen);
                if (ambiguousMoves != null)
                    foreach (string pos in ambiguousMoves)
                        if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            fileOrRank = isWhiteTurn ? DifferentFiles(whiteQueen) : DifferentFiles(blackQueen);
            }
            else if (initialPiece == PieceType.Bishop)
            {
                ambiguousMoves = isWhiteTurn ? FindAmbiguousMoves(whiteBishops) : FindAmbiguousMoves(blackBishops);
                if (ambiguousMoves != null)
                    foreach (string pos in ambiguousMoves)
                        if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            fileOrRank = isWhiteTurn ? DifferentFiles(whiteBishops) : DifferentFiles(blackBishops);
            }

            switch (moveType)
            {
                case MoveType.MovePawn:
                    position = input.ToUpper(); pieceType = PieceType.Pawn; capture = false; break;
                case MoveType.MovePiece when fileOrRank == null:
                    position = input[1].ToString().ToUpper() + input[2]; pieceType = initialPiece; capture = false; break;
                case MoveType.CaptureWithPawn:
                    position = input[2].ToString().ToUpper() + input[3]; pieceType = PieceType.Pawn; capture = true; break;
                case MoveType.CaptureWithPiece when fileOrRank == null:
                    position = input[2].ToString().ToUpper() + input[3]; pieceType = initialPiece; capture = true; break;
                case MoveType.Castling:
                    result.Add(input); result.Add(MoveType.Castling); return result;
                case MoveType.MoveSpecificFile when fileOrRank == true:
                    position = input[2].ToString().ToUpper() + input[3]; pieceType = initialPiece; capture = false;
                    specificFile = true; specificRank = false;
                    result.Add(position); result.Add(pieceType); result.Add(capture);
                    result.Add(specificFile); result.Add(specificRank); result.Add(ambiguousMoves);
                    return result;
                case MoveType.MoveSpecificRank when fileOrRank == false:
                    position = input[2].ToString().ToUpper() + input[3]; pieceType = initialPiece; capture = false;
                    specificFile = false; specificRank = true;
                    result.Add(position); result.Add(pieceType); result.Add(capture);
                    result.Add(specificFile); result.Add(specificRank); result.Add(ambiguousMoves);
                    return result;
                case MoveType.CaptureSpecificFile when fileOrRank == true:
                    position = input[3].ToString().ToUpper() + input[4]; pieceType = initialPiece; capture = true;
                    specificFile = true; specificRank = false;
                    result.Add(position); result.Add(pieceType); result.Add(capture);
                    result.Add(specificFile); result.Add(specificRank); result.Add(ambiguousMoves);
                    return result;
                case MoveType.CaptureSpecificRank when fileOrRank == false:
                    position = input[3].ToString().ToUpper() + input[4]; pieceType = initialPiece; capture = true;
                    specificFile = false; specificRank = true;
                    result.Add(position); result.Add(pieceType); result.Add(capture);
                    result.Add(specificFile); result.Add(specificRank); result.Add(ambiguousMoves);
                    return result;
            }

            result.Add(position); result.Add(pieceType); result.Add(capture);
            return result;
        }

        public static List<string> FindAmbiguousMoves<T>(T[] pieces) where T : Piece
        {
            if (pieces[0] == null || pieces[1] == null) return null;

            string[] occupied = AllOccupiedSquares();
            string[] black = BlackOccupiedSquares();
            string[] white = WhiteOccupiedSquares();

            List<string> moves0 = new List<string>();
            moves0.AddRange(pieces[0].ValidSquares(occupied, black, white, null));
            moves0.AddRange(pieces[0].Capture(occupied, black, white, null));

            List<string> moves1 = new List<string>();
            moves1.AddRange(pieces[1].ValidSquares(occupied, black, white, null));
            moves1.AddRange(pieces[1].Capture(occupied, black, white, null));

            moves0.RemoveAll(item => int.TryParse(item, out _));
            moves1.RemoveAll(item => int.TryParse(item, out _));

            List<string> shared = new List<string>();
            moves0.ForEach(m0 =>
            {
                if (m0 != null && m0 != "")
                    moves1.ForEach(m1 => { if (m1 != null && m0 == m1) shared.Add(m0); });
            });

            return shared.Count == 0 ? null : shared;
        }

        public static bool DifferentFiles<T>(T[] pieces) where T : Piece
            => pieces[0].Position[0] != pieces[1].Position[0];
    }
}
