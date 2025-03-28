namespace Chess
{
    public static class Piece
    {
        public const int None = 0;
        public const int Pawn = 1;
        public const int Rook = 2;
        public const int Knight = 3;
        public const int Bishop = 4;
        public const int Queen = 5;
        public const int King = 6;

        public const int White = 8;
        public const int Black = 16;

        const int typeMask = 0b00111;
        const int colorMask = 0b11000;


        public static int Type(int piece)
        {
            return piece & typeMask;
        }

        public static int Color(int piece)
        {
            return piece & colorMask;
        }

        public static int OppositeColor(int color)
        {
            return color == White ? Black : White;
        }
    }
}