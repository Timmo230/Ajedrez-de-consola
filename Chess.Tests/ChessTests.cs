using Chess;
using Xunit;
using static Chess.Program;

namespace Chess.Tests
{
    public class ChessTests
    {
        private static void Setup() => InitializeGame();

        // ── Initialization ───────────────────────────────────────────────

        [Fact]
        public void InitializeGame_PlacesCorrectNumberOfPieces()
        {
            Setup();
            Assert.Equal(8, whitePawns.Count(p => p != null));
            Assert.Equal(8, blackPawns.Count(p => p != null));
            Assert.Equal(2, whiteRooks.Count(p => p != null));
            Assert.Equal(2, blackRooks.Count(p => p != null));
            Assert.Equal(2, whiteKnights.Count(p => p != null));
            Assert.Equal(2, blackKnights.Count(p => p != null));
            Assert.Equal(2, whiteBishops.Count(p => p != null));
            Assert.Equal(2, blackBishops.Count(p => p != null));
            Assert.NotNull(whiteKing[0]);
            Assert.NotNull(blackKing[0]);
            Assert.NotNull(whiteQueen[0]);
            Assert.NotNull(blackQueen[0]);
        }

        [Fact]
        public void InitializeGame_PiecesOnCorrectSquares()
        {
            Setup();
            Assert.Equal("E1", whiteKing[0].Position);
            Assert.Equal("E8", blackKing[0].Position);
            Assert.Equal("D1", whiteQueen[0].Position);
            Assert.Equal("D8", blackQueen[0].Position);
            Assert.Equal("A1", whiteRooks[0].Position);
            Assert.Equal("H1", whiteRooks[1].Position);
            Assert.Equal("A8", blackRooks[0].Position);
            Assert.Equal("H8", blackRooks[1].Position);
        }

        [Fact]
        public void InitializeGame_AllPiecesArrayContainsAllPieces()
        {
            Setup();
            int count = 0;
            foreach (Piece p in allPieces) if (p != null) count++;
            Assert.Equal(32, count);
        }

        // ── Pawn movement ─────────────────────────────────────────────────

        [Fact]
        public void Pawn_CanMoveOneSquareForward_White()
        {
            Setup();
            var pawn = whitePawns[4]; // E2
            var valid = pawn.ValidSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.Contains("E3", valid);
        }

        [Fact]
        public void Pawn_CanMoveTwoSquaresFromStart_White()
        {
            Setup();
            var pawn = whitePawns[4]; // E2
            var valid = pawn.ValidSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.Contains("E4", valid);
        }

        [Fact]
        public void Pawn_CannotMoveTwoSquares_AfterFirstMove()
        {
            Setup();
            var pawn = whitePawns[4]; // E2
            pawn.Position = "E3";
            var valid = pawn.ValidSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.DoesNotContain("E5", valid);
        }

        [Fact]
        public void Pawn_CannotMoveForward_WhenBlocked()
        {
            Setup();
            var pawn = whitePawns[4]; // E2
            pawn.Position = "E6";
            // black pawn at E7 blocks
            var valid = pawn.ValidSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.DoesNotContain("E7", valid);
        }

