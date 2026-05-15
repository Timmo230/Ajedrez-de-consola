namespace Chess
{
    internal partial class Program
    {
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
    }
}
