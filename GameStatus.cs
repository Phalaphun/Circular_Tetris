using OpenTK.Mathematics;
namespace Kursach{
    internal class GameStatus{
        private Block currentBlock;
        private int width, height;
        public Block CurrentBlock{
            get { return currentBlock; }
            set{currentBlock = value;currentBlock.Reset(); 
            }
        }
        public CircleCells CircleCell{ get; }
        public BlockQueue BQ { get; }
        public bool GameOver { get; set; }
        public int Scores { get; set; }
        public GameStatus(int height, int width, Vector2 centerPoint,int r, int dr ){
            CircleCell = new CircleCells(height, width, centerPoint, r, dr);
            this.width = width;
            this.height = height;
            BQ = new BlockQueue();
            CurrentBlock = BQ.GetAndUpdate();
            Block.Width = width;
        }
        private bool BlockFits(){
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!CircleCell.Empty(p.Row, p.Column))
                    return false;
                if (CurrentBlock.Offset.Column + p.Column >= width)
                    CurrentBlock.Offset.Column -= width;
                else if(p.Column + CurrentBlock.Offset.Column < 0)
                    CurrentBlock.Offset.Column += width;
            }
            return true;
        }
        public void RotateBlockCW(){
            CurrentBlock.RotateCW();
            if (!BlockFits())
                CurrentBlock.RotateCCW();
        }
        public void MoveBlockLeft(){
            CurrentBlock.Move(0, -1);
            if (!BlockFits())
                CurrentBlock.Move(0, 1);
        }
        public void MoveBlockRight(){
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
                CurrentBlock.Move(0, -1);
        }
        private bool IsGameOver(){
            return !(CircleCell.RowEmptyChecker(0) && CircleCell.RowEmptyChecker(1));
        }
        private void PlaceBlock(){
            foreach (Position p in CurrentBlock.TilePositions())
                CircleCell[p.Row, p.Column] = CurrentBlock.Id;
            if (currentBlock.Id == 8)
            {
                foreach (Position p in CurrentBlock.TilePositions())
                {

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (p.Row+i>=0 && p.Row+i< height)
                            {
                                if (p.Column + j >= width)
                                    CircleCell[p.Row + i, p.Column + j - width] = 0;
                                else if (p.Column + j < 0)
                                    CircleCell[p.Row + i, p.Column + j + width] = 0;
                                else
                                    CircleCell[p.Row + i, p.Column + j] = 0;
                            }
                        }
                    }
                }
            }
            Scores = CircleCell.ClearAllRowsFull();
            if (IsGameOver())
                GameOver = true;
            else
                CurrentBlock = BQ.GetAndUpdate();
        }
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}