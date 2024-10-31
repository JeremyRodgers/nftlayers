public class Config
{
    public RunOptionsConfig RunOptions { get; set; }
    public LayerOptionsConfig LayerOptions { get; set; }

    public class RunOptionsConfig
    {
        public bool Debug { get; set; }
    }

    public class LayerOptionsConfig
    {
        public bool IncludeTvBg { get; set; }
        public bool BgColor { get; set; }
        public string BgColorPalette { get; set; }
        public string BgImagePath { get; set; }
        public List<List<string>> TvBottomsOffsets { get; set; }
    }
}