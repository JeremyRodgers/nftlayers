public static class BackgroundImageGen
{
    private static DirectoryInfo picsDir = new DirectoryInfo(Constants.config.LayerOptions.BgImagePath);
    private static IEnumerable<FileInfo> pics;
    private static Int32 picsCount = 0;
    static BackgroundImageGen()
    {
        Console.WriteLine("BIG: Dir: " + picsDir.FullName);
        pics = picsDir.EnumerateFiles("*.JPG");
        picsCount = pics.Count();
    }

    public static ImageMagick.MagickImage GenerateImage(int width, int height)
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        Int32 picPick = rand.Next(picsCount);

        FileInfo pic = pics.ElementAt(picPick);
        Console.WriteLine("Total pics: " + picsCount + ", pick: " + picPick + ", file: " + pic.FullName);
        var image = new ImageMagick.MagickImage(pic);
        //var matrix = new ImageMagick.MatrixFactory();
        //var cMatrix = matrix.CreateConvolveMatrix(3,
        //0.5, 1.1, 2.0,
        //0.6, 1.1, 2.1,
        //0.7, 1.2, 2.2);
        //image.Convolve(cMatrix);
        image.Resize(new ImageMagick.MagickGeometry(width, height) { IgnoreAspectRatio = true });
        //image.RotationalBlur(rand.Next(360));
        image.MotionBlur(50, 50, rand.Next(360));
        image.ContrastStretch(new ImageMagick.Percentage(20));
        return image;
    }
}