using System.Drawing;

namespace sokoban
{
    class Map
    {
        private int height;
        private int width;
        public int[,] field;
        private static readonly Point[] dp = {
            new Point(-1,  0),
            new Point( 1,  0),
            new Point( 0, -1),
            new Point( 0,  1),
        };

        private Point player;

        public enum FieldType {
            none,
            block,
            wall,
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
            player = new Point();
            field = new int[height, width];
            field[2, 2] = (int)FieldType.block;
            field[player.X, player.Y] = (int)FieldType.player;
        }

        private bool isInField(Point p)
        {
            return p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;
        }

        private bool isEmptyField(Point p)
        {
            return isInField(p) && field[p.X, p.Y] == (int)FieldType.none;
        }

        private bool isMoveableField(Point p)
        {
            return isInField(p) && field[p.X, p.Y] == (int)FieldType.block;
        }

        public void move(MoveType movement)
        {
            Point newPos = player + (Size)dp[(int)movement];
            Point blockAfter = newPos + (Size)dp[(int)movement];

            if (isMoveableField(newPos) && !isEmptyField(blockAfter))
                return;

            if (!isEmptyField(newPos) && !isMoveableField(newPos))
                return;


            if (isMoveableField(newPos)) {
                field[blockAfter.X, blockAfter.Y] = (int)FieldType.block;
                field[newPos.X,     newPos.Y]     = (int)FieldType.none;
            }

            field[player.X, player.Y] = (int)FieldType.none;
            player += (Size)dp[(int)movement];
            field[player.X, player.Y] = (int)FieldType.player;
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
