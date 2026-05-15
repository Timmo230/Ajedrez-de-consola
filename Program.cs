using System.Collections;
using System.Text.RegularExpressions;
using static Chess.Program;

namespace Chess
{
    internal class Program
    {
        public enum MoveType
        {
            MovePawn, MovePiece, CaptureWithPawn, CaptureWithPiece, Castling,
            MoveSpecificFile, MoveSpecificRank, CaptureSpecificFile, CaptureSpecificRank
        }

        public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }

        public static string[,] Board =
        {
            {"A8","B8","C8","D8","E8","F8","G8","H8"},
            {"A7","B7","C7","D7","E7","F7","G7","H7"},
            {"A6","B6","C6","D6","E6","F6","G6","H6"},
            {"A5","B5","C5","D5","E5","F5","G5","H5"},
            {"A4","B4","C4","D4","E4","F4","G4","H4"},
            {"A3","B3","C3","D3","E3","F3","G3","H3"},
            {"A2","B2","C2","D2","E2","F2","G2","H2"},
            {"A1","B1","C1","D1","E1","F1","G1","H1"}
        };

        public static string[,] BoardFlipped =
        {
            {"H1","G1","F1","E1","D1","C1","B1","A1"},
            {"H2","G2","F2","E2","D2","C2","B2","A2"},
            {"H3","G3","F3","E3","D3","C3","B3","A3"},
            {"H4","G4","F4","E4","D4","C4","B4","A4"},
            {"H5","G5","F5","E5","D5","C5","B5","A5"},
            {"H6","G6","F6","E6","D6","C6","B6","A6"},
            {"H7","G7","F7","E7","D7","C7","B7","A7"},
            {"H8","G8","F8","E8","D8","C8","B8","A8"}
        };

        public static Pawn[] whitePawns = new Pawn[10];
        public static Pawn[] blackPawns = new Pawn[10];
        public static Rook[] whiteRooks = new Rook[10];
        public static Rook[] blackRooks = new Rook[10];
        public static Knight[] whiteKnights = new Knight[10];
        public static Knight[] blackKnights = new Knight[10];
        public static Bishop[] whiteBishops = new Bishop[10];
        public static Bishop[] blackBishops = new Bishop[10];
        public static King[] whiteKing = new King[1];
        public static King[] blackKing = new King[1];
        public static Queen[] whiteQueen = new Queen[10];
        public static Queen[] blackQueen = new Queen[10];

        public static Piece[,] allPieces = new Piece[12, 10];
        public static Piece[,] whitePieces = new Piece[6, 10];
        public static Piece[,] blackPieces = new Piece[6, 10];

        public static bool isWhiteTurn = true;
        public static bool isPlaying = true;
        public static List<string> moveHistory = new List<string>();
        public static Piece capturedPieceCopy = null;

        static void Main(string[] args)
        {
            InitializeGame();
            while (isPlaying)
                PlayTurn();
        }

