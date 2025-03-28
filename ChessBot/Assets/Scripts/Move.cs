namespace Chess
{
    public struct Move
    {
        ushort moveValue;
        const ushort startSquareMask = 0b0000000000111111;
        const ushort targetSquareMask = 0b0000111111000000;
        const ushort flagMask = 0b1111000000000000;

        public Move(int startSquare, int targetSquare)
        {
            moveValue = (ushort)(startSquare | (targetSquare << 6));
        }

        public int StartSquare
        {
            get
            {
                return moveValue & startSquareMask;
            }
        }

        public int TargetSquare
        {
            get
            {
                return (moveValue & targetSquareMask) >> 6;
            }
        }

        public override string ToString()
        {
            return $"Move: StartSquare={StartSquare}, TargetSquare={TargetSquare}";
        }

    }
}
