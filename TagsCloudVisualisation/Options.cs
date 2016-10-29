using CommandLine;

namespace TagsCloudVisualisation
{
    public class Options
    {
        [Option('n', "number", DefaultValue = 100)]
        public int RectanglesNumber { get; set; }
        [Option('h', "height", DefaultValue = 30)]
        public int Height { get; set; }
        [Option('w', "width", DefaultValue = 30)]
        public int Width { get; set; }
        [Option('o', "output", DefaultValue = @"main.bmp")]
        public string OutputFileName { get; set; }
    }
}