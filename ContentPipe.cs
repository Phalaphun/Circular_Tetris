using System.Drawing.Imaging;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
namespace Kursach
{
    internal class ContentPipe{
        static public int LoadTexture(string path){
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found at '{path}'");
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap(path); 
            BitmapData data = bmp.LockBits( new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb); 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,data.Width, data.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Rgba, 
                PixelType.UnsignedByte, 
                data.Scan0); 
            bmp.UnlockBits(data); 
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp); 
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear); 
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear); 
            return id;
        }

    }
}