        [Fact]
        public void Pawn_CanCaptureDiagonally_White()
        {
            Setup();
            var pawn = whitePawns[3]; // D2
            pawn.Position = "D6";
            // black pawn at E7 is capturable
            var captures = pawn.Capture(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.Contains("E7", captures);
        }

        [Fact]
        public void Pawn_BlackMovesDown()
        {
            Setup();
            var pawn = blackPawns[4]; // E7
            var valid = pawn.ValidSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.Contains("E6", valid);
            Assert.Contains("E5", valid);
        }

        // ── Rook movement ─────────────────────────────────────────────────

        [Fact]
        public void Rook_CanMoveAlongRanksAndFiles()
        {
            var rook = new Rook('r', "D4", true);
            string[] occupied = { "D4" };
            var valid = rook.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            Assert.Contains("D5", valid);
            Assert.Contains("D3", valid);
            Assert.Contains("E4", valid);
            Assert.Contains("C4", valid);
        }

        [Fact]
        public void Rook_CannotJumpOverPieces()
        {
            var rook = new Rook('r', "A1", true);
            string[] occupied = { "A1", "A4" };
            var valid = rook.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            Assert.DoesNotContain("A5", valid);
            Assert.DoesNotContain("A6", valid);
        }

        [Fact]
        public void Rook_CanCaptureEnemyPiece()
        {
            var rook = new Rook('r', "A1", true);
            string[] occupied = { "A1", "A5" };
            string[] blackSquares = { "A5" };
            var captures = rook.Capture(occupied, blackSquares, new[] { "A1" }, null);
            Assert.Contains("A5", captures);
        }

        [Fact]
        public void Rook_CannotCaptureOwnPiece()
        {
            var rook = new Rook('r', "A1", true);
            string[] occupied = { "A1", "A5" };
            string[] whiteSquares = { "A1", "A5" };
            var captures = rook.Capture(occupied, Array.Empty<string>(), whiteSquares, null);
            Assert.DoesNotContain("A5", captures);
        }

        // ── Bishop movement ───────────────────────────────────────────────

        [Fact]
        public void Bishop_CanMoveDiagonally()
        {
            var bishop = new Bishop('b', "D4", true);
            string[] occupied = { "D4" };
            var valid = bishop.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            Assert.Contains("E5", valid);
            Assert.Contains("C3", valid);
            Assert.Contains("E3", valid);
            Assert.Contains("C5", valid);
        }

        [Fact]
        public void Bishop_CannotJumpOverPieces()
        {
            var bishop = new Bishop('b', "A1", true);
            string[] occupied = { "A1", "C3" };
            var valid = bishop.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            Assert.DoesNotContain("D4", valid);
            Assert.DoesNotContain("E5", valid);
        }

        // ── Knight movement ───────────────────────────────────────────────

        [Fact]
        public void Knight_CanJumpOverPieces()
        {
            Setup();
            var knight = whiteKnights[0]; // B1
            // board is full but knight can jump
            var valid = knight.ValidSquares(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares(), null);
            Assert.Contains("A3", valid);
            Assert.Contains("C3", valid);
        }

        [Fact]
        public void Knight_MovesInLShape()
        {
            var knight = new Knight('n', "D4", true);
            string[] occupied = { "D4" };
            var valid = knight.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            Assert.Contains("E6", valid);
            Assert.Contains("C6", valid);
            Assert.Contains("F5", valid);
            Assert.Contains("B5", valid);
            Assert.Contains("F3", valid);
            Assert.Contains("B3", valid);
            Assert.Contains("E2", valid);
            Assert.Contains("C2", valid);
        }

        // ── Queen movement ────────────────────────────────────────────────

        [Fact]
        public void Queen_CombinesRookAndBishopMovement()
        {
            var queen = new Queen('q', "D4", true);
            string[] occupied = { "D4" };
            var valid = queen.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            // rook-like
            Assert.Contains("D5", valid);
            Assert.Contains("D3", valid);
            Assert.Contains("E4", valid);
            // bishop-like
            Assert.Contains("E5", valid);
            Assert.Contains("C3", valid);
        }

        // ── King movement ─────────────────────────────────────────────────

        [Fact]
        public void King_MovesOneSquareInAnyDirection()
        {
            var king = new King('k', "D4", true);
            string[] occupied = { "D4" };
            var valid = king.ValidSquares(occupied, Array.Empty<string>(), Array.Empty<string>(), null);
            Assert.Contains("D5", valid);
            Assert.Contains("D3", valid);
            Assert.Contains("E4", valid);
            Assert.Contains("C4", valid);
            Assert.Contains("E5", valid);
            Assert.Contains("C3", valid);
            Assert.Contains("E3", valid);
            Assert.Contains("C5", valid);
        }

        [Fact]
        public void King_CannotMoveIntoAttackedSquare()
        {
            var king = new King('k', "E1", true);
            string[] occupied = { "E1" };
            // black rook attacks E-file
            List<string> attacked = new List<string> { "E2", "E3", "E4", "E5", "E6", "E7", "E8" };
            var valid = king.ValidSquares(occupied, Array.Empty<string>(), new[] { "E1" }, attacked);
            Assert.DoesNotContain("E2", valid);
        }

        // ── Piece removal ─────────────────────────────────────────────────

        [Fact]
        public void RemoveCaptured_RemovesPieceFromAllArrays()
        {
            Setup();
            isWhiteTurn = true;
            // Move white rook to A8 (where black rook is) to simulate capture
            whiteRooks[0].Position = "A8";
            RemoveCaptured(PieceType.Rook, true);

            Assert.Null(blackRooks[0]);
            Assert.Null(allPieces[3, 0]); // row 3 = black rooks
            Assert.Null(blackPieces[1, 0]); // row 1 in blackPieces = black rooks
        }

        [Fact]
        public void RemoveCaptured_RemovesFromBlackArrays_WhenBlackCaptures()
        {
            Setup();
            isWhiteTurn = false;
            // Move black rook to A1 (where white rook is)
            blackRooks[0].Position = "A1";
            RemoveCaptured(PieceType.Rook, false);

            Assert.Null(whiteRooks[0]);
        }

        // ── The check/capture bug fix (commit "Arreglo de errores al comer con jaque") ──

        [Fact]
        public void ValidateMove_CapturedPieceExcludedFromCheckCalculation()
        {
            // Scenario: Black queen at C3 attacks E1 diagonally (C3-D2-E1)
            // White pawn at D2 blocks the diagonal
            // White king at E1
            // White pawn captures black queen (D2xC3)
            // After capture: queen gone, king NOT in check → move must be valid

            Setup();
            isWhiteTurn = true;

            // Clear the board except for our 3 pieces
            for (int r = 0; r < 12; r++)
                for (int c = 0; c < 10; c++)
                    allPieces[r, c] = null;
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 10; c++)
                {
                    whitePieces[r, c] = null;
                    blackPieces[r, c] = null;
                }

            // Place white king E1
            whiteKing[0] = new King('k', "E1", true);
            allPieces[8, 0] = whiteKing[0];
            whitePieces[4, 0] = whiteKing[0];

            // Place white pawn D2
            whitePawns = new Pawn[10];
            var wPawn = new Pawn('p', "D2", true);
            whitePawns[0] = wPawn;
            allPieces[0, 0] = wPawn;
            whitePieces[0, 0] = wPawn;

            // Place black queen C3
            blackQueen = new Queen[10];
            var bQueen = new Queen('q', "C3", false);
            blackQueen[0] = bQueen;
            allPieces[11, 0] = bQueen;
            blackPieces[5, 0] = bQueen;

            // Also set arrays for pieces not on board
            blackPawns = new Pawn[10];
            blackRooks = new Rook[10];
            blackKnights = new Knight[10];
            blackBishops = new Bishop[10];
            blackKing = new King[] { new King('k', "H8", false) };
            allPieces[9, 0] = blackKing[0];
            blackPieces[3, 0] = blackKing[0];
            whiteRooks = new Rook[10];
            whiteKnights = new Knight[10];
            whiteBishops = new Bishop[10];
            whiteQueen = new Queen[10];

            // White pawn captures black queen: pawn moves from D2 to C3
            capturedPieceCopy = null;
            bool valid = ValidateMove(wPawn, "C3");

            // Move must be valid: after capture, black queen is excluded from
            // attacked squares calculation, so king at E1 is NOT in check
            Assert.True(valid);
        }

