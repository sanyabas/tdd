using System.Drawing;
using System.Drawing.Imaging;
using CommandLine;
using TagsCloudVisualisation.Layouter;
using TagsCloudVisualisation.Visualizer;

namespace TagsCloudVisualisation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!Parser.Default.ParseArguments(args, options))
                return;
            var center = new PointF(options.CenterX, options.CenterY);
            var layouter = new CircularCloudLayouter(center);
            var visualizer = new CircularCloudVisualizer(layouter);
            var size = new SizeF(options.Width, options.Height);
            for (var i = 0; i < options.RectanglesNumber; i++)
                layouter.PutNextRectangle(size);
            visualizer.VisualizeAndSave(layouter.GetLayout(), options.OutputFileName, ImageFormat.Bmp);
        }
    }
}
