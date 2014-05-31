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
            : base(800, 600, GraphicsMode.Default, "Sokoban")
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

            SwapBuffers();
        }
    }
}
