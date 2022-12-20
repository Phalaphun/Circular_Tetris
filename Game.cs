using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace Kursach
{
    internal class Game : GameWindow
    {
        int width, height, previouseScores, r, dr, textureId;
        double fiX, fiY, ortoWidth, ortoHeight, lag = 0, TIME_PER_FRAME = 0.45;
        bool pause=false;
        GameStatus gameState;
        Vector2 cursorPosition, centerPoint;
        Button restart, close;
        List<Button> buttons = new List<Button>() {};
        TextRenderer tr1,tr2,tr3,tr4;
        private Vector3[] ColorMass = new Vector3[]{
            new Vector3(0,0,0), //black
            new Vector3(0,255/255f,255/255f), //cyan
            new Vector3(0f,0f,255/255f), //blue
            new Vector3(255/255f,165/255f,0), //orange
            new Vector3(255/255f,255/255f,0), //yellow
            new Vector3(0,128/255f,0),//green
            new Vector3(238/255f,130/255f,238/255f),//purpule
            new Vector3(255/255f,0,0),//red
            new Vector3(1,1,1),//white
        };
        
        public Game(GameWindowSettings gSettings, NativeWindowSettings nSettings) : base(gSettings, nSettings){
            VSync = VSyncMode.On;
        }
        protected override void OnLoad(){
            base.OnLoad();
            width = 12;  
            height = 22; 
            ortoHeight = 1200;
            ortoWidth = 1600;
            r = 60;
            dr = 20;
            centerPoint = new Vector2(510, 510);
            GL.MatrixMode(MatrixMode.Projection); 
            GL.LoadIdentity(); 
            GL.Ortho(0, ortoWidth, 0, ortoHeight, -1, 1); 
            GL.MatrixMode(MatrixMode.Modelview); 
            gameState = new GameStatus(height, width, centerPoint, r, dr);
            textureId = ContentPipe.LoadTexture(@"Content\Consolas_Alpha_W.png");
            tr1 = new TextRenderer(16, 16, textureId, (float)ortoWidth, (float)ortoHeight);
            tr2 = new TextRenderer(16, 16, textureId, (float)ortoWidth, (float)ortoHeight);
            tr3 = new TextRenderer(16, 16, textureId, (float)ortoWidth, (float)ortoHeight);
            tr4 = new TextRenderer(16, 16, textureId, (float)ortoWidth, (float)ortoHeight);
            restart = new Button(1200, 800, 300, 200, Color4.Gray); close = new Button(1200, 500, 300, 200, Color4.Gray);
            buttons.Add(restart);
            buttons.Add(close);
            close.OnMouseDown += CloseEvent;
            restart.OnMouseDown += Restart;
            tr1.PrepareText(1280, 880, 20, "RESTART", 0.9f);
            tr1.PrepareText(1280, 580, 25, "CLOSE", 0.9f);
            tr4.PrepareText(100, 1100, 25, "SCORE:" + gameState.Scores.ToString(), 0.9f);
            tr2.PrepareText(900, 950, 37, "paused", 3f);
            tr3.PrepareText(640, 530, 25, "GAME OVER", 0.9f);
        }
        protected override void OnUnload(){
            gameState.CircleCell.Dispose();
            tr1.Dispose(); tr2.Dispose(); tr3.Dispose(); tr4.Dispose();
            base.OnUnload();
        }
        protected override void OnRenderFrame(FrameEventArgs args){
            base.OnRenderFrame(args);
            GL.ClearColor(Color4.Black); 
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); 
            DrawAll(gameState);
            if (pause) { GL.Color4(Color4.White); tr2.RenderText(); }
            if(gameState.GameOver)
            {
                GL.Color4(Color4.SlateGray);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(500, 300);
                GL.Vertex2(500, 300 + 500);
                GL.Vertex2(500 + 500, 300 + 500);
                GL.Vertex2(500 + 500, 300);
                GL.End();
                GL.Color4(Color4.White);
                tr3.RenderText();
            }
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs args){
            if (!pause)
            {
                if (!gameState.GameOver) 
                {
                    lag += args.Time;
                    
                    if (lag > TIME_PER_FRAME)
                    {
                        while (lag > TIME_PER_FRAME)
                        {
                            if (previouseScores < gameState.Scores && TIME_PER_FRAME - 0.03 > 0.03)
                                TIME_PER_FRAME -= 0.03;
                            previouseScores = gameState.Scores;
                            lag -= TIME_PER_FRAME;
                            gameState.MoveBlockDown();
                            this.Title = "Tetris        Scores: " + gameState.Scores.ToString();
                        }
                    }

                } 
            }
            base.OnUpdateFrame(args);   
        }
        protected override void OnResize(ResizeEventArgs e){
            fiX = e.Width / ortoWidth; fiY = e.Height / ortoHeight;
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e){
            base.OnKeyDown(e);
            if (gameState.GameOver)
                return;
            else
            {
                switch (e.Key)
                {
                    case Keys.Left: gameState.MoveBlockLeft(); break;
                    case Keys.Right: gameState.MoveBlockRight(); break;
                    case Keys.Up: gameState.RotateBlockCW(); break;
                    case Keys.Down: gameState.MoveBlockDown(); break;
                    case Keys.P:; pause = pause ? false : true; break;
                }
            }
        }
        protected override void OnMouseDown(MouseButtonEventArgs e){
            base.OnMouseDown(e);
            for (int i = 0; i < buttons.Count; i++)
                if (buttons[i].IsPointInFigure(cursorPosition))
                    buttons[i].OnMouseDown?.Invoke();
        }
        protected override void OnMouseMove(MouseMoveEventArgs e){
            base.OnMouseMove(e);
            cursorPosition = new Vector2((float)(e.Position.X/fiX), (float)ortoHeight - (float)(e.Position.Y/fiY));
        }
        private void DrawAll(GameStatus gameState){
            gameState.CircleCell.Draw(ColorMass);
            foreach (Position p in gameState.CurrentBlock.TilePositions())
            {
                GL.Color3(ColorMass[gameState.CurrentBlock.Id]); 
                gameState.CircleCell.Cells[p.Row][p.Column].Draw(); 
            }
            foreach (Button butt in buttons)
                butt.Draw();
            GL.Color4(Color4.White);
            tr1.RenderText();
            tr4.RenderText();
            if (previouseScores < gameState.Scores)
                tr4.Update(100, 1100, 25, "SCORE:" + gameState.Scores.ToString(), 0.9f);
        }
        private void Restart(){
            gameState = new GameStatus(height, width, centerPoint, r, dr);
            tr4.Update(100, 1100, 25, "SCORE:" + gameState.Scores.ToString(), 0.9f);
            GC.Collect();
        }
        private void CloseEvent(){
            this.Close();
        }
    }
}