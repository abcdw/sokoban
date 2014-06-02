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
using System.Media;

namespace sokoban
{
    class Game : GameWindow
    {
        #region fields

        private const int NominalWidth = 600;
        private const int NominalHeight = 600;
        private float ProjectionWidth;
        private float ProjectionHeight;
        private const int BlockSize = 30;

        private Map map;

        private const int ColorsCount = 5;
        private static Color4[] fieldColor = {
            Color4.White,
            Color4.DarkOrange,
            Color4.Brown,
            Color4.Red,
            Color4.BlueViolet,
        };

        private Texture TextureBackground;
        private Texture[] Textures = new Texture[ColorsCount];

        private SoundPlayer backgroundMusic;
        private bool playingMusic;

        #endregion
        [STAThread]

        static void Main(string[] args)
        {
            using (var Game = new Game()) {
                Game.Run(30);
            }
        }

        public Game()
            : base(NominalWidth, NominalHeight, GraphicsMode.Default, "Sokoban")
        {
            VSync = VSyncMode.On;
            map = new Map(20, 20);

            Keyboard.KeyDown +=
                new EventHandler<KeyboardKeyEventArgs>(OnKeyDown);

            //WindowState = WindowState.Fullscreen;
            //Height = 1000;
            //Width = 973;
            //WindowBorder = WindowBorder.Fixed;

            backgroundMusic = new SoundPlayer(sokoban.resources.bacground01);

            ToggleMusic();
            TextureBackground = new Texture(sokoban.resources.background);
            for (int i = 0; i < 5; i++) {
                Textures[i] = new Texture(sokoban.resources.background);
            }

            Textures[(int)Map.FieldType.wall] = new Texture(sokoban.resources.brickwall);
            Textures[(int)Map.FieldType.finish] = new Texture(sokoban.resources.finish);
            Textures[(int)Map.FieldType.block] = new Texture(sokoban.resources.box);
            Textures[(int)Map.FieldType.player] = new Texture(sokoban.resources.player);
            RenderText(sokoban.resources.help);
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
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

        protected void OnKeyDown(object Sender, KeyboardKeyEventArgs E)
        {

            switch (E.Key) {
                case Key.Escape:
                    Exit();
                    break;

                case Key.S:
                    map.save();
                    break;

                case Key.L:
                    map.load();
                    break;

                case Key.M:
                    ToggleMusic();
                    break;
                case Key.R:
                    map.generateField();
                    if (!playingMusic)
                        ToggleMusic();
                    break;
            }

            if (!map.Finished) {

                switch (E.Key) {

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

                if (map.Finished)
                    OnLevelFinish();
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

            //RenderBackground();

            GL.Begin(BeginMode.Quads);

            RenderMap();

            GL.End();
            SwapBuffers();
        }

        private void RenderBackground()
        {
            TextureBackground.Bind();
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.TexCoord2((float)ClientRectangle.Width / TextureBackground.Width, 0);
            GL.Vertex2(ProjectionWidth, 0);

            GL.TexCoord2((float)ClientRectangle.Width / TextureBackground.Width, (float)ClientRectangle.Height / TextureBackground.Height);
            GL.Vertex2(ProjectionWidth, ProjectionHeight);

            GL.TexCoord2(0, (float)ClientRectangle.Height / TextureBackground.Height);
            GL.Vertex2(0, ProjectionHeight);

            GL.End();
        }
        private void RenderMap()
        {
            for (int i = 0; i < map.Height; i++) {
                for (int j = 0; j < map.Width; ++j) {
                    RenderBlock(i, j, map.field[i, j]);
                }
            }
        }

        private void RenderBlock(float X, float Y, int type)
        {
            
            if (type == 0)
                return;

            Textures[type].Bind();
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(X * BlockSize, Y * BlockSize);

            GL.TexCoord2(1, 0);
            GL.Vertex2((X + 1) * BlockSize, Y * BlockSize);

            GL.TexCoord2(1, 1);
            GL.Vertex2((X + 1) * BlockSize, (Y + 1) * BlockSize);

            GL.TexCoord2(0, 1);
            GL.Vertex2(X * BlockSize, (Y + 1) * BlockSize);

            GL.End();
        }

        private void RenderText(String text)
        {
            System.Console.WriteLine(text);
        }

        private void ToggleMusic()
        {
            if (!playingMusic) {
                backgroundMusic.PlayLooping();
            } else {
                backgroundMusic.Stop();
            }

            playingMusic = !playingMusic;
        }

        private void OnLevelFinish()
        {
            if (playingMusic)
                ToggleMusic();

            RenderText("Level complete. Congratulations!");
            SoundPlayer player = new SoundPlayer(sokoban.resources.levelfinished01);
            player.Play();
            
        }
    }
}
