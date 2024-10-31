using ImageMagick;
using Microsoft.Extensions.Configuration;

public static class Constants
{
    public static string baseDir = "/home/jrod/Software/nftlayers/images";
    public static string outDir = "/home/jrod/Software/nftlayers/imgout";
    public static string blank = "blank";
    public static string heads = "head";
    public static string necks = "neck";
    public static string tops = "top";
    public static string contentTops = "tvtop";
    public static string contentBottoms = "tvbottom";
    public static string imgExt = ".png";
    public static MagickColor transparentColor = transparentColor = new MagickColor(215, 215, 215);
    public static FileInfo blankFile = new FileInfo(Path.Join(baseDir, blank + imgExt));
    public static MagickGeometry cropGeom = new MagickGeometry(1310, 2080, 2260, 2260);
    public static Dictionary<string, int> variants = new Dictionary<string, int>()
    {
        {Constants.necks, 7},
        {Constants.tops, 5},
        {Constants.contentTops, 6},
        {Constants.contentBottoms, 16}
    };

    public static IConfigurationRoot configuration;
    private static IConfigurationRoot colorConfiguration;

    public static Config config
    {
        get
        {
            return configuration.Get<Config>();
        }
    }

    static Constants()
    {
    }

    public static void Initialize()
    {
        Console.WriteLine("Loading configurations.");
        configuration = new ConfigurationBuilder()
            .AddJsonFile("src/config.json", optional: false)
            .Build();
        Console.WriteLine("Configuration loaded: [" + config.RunOptions.Debug + "]");
        colorConfiguration = new ConfigurationBuilder()
            .AddJsonFile("src/palettes/palettes.json", optional: false)
            .Build();
        Console.WriteLine("ColorConfiguration loaded: [" + GetPalette("unknown")[0] + "]");
    }

    public static List<string> GetPalette(string name)
    {
        var dict = colorConfiguration.GetSection("palettes:" + name).Get<string[]>();
        return dict.ToList();
    }
}