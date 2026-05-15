namespace Chess
{
    internal partial class Program
    {
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
    }
}
