using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
namespace Kursach{
    internal class CircleCells : IDisposable{
        private Cell[][] cells;
        internal Cell[][] Cells { get => cells; set => cells = value; }
        public CircleCells(int height, int width, Vector2 Center, int r, int dr){
            Cells = new Cell[height][];
            for (int i = height - 1; i >= 0; i--)
            {
                Cells[i] = new Cell[width];
            }
            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    Cells[i][j] = new Cell(Center, j, i, r, dr, width);
                }
            }
            this.rows = height;
            this.columns = width;
        }
        private int rows, columns;
        public int Cleared { get; set; }
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }
        public int this[int r, int c] { get => this.cells[r][c].Id; set => this.cells[r][c].Id = value; }
        public bool Insider(int r, int c){
            return (r >= 0 && r < Rows) && (c >= 0 && c < Columns);
        }
        public bool Empty(int r, int c){
            return Insider(r, c) && cells[r][c].Id == 0;
        }
        public bool RowFullChecker(int r){
            for (int c = 0; c < Columns; c++)
                if (cells[r][c].Id == 0)
                    return false;
            return true;
        }
        public bool RowEmptyChecker(int r){
            for (int c = 0; c < Columns; c++)
                if (cells[r][c].Id != 0)
                    return false;
            return true;
        }
        private void ClearRow(int r){
            for (int c = 0; c < Columns; c++)
                cells[r][c].Id = 0;
        }
        private void MoveRowDown(int r, int numRows){
            for (int c = 0; c < Columns; c++){
                if (r == Rows - 1)
                    cells[r][c].Id = 0;
                else
                {
                    cells[r + numRows][c].Id = cells[r][c].Id;
                    cells[r][c].Id = 0;
                }
            }
        }
        public int ClearRow(){
            int cleared = 0; 
            for (int r = Rows - 1; r > 0; r--) 
            {
                if (RowFullChecker(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                if (cleared > 0)
                    MoveRowDown(r - 1, cleared);
            }
            return cleared;
        }
        public int ClearAllRowsFull(){
            int temp = 0;
            while (RowFullCheckerAll())
                temp += ClearRow();
            if (temp >= 4)
            {
                Cleared = 2 * temp + Cleared;
                return Cleared;
            }
            else
                Cleared += temp;
            return Cleared;
        }
        private bool RowFullCheckerAll(){
            bool temp = false;
            for (int r = Rows - 1; r > 0; r--){
                temp = RowFullChecker(r);
                if (temp)
                    break;
            }
            return temp;
        }
        public void Draw(Vector3[] ColorMass){
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < columns; j++){
                    GL.Color3(ColorMass[cells[i][j].Id]);
                    cells[i][j].Draw();
                }
            }
        }
        public void Dispose(){
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < columns; j++)
                    Cells[i][j].Dispose();
            }
        }
    }
    internal class Cell : IDisposable
    {
        float x2, y2, x3, y3, x4, y4, x5, y5;
        int id, bufferId; float[] temp;
        public int Id { get { return id; } set { id = value; } }
        public Cell(Vector2 Center, int j, int i, int r, int dr, int width){
            float dAlpha = 2 * (float)Math.PI / width;
            x2 = Center.X + (r + i * dr) * (float)Math.Cos(j * dAlpha); // i- номер круга, j- число блоков в круге
            y2 = Center.Y + (r + i * dr) * (float)Math.Sin(j * dAlpha);

            x3 = Center.X + (r + i * dr + dr) * (float)Math.Cos(j * dAlpha);
            y3 = Center.Y + (r + i * dr + dr) * (float)Math.Sin(j * dAlpha);

            x4 = Center.X + (r + i * dr + dr) * (float)Math.Cos((j + 1) * dAlpha);
            y4 = Center.Y + (r + i * dr + dr) * (float)Math.Sin((j + 1) * dAlpha);

            x5 = Center.X + (r + i * dr) * (float)Math.Cos((j + 1) * dAlpha);
            y5 = Center.Y + (r + i * dr) * (float)Math.Sin((j + 1) * dAlpha);

            temp = new float[] { x2, y2, x3, y3, x4, y4, x5, y5 };
            bufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, temp.Length * sizeof(float), temp, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void Draw(){
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.VertexPointer(2, VertexPointerType.Float, 0, 0);
            GL.DrawArrays(PrimitiveType.Polygon, 0, 4);
            GL.Color4(Color4.Red);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
        public void Dispose(){
            GL.DeleteBuffer(bufferId);
        }
    }
}