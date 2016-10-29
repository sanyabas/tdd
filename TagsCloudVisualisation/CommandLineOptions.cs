using CommandLine;

namespace TagsCloudVisualisation
{
    public class CommandLineOptions
    {
        [Option('n', "number", DefaultValue = 100)]
        public int RectanglesNumber { get; set; }

        [Option('h', "height", DefaultValue = 30)]
        public int Height { get; set; }

        [Option('w', "width", DefaultValue = 30)]
        public int Width { get; set; }

        [Option('o', "output", DefaultValue = @"main.bmp")]
        public string OutputFileName { get; set; }

        [Option('x',"x position of center", DefaultValue = 400)]
        public int CenterX { get; set; }

        [Option('y', "y position of center", DefaultValue = 300)]
        public int CenterY { get; set; }
    }
}