        [Fact]
        public void ValidateMove_MoveIntoCheckIsRejected()
        {
            Setup();
            isWhiteTurn = true;

            // Clear board except king and enemy rook
            for (int r = 0; r < 12; r++)
                for (int c = 0; c < 10; c++) allPieces[r, c] = null;
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 10; c++) { whitePieces[r, c] = null; blackPieces[r, c] = null; }

            whiteKing[0] = new King('k', "E1", true);
            allPieces[8, 0] = whiteKing[0];
            whitePieces[4, 0] = whiteKing[0];

            blackKing = new King[] { new King('k', "H8", false) };
            allPieces[9, 0] = blackKing[0];
            blackPieces[3, 0] = blackKing[0];

            // Black rook on E8 controls E-file
            blackRooks = new Rook[10];
            var bRook = new Rook('r', "E8", false);
            blackRooks[0] = bRook;
            allPieces[3, 0] = bRook;
            blackPieces[1, 0] = bRook;

            whitePawns = new Pawn[10];
            whiteRooks = new Rook[10];
            whiteKnights = new Knight[10];
            whiteBishops = new Bishop[10];
            whiteQueen = new Queen[10];
            blackPawns = new Pawn[10];
            blackKnights = new Knight[10];
            blackBishops = new Bishop[10];
            blackQueen = new Queen[10];