        public static void PlayTurn()
        {
            ResetEnPassant();
            capturedPieceCopy = null;

            ArrayList output = null;
            string input;
            RefreshBoard();
            ArrayList result = null;
            bool castled = false;
            bool moved = false;

            do
            {
                bool gameOn = IsCheckmate(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares());

                if (!gameOn)
                {
                    int counter = 1;
                    Console.WriteLine(isWhiteTurn ? "\nBlack wins\n" : "\nWhite wins\n");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nMove history:");
                    foreach (string move in moveHistory)
                    {
                        if (counter % 2 == 0) Console.WriteLine(move);
                        else Console.Write("{0} -> {1} -", (counter / 2) + 1, move);
                        counter++;
                    }
                    Console.WriteLine("\n");
                    Environment.Exit(0);
                }

                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.WriteLine(isWhiteTurn ? "White's turn:" : "Black's turn:");
                Console.Write("Enter move: ");
                input = Console.ReadLine();
                Console.WriteLine();
                output = ParseInput(input);

                if (output != null && output.Count == 3)
                {
                    switch (output[1])
                    {
                        case PieceType p when p == PieceType.Pawn && !(bool)output[2]:
                            result = TryMovePawn(output[0].ToString()); break;
                        case PieceType p when p != PieceType.King && !(bool)output[2]:
                            result = TryMovePiece(output[0].ToString(), (PieceType)output[1]); break;
                        case PieceType p when p == PieceType.King && !(bool)output[2]:
                            result = TryMovePiece(output[0].ToString(), (PieceType)output[1]); break;
                        case PieceType p when p == PieceType.Pawn && (bool)output[2]:
                            result = TryCapturePawn(output[0].ToString(), input); break;
                        case PieceType p when p != PieceType.King && (bool)output[2]:
                            result = TryCapturePiece(output[0].ToString(), (PieceType)output[1]); break;
                        case PieceType p when p == PieceType.King && (bool)output[2]:
                            result = TryCapturePiece(output[0].ToString(), (PieceType)output[1]); break;
                    }
                }
                else if (output != null && output.Count == 6)
                {
                    switch (output[1])
                    {
                        case PieceType p when p == PieceType.Rook && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Rook && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Bishop && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Bishop && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;

                        case PieceType p when p == PieceType.Rook && (bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryCaptureSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Rook && (bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryCaptureSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && (bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryCaptureSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && (bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryCaptureSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen && (bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryCaptureSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen && (bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryCaptureSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                    }
                }
                else if (output != null && output[1].GetType().ToString() == "Chess.Program+MoveType")
                {
                    if (output.Count == 2 && (MoveType)output[1] == MoveType.Castling && result == null)
                    {
                        castled = isWhiteTurn
                            ? TryCastle(output[0].ToString(), whiteKing)
                            : TryCastle(output[0].ToString(), blackKing);

                        if (castled) moved = true;
                    }
                }

                if (!castled && output != null && result != null)
                    moved = ValidateMove((Piece)result[0], result[1].ToString());

                if (!moved)
                {
                    RefreshBoard();
                    Console.WriteLine("Invalid move");
                }
                else
                {
                    moveHistory.Add(input);
                }

            } while (!moved);

            if ((output.Count == 3 || output.Count == 6) && isWhiteTurn && (bool)output[2])
            {
                RemoveCaptured((PieceType)output[1], true);
                if ((PieceType)output[1] == PieceType.Pawn) PromotePawn();
            }
            else if ((output.Count == 3 || output.Count == 6) && !isWhiteTurn && (bool)output[2])
            {
                RemoveCaptured((PieceType)output[1], false);
                if ((PieceType)output[1] == PieceType.Pawn) PromotePawn();
            }

            isWhiteTurn = !isWhiteTurn;
        }

        public static void PromotePawn()
        {
            bool promoted = false;
            Pawn pawnToPromote = null;

            Pawn[] pawns = isWhiteTurn ? whitePawns : blackPawns;
            foreach (Pawn p in pawns)
            {
                if (p != null && p.ReachedPromotion()) { pawnToPromote = p; promoted = true; break; }
            }

            if (!promoted) return;

            int globalIndex = 0;
            foreach (Piece p in allPieces) { if (p == pawnToPromote) break; globalIndex++; }

            string pos = pawnToPromote.Position;
            bool repeat = true;

            while (repeat)
            {
                Console.WriteLine("Promote pawn to:\n\tKnight = n\n\tBishop = b\n\tRook = r\n\tQueen = q\n-----------------------------");
                Console.Write("Piece: ");
                string choice = Console.ReadLine().ToLower().Trim();

                if (choice != "n" && choice != "b" && choice != "r" && choice != "q")
                {
                    RefreshBoard();
                    Console.WriteLine("Invalid choice");
                }
                else
                {
                    repeat = false;
                    int typeIndex = 0;
                    int colorIndex = 0;

                    Piece[,] coloredPieces = isWhiteTurn ? whitePieces : blackPieces;

                    foreach (Piece p in coloredPieces) { if (p == pawnToPromote) break; colorIndex++; }

                    if (isWhiteTurn)
                    {
                        switch (choice)
                        {
                            case "n":
                                foreach (Piece p in whiteKnights) { if (p == null) break; typeIndex++; }
                                var wk = new Knight('n', pos, true);
                                allPieces[globalIndex / 10, globalIndex % 10] = wk;
                                whiteKnights[typeIndex] = wk;
                                whitePieces[colorIndex / 10, colorIndex % 10] = wk;
                                break;
                            case "b":
                                foreach (Piece p in whiteBishops) { if (p == null) break; typeIndex++; }
                                var wb = new Bishop('b', pos, true);
                                allPieces[globalIndex / 10, globalIndex % 10] = wb;
                                whiteBishops[typeIndex] = wb;
                                whitePieces[colorIndex / 10, colorIndex % 10] = wb;
                                break;
                            case "r":
                                foreach (Piece p in whiteRooks) { if (p == null) break; typeIndex++; }
                                var wr = new Rook('r', pos, true);
                                allPieces[globalIndex / 10, globalIndex % 10] = wr;
                                whiteRooks[typeIndex] = wr;
                                whitePieces[colorIndex / 10, colorIndex % 10] = wr;
                                break;
                            case "q":
                                foreach (Piece p in whiteQueen) { if (p == null) break; typeIndex++; }
                                var wq = new Queen('q', pos, true);
                                allPieces[globalIndex / 10, globalIndex % 10] = wq;
                                whiteQueen[typeIndex] = wq;
                                whitePieces[colorIndex / 10, colorIndex % 10] = wq;
                                break;
                        }
                    }
                    else
                    {
                        switch (choice)
                        {
                            case "n":
                                foreach (Piece p in blackKnights) { if (p == null) break; typeIndex++; }
                                var bk = new Knight('n', pos, false);
                                allPieces[globalIndex / 10, globalIndex % 10] = bk;
                                blackKnights[typeIndex] = bk;
                                blackPieces[colorIndex / 10, colorIndex % 10] = bk;
                                break;
                            case "b":
                                foreach (Piece p in blackBishops) { if (p == null) break; typeIndex++; }
                                var bb = new Bishop('b', pos, false);
                                allPieces[globalIndex / 10, globalIndex % 10] = bb;
                                blackBishops[typeIndex] = bb;
                                blackPieces[colorIndex / 10, colorIndex % 10] = bb;
                                break;
                            case "r":
                                foreach (Piece p in blackRooks) { if (p == null) break; typeIndex++; }
                                var br = new Rook('r', pos, false);
                                allPieces[globalIndex / 10, globalIndex % 10] = br;
                                blackRooks[typeIndex] = br;
                                blackPieces[colorIndex / 10, colorIndex % 10] = br;
                                break;
                            case "q":
                                foreach (Piece p in blackQueen) { if (p == null) break; typeIndex++; }
                                var bq = new Queen('q', pos, false);
                                allPieces[globalIndex / 10, globalIndex % 10] = bq;
                                blackQueen[typeIndex] = bq;
                                blackPieces[colorIndex / 10, colorIndex % 10] = bq;
                                break;
                        }
                    }
                }
            }
        }

        public static bool ValidateMove(Piece piece, string targetPosition)
        {
            string previousPosition = piece.Position;

            foreach (Piece p in allPieces)
                if (p != null && p.Position == targetPosition) { capturedPieceCopy = p; break; }

            capturedPieceCopy = null;
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
                if (CheckmateCheck(whiteKing, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whitePawns, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteRooks, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteKnights, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteBishops, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(whiteQueen, occupied, blackOccupied, whiteOccupied)) return true;
            }
            else
            {
                if (CheckmateCheck(blackKing, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackPawns, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackRooks, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackKnights, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackBishops, occupied, blackOccupied, whiteOccupied)) return true;
                if (CheckmateCheck(blackQueen, occupied, blackOccupied, whiteOccupied)) return true;
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

        public static void RemoveCaptured(PieceType pieceType, bool isWhite)
        {
            switch (pieceType)
            {
                case PieceType.Rook:
                    if (isWhite) RemoveCapturedByWhite(whiteRooks); else RemoveCapturedByBlack(blackRooks); break;
                case PieceType.Knight:
                    if (isWhite) RemoveCapturedByWhite(whiteKnights); else RemoveCapturedByBlack(blackKnights); break;
                case PieceType.Bishop:
                    if (isWhite) RemoveCapturedByWhite(whiteBishops); else RemoveCapturedByBlack(blackBishops); break;
                case PieceType.Queen:
                    if (isWhite) RemoveCapturedByWhite(whiteQueen); else RemoveCapturedByBlack(blackQueen); break;
                case PieceType.King:
                    if (isWhite) RemoveCapturedByWhite(whiteKing); else RemoveCapturedByBlack(blackKing); break;
                case PieceType.Pawn:
                    if (isWhite) RemoveCapturedByWhite(whitePawns); else RemoveCapturedByBlack(blackPawns); break;
            }
        }

        public static void RemoveCapturedByWhite(Piece[] attackers)
        {
            foreach (Piece attacker in attackers)
            {
                if (attacker == null) continue;
                int i = 0;
                foreach (Piece blackPiece in blackPieces)
                {
                    if (blackPiece != null && blackPiece.Position == attacker.Position)
                    {
                        char initial = blackPieces[i / 10, i % 10]?.PieceInitial ?? ' ';
                        blackPieces[i / 10, i % 10] = null;

                        if (i / 10 == 0) allPieces[1, i % 10] = null;
                        else if (i / 10 == 1) allPieces[3, i % 10] = null;
                        else if (i / 10 == 2) allPieces[5, i % 10] = null;
                        else if (i / 10 == 3) allPieces[7, i % 10] = null;
                        else if (i / 10 == 4) allPieces[9, i % 10] = null;
                        else if (i / 10 == 5) allPieces[11, i % 10] = null;

                        switch (initial)
                        {
                            case 'r': blackRooks[i % 10] = null; break;
                            case 'b': blackBishops[i % 10] = null; break;
                            case 'n': blackKnights[i % 10] = null; break;
                            case 'q': blackQueen[i % 10] = null; break;
                            case 'k': blackKing[0] = null; break;
                            case 'p': blackPawns[i % 10] = null; break;
                        }
                        break;
                    }
                    i++;
                }
            }
        }

        public static void RemoveCapturedByBlack(Piece[] attackers)
        {
            foreach (Piece attacker in attackers)
            {
                if (attacker == null) continue;
                int i = 0;
                foreach (Piece whitePiece in whitePieces)
                {
                    if (whitePiece != null && whitePiece.Position == attacker.Position)
                    {
                        char initial = whitePieces[i / 10, i % 10]?.PieceInitial ?? ' ';
                        whitePieces[i / 10, i % 10] = null;

                        if (i / 10 == 0) allPieces[0, i % 10] = null;
                        else if (i / 10 == 1) allPieces[2, i % 10] = null;
                        else if (i / 10 == 2) allPieces[4, i % 10] = null;
                        else if (i / 10 == 3) allPieces[6, i % 10] = null;
                        else if (i / 10 == 4) allPieces[8, i % 10] = null;
                        else if (i / 10 == 5) allPieces[10, i % 10] = null;

                        switch (initial)
                        {
                            case 'r': whiteRooks[i % 10] = null; break;
                            case 'b': whiteBishops[i % 10] = null; break;
                            case 'n': whiteKnights[i % 10] = null; break;
                            case 'q': whiteQueen[i % 10] = null; break;
                            case 'k': whiteKing[0] = null; break;
                            case 'p': whitePawns[i % 10] = null; break;
                        }
                        break;
                    }
                    i++;
                }
            }
        }

        public static ArrayList ParseInput(string input)
        {
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

        public static void DrawBoard()
        {
            if (isWhiteTurn) DrawBoardTemplate(Board);
            else DrawBoardTemplate(BoardFlipped);
        }

        public static void DrawBoardTemplate(string[,] board)
        {
            bool isLight = false;
            int counter = 1;
            bool addSpacing = true;

            foreach (string sq in board)
            {
                isLight = !isLight;
                SetSquareColor(isLight);

                string display = GetSquareDisplay(sq);

                if (addSpacing)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (i == 7) Console.WriteLine("       ");
                        else Console.Write("       ");
                        isLight = !isLight;
                        Console.BackgroundColor = isLight ? ConsoleColor.White : ConsoleColor.Black;
                    }
                }

                if (display == " ")
                {
                    if (counter % 8 == 0) { Console.WriteLine("       "); isLight = !isLight; addSpacing = true; }
                    else { Console.Write("       "); addSpacing = false; }
                }
                else
                {
                    if (counter % 8 == 0) { Console.WriteLine("  " + display.ToUpper() + "   "); isLight = !isLight; addSpacing = true; }
                    else { Console.Write("  " + display.ToUpper() + "   "); addSpacing = false; }
                }

                if (addSpacing)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SetSquareColor(isLight);
                        isLight = !isLight;
                        if (i == 7) Console.WriteLine("       ");
                        else Console.Write("       ");
                    }
                }

                counter++;
            }
        }

        public static string GetSquareDisplay(string square)
        {
            foreach (Piece p in allPieces)
            {
                if (p != null && p.Position == square)
                {
                    SetPieceColor(p);
                    return " " + p.PieceInitial.ToString();
                }
            }
            return " ";
        }

        public static void RefreshBoard()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
            DrawBoard();
        }

        public static void SetSquareColor(bool isLight)
        {
            Console.ForegroundColor = isLight ? ConsoleColor.Black : ConsoleColor.White;
            Console.BackgroundColor = isLight ? ConsoleColor.White : ConsoleColor.Black;
        }

        public static void SetPieceColor(Piece p)
        {
            Console.ForegroundColor = p.IsWhite ? ConsoleColor.Magenta : ConsoleColor.DarkCyan;
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

        public static string[] AllOccupiedSquares()
        {
            string[] occupied = new string[32];
            int i = 0;
            foreach (Piece p in allPieces)
                if (p != null && p != capturedPieceCopy) occupied[i++] = p.Position;
            return occupied;
        }

        public static string[] WhiteOccupiedSquares()
        {
            string[] occupied = new string[16];
            int i = 0;
            foreach (Piece p in whitePieces)
                if (p != null && p != capturedPieceCopy) occupied[i++] = p.Position;
            return occupied;
        }

        public static string[] BlackOccupiedSquares()
        {
            string[] occupied = new string[16];
            int i = 0;
            foreach (Piece p in blackPieces)
                if (p != null) occupied[i++] = p.Position;
            return occupied;
        }

        public static void InitializeGame()
        {
            whitePawns = new Pawn[] { new('p',"A2",true), new('p',"B2",true), new('p',"C2",true), new('p',"D2",true), new('p',"E2",true), new('p',"F2",true), new('p',"G2",true), new('p',"H2",true), null, null };
            blackPawns  = new Pawn[] { new('p',"A7",false), new('p',"B7",false), new('p',"C7",false), new('p',"D7",false), new('p',"E7",false), new('p',"F7",false), new('p',"G7",false), new('p',"H7",false), null, null };
            whiteRooks   = new Rook[]   { new('r',"A1",true),  new('r',"H1",true),  null,null,null,null,null,null,null,null };
            blackRooks   = new Rook[]   { new('r',"A8",false), new('r',"H8",false), null,null,null,null,null,null,null,null };
            whiteKnights = new Knight[] { new('n',"B1",true),  new('n',"G1",true),  null,null,null,null,null,null,null,null };
            blackKnights = new Knight[] { new('n',"B8",false), new('n',"G8",false), null,null,null,null,null,null,null,null };
            whiteBishops = new Bishop[] { new('b',"C1",true),  new('b',"F1",true),  null,null,null,null,null,null,null,null };
            blackBishops = new Bishop[] { new('b',"C8",false), new('b',"F8",false), null,null,null,null,null,null,null,null };
            whiteKing    = new King[]   { new('k',"E1",true)  };
            blackKing    = new King[]   { new('k',"E8",false) };
            whiteQueen   = new Queen[]  { new('q',"D1",true),  null,null,null,null,null,null,null,null };
            blackQueen   = new Queen[]  { new('q',"D8",false), null,null,null,null,null,null,null,null };

            int row = 0, wRow = 0, bRow = 0;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whitePawns[col]; whitePieces[wRow, col] = whitePawns[col]; }
            row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackPawns[col]; blackPieces[bRow, col] = blackPawns[col]; }
            row++; bRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whiteRooks[col]; whitePieces[wRow, col] = whiteRooks[col]; }
            row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackRooks[col]; blackPieces[bRow, col] = blackRooks[col]; }
            row++; bRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whiteKnights[col]; whitePieces[wRow, col] = whiteKnights[col]; }
            row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackKnights[col]; blackPieces[bRow, col] = blackKnights[col]; }
            row++; bRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whiteBishops[col]; whitePieces[wRow, col] = whiteBishops[col]; }
            row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackBishops[col]; blackPieces[bRow, col] = blackBishops[col]; }
            row++; bRow++;
            allPieces[row++, 0] = whiteKing[0]; whitePieces[wRow++, 0] = whiteKing[0];
            allPieces[row++, 0] = blackKing[0]; blackPieces[bRow++, 0] = blackKing[0];
            allPieces[row++, 0] = whiteQueen[0]; whitePieces[wRow++, 0] = whiteQueen[0];
            allPieces[row++, 0] = blackQueen[0]; blackPieces[bRow++, 0] = blackQueen[0];
        }

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

        public static string[] WhiteOccupiedSquaresForPawn()
        {
            string[] occupied = new string[17];
            int i = 0;
            foreach (Piece p in whitePieces)
                if (p != null) occupied[i++] = p.Position;
            foreach (Pawn pawn in whitePawns)
                if (pawn != null && pawn.EnPassantSquare != null && pawn.EnPassantSquare != "")
                { occupied[16] = pawn.EnPassantSquare; break; }
            return occupied;
        }

        public static string[] BlackOccupiedSquaresForPawn()
        {
            string[] occupied = new string[17];
            int i = 0;
            foreach (Piece p in blackPieces)
                if (p != null) occupied[i++] = p.Position;
            foreach (Pawn pawn in blackPawns)
                if (pawn != null && pawn.EnPassantSquare != null && pawn.EnPassantSquare != "")
                { occupied[16] = pawn.EnPassantSquare; break; }
            return occupied;
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
                        if (valid[1] != null && isWhiteTurn && valid[1] == sq) pawn.EnPassantSquare = sq[0] + "3";
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

        public static void ResetEnPassant()
        {
            Pawn[] pawns = isWhiteTurn ? whitePawns : blackPawns;
            foreach (Pawn pawn in pawns)
                if (pawn != null) pawn.EnPassantSquare = "";
        }

        public static void RemoveEnPassantPawn(string captureSquare)
        {
            int i = 0;
            if (isWhiteTurn)
            {
                foreach (Pawn pawn in blackPawns)
                {
                    if (pawn != null && pawn.Position == captureSquare[0] + "5")
                    { blackPawns[i] = null; allPieces[1, i] = null; break; }
                    i++;
                }
            }
            else
            {
                foreach (Pawn pawn in whitePawns)
                {
                    if (pawn != null && pawn.Position == captureSquare[0] + "4")
                    { blackPawns[i] = null; allPieces[0, i] = null; break; }
                    i++;
                }
            }
        }
    }
}
