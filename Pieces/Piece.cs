namespace Chess
{
    public class Piece
    {
        public enum Operation { Add, Subtract, Column, Row }

        public char PieceInitial { get; set; }
        public string Position { get; set; }
        public bool IsWhite { get; set; }
        public byte PointValue { get; set; }

        protected virtual string[] Move() => null;

        protected int ColumnToInt(char column)
        {
            return column == 'A' ? 1 : column == 'B' ? 2 : column == 'C' ? 3 : column == 'D' ? 4
                 : column == 'E' ? 5 : column == 'F' ? 6 : column == 'G' ? 7 : column == 'H' ? 8 : 0;
        }

        protected char IntToColumn(int number)
        {
            return number == 1 ? 'A' : number == 2 ? 'B' : number == 3 ? 'C' : number == 4 ? 'D'
                 : number == 5 ? 'E' : number == 6 ? 'F' : number == 7 ? 'G' : number == 8 ? 'H' : '0';
        }

        public virtual List<string> ValidSquares(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
            => null;

        public virtual List<string> Capture(string[] occupiedSquares, string[] blackSquares, string[] whiteSquares, List<string> attackedSquares)
            => null;
    }
}
