namespace Kursach{
    internal class BlockQueue{
        private Block[] blocks = new Block[]{
            new Block( // BlockI
                new Position[][]{
                    new Position[] { new(1,0), new(1,1), new(1,2), new(1,3) },
                    new Position[] { new(0,2), new(1,2), new(2,2), new(3,2) },
                    new Position[] { new(2,0), new(2,1), new(2,2), new(2,3) },
                    new Position[] { new(0,1), new(1,1), new(2,1), new(3,1) }
                }, new Position(-1, 3), 1
                ),
            new Block( //J
                new Position[][]{
                    new Position[] {new(0, 0), new(1, 0), new(1, 1), new(1, 2)},
                    new Position[] {new(0, 1), new(0, 2), new(1, 1), new(2, 1)},
                    new Position[] {new(1, 0), new(1, 1), new(1, 2), new(2, 2)},
                    new Position[] {new(0, 1), new(1, 1), new(2, 1), new(2, 0)}
                },new Position(0, 3),2
                ),
            new Block(//L
                new Position[][]{
                    new Position[] {new(0,2), new(1,0), new(1,1), new(1,2)},
                    new Position[] {new(0,1), new(1,1), new(2,1), new(2,2)},
                    new Position[] {new(1,0), new(1,1), new(1,2), new(2,0)},
                    new Position[] {new(0,0), new(0,1), new(1,1), new(2,1)}
                },new Position(0, 3),3
                ),
            new Block(//O
                new Position[][]{
                    new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) },
                    new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) },
                    new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) },
                    new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
                }, new Position(0,4),4
                ),
            new Block( //S
                new Position[][]{
                    new Position[] { new(0,1), new(0,2), new(1,0), new(1,1) },
                    new Position[] { new(0,1), new(1,1), new(1,2), new(2,2) },
                    new Position[] { new(1,1), new(1,2), new(2,0), new(2,1) },
                    new Position[] { new(0,0), new(1,0), new(1,1), new(2,1) }
                },new Position(0,3),5
                ),
            new Block( // T
                new Position[][]{
                    new Position[] {new(0,1), new(1,0), new(1,1), new(1,2)},
                    new Position[] {new(0,1), new(1,1), new(1,2), new(2,1)},
                    new Position[] {new(1,0), new(1,1), new(1,2), new(2,1)},
                    new Position[] {new(0,1), new(1,0), new(1,1), new(2,1)}
                }, new Position(0,3),6
                ),
            new Block( // Z
                new Position[][]{
                    new Position[] {new(0,0), new(0,1), new(1,1), new(1,2)},
                    new Position[] {new(0,2), new(1,1), new(1,2), new(2,1)},
                    new Position[] {new(1,0), new(1,1), new(2,1), new(2,2)},
                    new Position[] {new(0,1), new(1,0), new(1,1), new(2,0)}
                }, new Position(0,3),7
                ),
             new Block(//Bomb
                new Position[][]{
                    new Position[] { new(0,0) },
                    new Position[] { new(0,0) },
                    new Position[] { new(0,0) },
                    new Position[] { new(0,0) }
                }, new Position(0,4),8
                ),
        };
        private Random random = new Random(); 
        public Block NextBlock { get; set; }
        private Block RandomBlock(){
            return blocks[random.Next(blocks.Length)];
        }
        public BlockQueue(){
            NextBlock = RandomBlock();
        }
        public Block GetAndUpdate(){
            Block block = NextBlock; 
            do
            {
                NextBlock = RandomBlock(); 
            } while (block.Id == NextBlock.Id); 
            return block;
        }
    }
}