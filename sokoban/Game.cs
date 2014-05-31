using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace sokoban
{
    class Game : GameWindow
    {
        private const int NominalWidth = 700;
        private const int NominalHeight = 500;

        private float ProjectionWidth;
        private float ProjectionHeight;
        private const int BlockSize = 35;
        private Map map;
        private Color4[] fieldColor = {
            Color4.Black,
            Color4.Brown,
            Color4.Gray,
            Color4.Red,
        };

        [STAThread]

        static void Main(string[] args)
        {
            using (var Game = new Game())
            {
                Game.Run(30);
            }
        }

        public Game()
            : base(NominalWidth, NominalHeight, GraphicsMode.Default, "Sokoban")
        {
            VSync = VSyncMode.On;
            map = new Map();

            Keyboard.KeyDown +=
                new EventHandler<KeyboardKeyEventArgs>(OnKeyDown);
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y,
                    ClientRectangle.Width, ClientRectangle.Height);

            ProjectionWidth = NominalWidth;
            ProjectionHeight = (float)ClientRectangle.Height
                             / (float)ClientRectangle.Width
                             * ProjectionWidth;
            if (ProjectionHeight < NominalHeight) {
                ProjectionHeight = NominalHeight;
                ProjectionWidth = (float)ClientRectangle.Width
                                / (float)ClientRectangle.Height
                                * ProjectionHeight;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs E)
        {
            base.OnUpdateFrame(E);
        }

        protected void OnKeyDown(object Sender, KeyboardKeyEventArgs E) {

            switch (E.Key)
            {
                case Key.Escape:
                    Exit();
                    break;

                case Key.Right:
                    map.move(Map.MoveType.right);
                    break;

                case Key.Left:
                    map.move(Map.MoveType.left);
                    break;

                case Key.Up:
                    map.move(Map.MoveType.up);
                    break;

                case Key.Down:
                    map.move(Map.MoveType.down);
                    break;
            
                default:
                    break;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);

            GL.ClearColor(Color4.AliceBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit
                    | ClearBufferMask.DepthBufferBit);

            var Projection =
                Matrix4.CreateOrthographic(-ProjectionWidth,
                                           -ProjectionHeight, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref Projection);
            GL.Translate(ProjectionWidth / 2, -ProjectionHeight / 2, 0);

            var Modelview = Matrix4.LookAt(Vector3.Zero,
                                           Vector3.UnitZ,
                                           Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref Modelview);

            GL.Begin(BeginMode.Quads);

            RenderMap();

            GL.End();
            SwapBuffers();
        }

        private void RenderMap() 
        {
            for (int i = 0; i < map.Height; i++) {
                for (int j = 0; j < map.Width; ++j) {
                    RenderBlock(i, j, fieldColor[map.field[i, j]]);
                }
            }
        }

        private void RenderBlock(float X, float Y, Color4 color) 
        {
            GL.Color4(color);
            GL.Vertex2(X * BlockSize, Y * BlockSize);
            GL.Vertex2((X + 1) * BlockSize, Y * BlockSize);
            GL.Vertex2((X + 1) * BlockSize, (Y + 1) * BlockSize);
            GL.Vertex2(X * BlockSize, (Y + 1) * BlockSize);
        }

    }
}
