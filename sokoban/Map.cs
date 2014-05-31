namespace sokoban
{
    class Map
    {
        private int height;
        private int width;
        private int[,] field;
        private const int[] dx = { -1, 1,  0, 0, };
        private const int[] dy = {  0, 0, -1, 1, };
        private int playerX;
        private int playerY;

        public enum FieldType {
            none,
            wall,
            block,
            player,
        };

        public enum MoveType {
            left,
            right,
            up,
            down,
        }

        public Map(int height, int width)
        {
            this.height = height;
            this.width  = width;
            initField();
        }

        public Map()
        {
            this.height = 10;
            this.width  = 10;
            initField();
        }

        private void initField()
        {
            field = new int[height, width];
            //field[2, 2] = (int)FieldType.block;
            field[playerX, playerY] = FieldType.player;
            System.Console.WriteLine("field[2, 2]: " + field[2, 2]);
        }

        public void move(MoveType movement)
        {
            field[playerX, playerY] = FieldType.none;
            field[playerX + dx[(int)movement],
                  playerY + dy[(int)movement]] = FieldType.player;
        }

        public void load()
        {

        }

        public void save()
        {

        }

        public int Height {
            get { return height; }
        }

        public int Width {
            get { return width; }
        }
    }
}
