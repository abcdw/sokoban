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
        [STAThread]

        static void Main(string[] args)
        {
            using (var Game = new Game())
            {
                Game.Run(30);
            }
        }

        public Game()
            : base(700, 500, GraphicsMode.Default, "Sokoban")
        {
            VSync = VSyncMode.On;
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
            // System.Console.WriteLine(E.Key);
            if (E.Key == Key.Escape) {
                Exit();
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

            RenderBody(1.0f, 1.0f, Color4.Bisque);

            GL.End();
            SwapBuffers();
        }

        private void RenderBody(float X, float Y, Color4 color) {
            GL.Color4(color);
            GL.Vertex2(X * BodySize, Y * BodySize);
            GL.Vertex2((X + 1) * BodySize, Y * BodySize);
            GL.Vertex2((X + 1) * BodySize, (Y + 1) * BodySize);
            GL.Vertex2(X * BodySize, (Y + 1) * BodySize);
        }

        //private Random Rand;

        private const int MapWidth = 7;
        private const int MapHeight = 13;
        private const int NominalWidth = 700;
        private const int NominalHeight = 500;

        private float ProjectionWidth;
        private float ProjectionHeight;
        private const int BodySize = 35;
    }
}
