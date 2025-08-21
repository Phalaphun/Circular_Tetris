using System.IO;
using OpenTK.Graphics.OpenGL;
#if WINDOWS
using System.Drawing;
using System.Drawing.Imaging;
#elif OS_LINUX
using StbImageSharp;
#endif

namespace Kursach
{
    internal class ContentPipe
    {
        public static int LoadTexture(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found at '{path}'");
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

#if OS_WINDOWS

            Bitmap bmp = new Bitmap(path); 
            BitmapData data = bmp.LockBits( new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb); 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,data.Width, data.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Rgba, 
                PixelType.UnsignedByte, 
                data.Scan0); 
            bmp.UnlockBits(data); 
#elif OS_LINUX
            // Load image using StbImageSharp (no vertical flip)
            // StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D,
                          level: 0,
                          internalformat: PixelInternalFormat.Rgba,
                          width: image.Width,
                          height: image.Height,
                          border: 0,
                          format: OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
                          type: PixelType.UnsignedByte,
                          pixels: image.Data);
#endif
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return id;
        }
    }
}
