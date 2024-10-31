using ImageMagick;

public static class TextBorder
{
    static TextBorder() { }

    public static MagickImage GenerateBorder(string text)
    {
        using (var image = new MagickImage(new MagickColor("#ff00ff"), 512, 128))
        {
            new Drawables()
              // Draw text on the image
              .FontPointSize(22)
              .Font("Consolas")
              .StrokeColor(new MagickColor("white"))
              .FillColor(MagickColors.Black)
              .TextAlignment(TextAlignment.Center)
              .Text(256, 64, text)
              // Add an ellipse
              //   .StrokeColor(new MagickColor(0, Quantum.Max, 0))
              //   .FillColor(MagickColors.SaddleBrown)
              //   .Ellipse(256, 96, 192, 8, 0, 360)
              .Draw(image);
            return image;
        }
    }
}