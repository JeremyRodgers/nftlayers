using ImageMagick;
public static class Stacker
{
    static MagickImage headImg;
    static MagickImage headBgImg;
    static MagickImage blankImg;

    static MagickImage[] tops = new MagickImage[Constants.variants[Constants.tops]];
    static MagickImage[] necks = new MagickImage[Constants.variants[Constants.necks]];
    static MagickImage[] contentTops = new MagickImage[Constants.variants[Constants.contentTops]];
    static MagickImage[] contentBottoms = new MagickImage[Constants.variants[Constants.contentBottoms]];
    static MagickGeometry resizeGeometry;
    static List<string> colors = Constants.GetPalette(Constants.config.LayerOptions.BgColorPalette);
    static Random rand;

    static Stacker()
    {
        rand = new Random((int)DateTime.Now.Ticks);

        blankImg = new MagickImage(Constants.blankFile);
        blankImg.Crop(Constants.cropGeom);
        blankImg.RePage();
        var headFile = new FileInfo(Path.Join(Constants.baseDir, Constants.heads + "-1" + Constants.imgExt));
        headImg = new MagickImage(headFile);
        headImg.Crop(Constants.cropGeom);
        headImg.RePage();
        var headBgFile = new FileInfo(Path.Join(Constants.baseDir, Constants.heads + "bg-1" + Constants.imgExt));
        headBgImg = new MagickImage(headBgFile);
        headBgImg.Crop(Constants.cropGeom);
        headBgImg.RePage();
        for (int idx = 0; idx < tops.Length; idx++)
        {
            var topFile = new FileInfo(Path.Join(Constants.baseDir, Constants.tops + "-" + (idx + 1).ToString() + Constants.imgExt));
            var topImg = new MagickImage(topFile);
            topImg.Crop(Constants.cropGeom);
            topImg.RePage();
            tops[idx] = topImg;
        }
        for (int idx = 0; idx < necks.Length; idx++)
        {
            var neckFile = new FileInfo(Path.Join(Constants.baseDir, Constants.necks + "-" + (idx + 1).ToString() + Constants.imgExt));
            var neckImg = new MagickImage(neckFile);
            neckImg.Crop(Constants.cropGeom);
            neckImg.RePage();
            //neckImg.TransparentChroma(MagickColors.Gray, MagickColors.White);
            necks[idx] = neckImg;
        }
        for (int idx = 0; idx < contentTops.Length; idx++)
        {
            var contentTopFile = new FileInfo(Path.Join(Constants.baseDir, Constants.contentTops + "-" + (idx + 1).ToString() + Constants.imgExt));
            var contentTopImg = new MagickImage(contentTopFile);
            contentTopImg.Crop(Constants.cropGeom);
            contentTopImg.RePage();
            //contentTopImg.TransparentChroma(MagickColors.Gray, MagickColors.White);

            contentTops[idx] = contentTopImg;
        }
        for (int idx = 0; idx < contentBottoms.Length; idx++)
        {
            var contentBottomFile = new FileInfo(Path.Join(Constants.baseDir, Constants.contentBottoms + "-" + (idx + 1).ToString() + Constants.imgExt));
            var contentBottomImg = new MagickImage(contentBottomFile);
            var offsetX = Constants.config.LayerOptions.TvBottomsOffsets[idx][0];
            var offsetY = Constants.config.LayerOptions.TvBottomsOffsets[idx][1];
            Console.WriteLine("offsets: " + idx + ":" + Int32.Parse(offsetX) + ", " + Int32.Parse(offsetY));
            MagickGeometry _cropGeom = new MagickGeometry(
                Constants.cropGeom.X - Int32.Parse(offsetX),
                Constants.cropGeom.Y - Int32.Parse(offsetY),
                Constants.cropGeom.Width,
                Constants.cropGeom.Height);

            contentBottomImg.Crop(_cropGeom);
            contentBottomImg.RePage();
            //contentBottomImg.TransparentChroma(MagickColors.Gray, MagickColors.White);

            contentBottoms[idx] = contentBottomImg;
        }
        // resizeGeometry = new MagickGeometry(4819, 6874);
        // resizeGeometry.IgnoreAspectRatio = true;
        // Console.WriteLine("Stacker:resizeGeom: " + resizeGeometry.Width + ", " + resizeGeometry.Height);
    }

    public static void initialize()
    {
        Console.WriteLine("Stacker.initialize()");
    }

    public static string process(int _topIdx, int _neckIdx, int _contentTopIdx, int _contentBottomIdx)
    {
        var fileName = "out-" + _contentTopIdx + "-" + _contentBottomIdx + "-" + _topIdx + "-" + _neckIdx + Constants.imgExt;
        using (var images = new MagickImageCollection())
        {
            if (Constants.config.LayerOptions.BgColor)
            {
                var rcolor = rand.Next(colors.Count);
                Console.WriteLine("  -- using color: " + colors[rcolor]);
                MagickColor color = new ImageMagick.MagickColor("#" + colors[rcolor]);
                var bg = new ImageMagick.MagickImage(color, 2260, 2260);
                images.Add(bg);
            }
            else
            {
                images.Add(blankImg.Clone()); // const
            }
            //var bgImg = BackgroundImageGen.GenerateImage(Constants.cropGeom.Width, Constants.cropGeom.Height);
            //images.Add(bgImg);

            images.Add(necks[_neckIdx - 1].Clone());
            if (Constants.config.LayerOptions.IncludeTvBg)
            {
                images.Add(headBgImg.Clone());
            }
            images.Add(contentTops[_contentTopIdx - 1].Clone());
            images.Add(contentBottoms[_contentBottomIdx - 1].Clone());
            images.Add(headImg.Clone()); // const
            images.Add(tops[_topIdx - 1].Clone());

            var outImg = images.Flatten();

            FileInfo outFile = new FileInfo(Path.Combine(Constants.outDir, fileName));
            outImg.Write(outFile); // 198s

            outImg.Dispose();

            foreach (var image in images)
            {
                image.Dispose();
            }
            images.Clear();
            images.Dispose();
        }
        return fileName;
    }
}
