namespace Kursach
{
    internal  class Block
    {
        internal Position[][] Tiles { get; set; }
        internal Position StartOffset { get; set; }
        public int Id { get; set; }
        internal Position Offset { get => offset; set => offset = value; }

        private int rotationState;
        private Position offset;
        static internal int Width { get; set; }
        static internal int Height { get; set; }
        public Block()
        {
            Offset = new Position(StartOffset.Row, StartOffset.Column);
        }
        public Block(Position[][] Tiles, Position StartOffset, int Id)
        {
            this.Id = Id;
            this.Tiles = Tiles;
            this.StartOffset = StartOffset;
            Offset = new Position(this.StartOffset.Row, this.StartOffset.Column);
        }
        public void Move(int rows, int columns)
        {
            Offset.Row += rows;
            Offset.Column += columns;
        }
        public void Reset()
        {
            rotationState = 0;
            Offset.Row = StartOffset.Row;
            Offset.Column = StartOffset.Column;
        }
        public IEnumerable<Position> TilePositions()
        {   
            foreach(Position p in Tiles[rotationState])
            {
                if(p.Column + Offset.Column >= Width)
                {
                    
                    yield return new Position(p.Row + Offset.Row, p.Column + Offset.Column - Width);
                }
                else if(p.Column + Offset.Column < 0)
                {
                    yield return new Position(p.Row + Offset.Row, p.Column + Offset.Column + Width);
                }
                else
                yield return new Position(p.Row + Offset.Row, p.Column + Offset.Column);
            }
        }
        public void RotateCW()
        {
            if(rotationState !=3)
                rotationState++;
            else { rotationState=0; }
        }
        public void RotateCCW() 
        {
            if(rotationState == 0)
                rotationState = Tiles.Length - 1;
            else
                rotationState--;
        }
    }
}