            capturedPieceCopy = null;
            // King tries to move to E2 — attacked by rook on E-file
            bool valid = ValidateMove(whiteKing[0], "E2");
            Assert.False(valid);
        }

        // ── Checkmate ─────────────────────────────────────────────────────

        [Fact]
        public void IsCheckmate_DetectsBackrankMate()
        {
            // White king at H1, black rooks at A1 and A2 → checkmate
            Setup();
            isWhiteTurn = true;

            for (int r = 0; r < 12; r++)
                for (int c = 0; c < 10; c++) allPieces[r, c] = null;
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 10; c++) { whitePieces[r, c] = null; blackPieces[r, c] = null; }

            whiteKing[0] = new King('k', "H1", true);
            allPieces[8, 0] = whiteKing[0];
            whitePieces[4, 0] = whiteKing[0];

            blackKing = new King[] { new King('k', "H8", false) };
            allPieces[9, 0] = blackKing[0];
            blackPieces[3, 0] = blackKing[0];

            blackRooks = new Rook[10];
            var bRook1 = new Rook('r', "A1", false);
            var bRook2 = new Rook('r', "A2", false);
            blackRooks[0] = bRook1;
            blackRooks[1] = bRook2;
            allPieces[3, 0] = bRook1;
            allPieces[3, 1] = bRook2;
            blackPieces[1, 0] = bRook1;
            blackPieces[1, 1] = bRook2;

            whitePawns = new Pawn[10];
            whiteRooks = new Rook[10];
            whiteKnights = new Knight[10];
            whiteBishops = new Bishop[10];
            whiteQueen = new Queen[10];
            blackPawns = new Pawn[10];
            blackKnights = new Knight[10];
            blackBishops = new Bishop[10];
            blackQueen = new Queen[10];

            bool gameOn = IsCheckmate(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares());
            Assert.False(gameOn); // false = checkmate = game over
        }

        [Fact]
        public void IsCheckmate_NotCheckmateWhenEscapeExists()
        {
            Setup();
            isWhiteTurn = true;

            for (int r = 0; r < 12; r++)
                for (int c = 0; c < 10; c++) allPieces[r, c] = null;
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 10; c++) { whitePieces[r, c] = null; blackPieces[r, c] = null; }

            whiteKing[0] = new King('k', "E1", true);
            allPieces[8, 0] = whiteKing[0];
            whitePieces[4, 0] = whiteKing[0];

            blackKing = new King[] { new King('k', "H8", false) };
            allPieces[9, 0] = blackKing[0];
            blackPieces[3, 0] = blackKing[0];

            // Single black rook on A1 — king can escape many ways
            blackRooks = new Rook[10];
            var bRook = new Rook('r', "A1", false);
            blackRooks[0] = bRook;
            allPieces[3, 0] = bRook;
            blackPieces[1, 0] = bRook;

            whitePawns = new Pawn[10];
            whiteRooks = new Rook[10];
            whiteKnights = new Knight[10];
            whiteBishops = new Bishop[10];
            whiteQueen = new Queen[10];
            blackPawns = new Pawn[10];
            blackKnights = new Knight[10];
            blackBishops = new Bishop[10];
            blackQueen = new Queen[10];

            bool gameOn = IsCheckmate(AllOccupiedSquares(), BlackOccupiedSquares(), WhiteOccupiedSquares());
            Assert.True(gameOn); // true = game continues
        }
    }
}
