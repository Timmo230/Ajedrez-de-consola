using System.Collections;
using static Chess.Program;

namespace Chess
{
    internal partial class Program
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

        public static Pawn[]   whitePawns   = new Pawn[10];
        public static Pawn[]   blackPawns   = new Pawn[10];
        public static Rook[]   whiteRooks   = new Rook[10];
        public static Rook[]   blackRooks   = new Rook[10];
        public static Knight[] whiteKnights = new Knight[10];
        public static Knight[] blackKnights = new Knight[10];
        public static Bishop[] whiteBishops = new Bishop[10];
        public static Bishop[] blackBishops = new Bishop[10];
        public static King[]   whiteKing    = new King[1];
        public static King[]   blackKing    = new King[1];
        public static Queen[]  whiteQueen   = new Queen[10];
        public static Queen[]  blackQueen   = new Queen[10];

        public static Piece[,] allPieces   = new Piece[12, 10];
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
                        case PieceType p when p == PieceType.Rook   && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Rook   && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen  && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen  && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Bishop && !(bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryMovePieceSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Bishop && !(bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryMovePieceSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;

                        case PieceType p when p == PieceType.Rook   && (bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryCaptureSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Rook   && (bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryCaptureSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && (bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryCaptureSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Knight && (bool)output[2] && !(bool)output[3] && (bool)output[4]:
                            result = TryCaptureSpecificRank(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen  && (bool)output[2] && (bool)output[3] && !(bool)output[4]:
                            result = TryCaptureSpecificFile(output[0].ToString(), (PieceType)output[1], (List<string>)output[5], input); break;
                        case PieceType p when p == PieceType.Queen  && (bool)output[2] && !(bool)output[3] && (bool)output[4]:
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
            Pawn pawnToPromote = null;
            Pawn[] pawns = isWhiteTurn ? whitePawns : blackPawns;
            foreach (Pawn p in pawns)
                if (p != null && p.ReachedPromotion()) { pawnToPromote = p; break; }

            if (pawnToPromote == null) return;

            int globalIndex = 0;
            foreach (Piece p in allPieces) { if (p == pawnToPromote) break; globalIndex++; }

            string pos = pawnToPromote.Position;
            Piece[,] coloredPieces = isWhiteTurn ? whitePieces : blackPieces;
            int colorIndex = 0;
            foreach (Piece p in coloredPieces) { if (p == pawnToPromote) break; colorIndex++; }

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
                    continue;
                }

                repeat = false;
                int typeIndex = 0;

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

        public static void RemoveCaptured(PieceType pieceType, bool isWhite)
        {
            switch (pieceType)
            {
                case PieceType.Rook:   if (isWhite) RemoveCapturedByWhite(whiteRooks);   else RemoveCapturedByBlack(blackRooks);   break;
                case PieceType.Knight: if (isWhite) RemoveCapturedByWhite(whiteKnights); else RemoveCapturedByBlack(blackKnights); break;
                case PieceType.Bishop: if (isWhite) RemoveCapturedByWhite(whiteBishops); else RemoveCapturedByBlack(blackBishops); break;
                case PieceType.Queen:  if (isWhite) RemoveCapturedByWhite(whiteQueen);   else RemoveCapturedByBlack(blackQueen);   break;
                case PieceType.King:   if (isWhite) RemoveCapturedByWhite(whiteKing);    else RemoveCapturedByBlack(blackKing);    break;
                case PieceType.Pawn:   if (isWhite) RemoveCapturedByWhite(whitePawns);   else RemoveCapturedByBlack(blackPawns);   break;
            }
        }

        public static void RemoveCapturedByWhite(Piece[] attackers)
        {
            foreach (Piece attacker in attackers)
            {
                if (attacker == null) continue;
                int i = 0;
                foreach (Piece bp in blackPieces)
                {
                    if (bp != null && bp.Position == attacker.Position)
                    {
                        char initial = blackPieces[i / 10, i % 10]?.PieceInitial ?? ' ';
                        blackPieces[i / 10, i % 10] = null;

                        if      (i / 10 == 0) allPieces[1,  i % 10] = null;
                        else if (i / 10 == 1) allPieces[3,  i % 10] = null;
                        else if (i / 10 == 2) allPieces[5,  i % 10] = null;
                        else if (i / 10 == 3) allPieces[7,  i % 10] = null;
                        else if (i / 10 == 4) allPieces[9,  i % 10] = null;
                        else if (i / 10 == 5) allPieces[11, i % 10] = null;

                        switch (initial)
                        {
                            case 'r': blackRooks[i % 10]   = null; break;
                            case 'b': blackBishops[i % 10] = null; break;
                            case 'n': blackKnights[i % 10] = null; break;
                            case 'q': blackQueen[i % 10]   = null; break;
                            case 'k': blackKing[0]         = null; break;
                            case 'p': blackPawns[i % 10]   = null; break;
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
                foreach (Piece wp in whitePieces)
                {
                    if (wp != null && wp.Position == attacker.Position)
                    {
                        char initial = whitePieces[i / 10, i % 10]?.PieceInitial ?? ' ';
                        whitePieces[i / 10, i % 10] = null;

                        if      (i / 10 == 0) allPieces[0,  i % 10] = null;
                        else if (i / 10 == 1) allPieces[2,  i % 10] = null;
                        else if (i / 10 == 2) allPieces[4,  i % 10] = null;
                        else if (i / 10 == 3) allPieces[6,  i % 10] = null;
                        else if (i / 10 == 4) allPieces[8,  i % 10] = null;
                        else if (i / 10 == 5) allPieces[10, i % 10] = null;

                        switch (initial)
                        {
                            case 'r': whiteRooks[i % 10]   = null; break;
                            case 'b': whiteBishops[i % 10] = null; break;
                            case 'n': whiteKnights[i % 10] = null; break;
                            case 'q': whiteQueen[i % 10]   = null; break;
                            case 'k': whiteKing[0]         = null; break;
                            case 'p': whitePawns[i % 10]   = null; break;
                        }
                        break;
                    }
                    i++;
                }
            }
        }

        public static void InitializeGame()
        {
            whitePawns   = new Pawn[]   { new('p',"A2",true),  new('p',"B2",true),  new('p',"C2",true),  new('p',"D2",true),  new('p',"E2",true),  new('p',"F2",true),  new('p',"G2",true),  new('p',"H2",true),  null, null };
            blackPawns   = new Pawn[]   { new('p',"A7",false), new('p',"B7",false), new('p',"C7",false), new('p',"D7",false), new('p',"E7",false), new('p',"F7",false), new('p',"G7",false), new('p',"H7",false), null, null };
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
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whitePawns[col];   whitePieces[wRow, col] = whitePawns[col]; }   row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackPawns[col];   blackPieces[bRow, col] = blackPawns[col]; }   row++; bRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whiteRooks[col];   whitePieces[wRow, col] = whiteRooks[col]; }   row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackRooks[col];   blackPieces[bRow, col] = blackRooks[col]; }   row++; bRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whiteKnights[col]; whitePieces[wRow, col] = whiteKnights[col]; } row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackKnights[col]; blackPieces[bRow, col] = blackKnights[col]; } row++; bRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = whiteBishops[col]; whitePieces[wRow, col] = whiteBishops[col]; } row++; wRow++;
            for (int col = 0; col < 10; col++) { allPieces[row, col] = blackBishops[col]; blackPieces[bRow, col] = blackBishops[col]; } row++; bRow++;
            allPieces[row++, 0] = whiteKing[0];  whitePieces[wRow++, 0] = whiteKing[0];
            allPieces[row++, 0] = blackKing[0];  blackPieces[bRow++, 0] = blackKing[0];
            allPieces[row++, 0] = whiteQueen[0]; whitePieces[wRow++, 0] = whiteQueen[0];
            allPieces[row++, 0] = blackQueen[0]; blackPieces[bRow++, 0] = blackQueen[0];
        }

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
