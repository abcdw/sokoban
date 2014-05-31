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
            player = new Point();
            field = new int[height, width];
            field[2, 2] = (int)FieldType.wall;
            field[player.X, player.Y] = (int)FieldType.player;
        }

        private bool isValidField(Point p)
        {

            return true;
        }

        public void move(MoveType movement)
        {

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
