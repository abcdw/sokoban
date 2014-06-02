using System.Drawing;
using System.IO;

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

        public enum FieldType
        {
            none,
            block,
            wall,
            player,
        };

        public enum MoveType
        {
            left,
            right,
            up,
            down,
        }

        public Map(int height, int width)
        {
            this.height = height;
            this.width = width;
            initField();
        }

        public Map()
        {
            this.height = 10;
            this.width = 10;
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
                field[newPos.X, newPos.Y] = (int)FieldType.none;
            }

            field[player.X, player.Y] = (int)FieldType.none;
            player += (Size)dp[(int)movement];
            field[player.X, player.Y] = (int)FieldType.player;
        }

        public void load()
        {

            System.IO.StreamReader file = new System.IO.StreamReader("1.map");
            string[] numbers = file.ReadLine().Split();
            System.Console.WriteLine(numbers[1]);
            height = int.Parse(numbers[0]);
            width = int.Parse(numbers[1]);

            System.Console.WriteLine(height + "  " + width);
            for (int i = 0; i < Height; ++i) {
                numbers = file.ReadLine().Split();
                for (int j = 0; j < Width; ++j) {
                    field[i, j] = int.Parse(numbers[j]);
                    if (field[i, j] == (int)FieldType.player) {
                        player.X = j;
                        player.Y = i;
                    }
                }
            }

            file.Close();
            System.Console.WriteLine("map loaded from 1.map");
        }

        public void save()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("1.map");
            file.WriteLine(Height + " " + Width);
            for (int i = 0; i < Height; ++i) {
                for (int j = 0; j < Width; ++j) {
                    file.Write(field[i, j] + " ");
                }
                file.WriteLine();
            }

            file.Close();
            System.Console.WriteLine("map saved to 1.map");
            //Stream file;
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //saveFileDialog1.Filter = "map files (*.map)|*.map";
            //saveFileDialog1.RestoreDirectory = true ;

            //if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
            //if ((file = saveFileDialog1.OpenFile()) != null) {
            //file.Close();
            //}
            //}

        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }
    }
}
