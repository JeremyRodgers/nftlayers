/**
Stack images using ImageMagick
*/
using ImageMagick;

Constants.Initialize();
MagickNET.Initialize();
MagickNET.SetFontConfigDirectory("/etc/fonts");
// Starting indicies for the layers
var contentTopIdx = 1;
var contentBottomIdx = 1;
var topIdx = 1;
var neckIdx = 1;

List<StackedImage> images = new List<StackedImage>();
var isDone = false;
// var totalItems = 0;

var maxItems = Constants.config.RunOptions.Debug ? 100 : Int32.MaxValue;

// var border = TextBorder.GenerateBorder("The Warnings Went Unheeded");
// border.Write(new FileInfo(Path.Join(Constants.baseDir, "border.png")));
// Console.WriteLine("FROM CONFIG: [" + Constants.configuration["layerOptions:bgColorPalette"] +"]" );
// Console.WriteLine("Palette: " + Constants.GetPalette(Constants.config.LayerOptions.BgColorPalette)[0]);
// throw new Exception("End of Program");

// build up the list of image configurations
do
{
    StackedImage image = new StackedImage(1, 1, 1, 1);
    if (contentTopIdx + 1 > Constants.variants[Constants.contentTops])
    {
        contentTopIdx = 1;
        if (contentBottomIdx + 1 > Constants.variants[Constants.contentBottoms])
        {
            contentBottomIdx = 1;
            if (topIdx + 1 > Constants.variants[Constants.tops])
            {
                topIdx = 1;
                if (neckIdx + 1 > Constants.variants[Constants.necks])
                {
                    isDone = true;
                }
                else { neckIdx++; }
            }
            else { topIdx++; }
        }
        else { contentBottomIdx++; }
    }
    else { contentTopIdx++; }
    image._contentTopIdx = contentTopIdx;
    image._contentBottomIdx = contentBottomIdx;
    image._topIdx = topIdx;
    image._neckIdx = neckIdx;
    images.Add(image);
    //if (++totalItems == maxItems) { isDone = true; }
    
} while (!isDone);

Stacker.initialize();

var totalCount = 1;
var stopwatch = new System.Diagnostics.Stopwatch();
stopwatch.Start();
double avgTimePerImage = 0;
var totalImages = images.Count();
Console.WriteLine("Processing total of: " + totalImages + " images. Time: " + stopwatch.Elapsed.TotalSeconds);
// Impl A: Parallel.ForEach
CancellationTokenSource cts = new CancellationTokenSource();
try
{
    Parallel.ForEach(images, new ParallelOptions
    {
        CancellationToken = cts.Token,
        // without limiting max degree getting OOM errors.
        // multiply the count because a processor has 2 cores
        MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0))
    }, image =>
    {
        var fileName = Stacker.process(image._topIdx, image._neckIdx, image._contentTopIdx, image._contentBottomIdx);
        if (fileName != null)
        {
            Console.WriteLine("--- Writing file: " + fileName + ", total: " + totalCount++ + ", time: " + stopwatch.Elapsed.TotalSeconds);
        }
        else
        {
            Console.WriteLine("### Error creating file: " + image.ToString() + ", at: " + totalCount);
        }
        if (totalCount == 100)
        {
            avgTimePerImage = stopwatch.Elapsed.TotalSeconds / 100;
        }
        if (totalCount >= 100 && totalCount % 10 == 0)
        {
            var secondsRemaining = avgTimePerImage * (totalImages - totalCount - 100);
            TimeSpan remaining = TimeSpan.FromSeconds(secondsRemaining);
            Console.WriteLine(">>>>>> Remaining seconds: " + remaining.Hours + ":" + remaining.Minutes);
        }
        if (totalCount >= maxItems)
        {
            cts.Cancel();
        }
    });
}
catch (OperationCanceledException e)
{
    Console.WriteLine(e.Message);
}
finally
{
    cts.Dispose();
}
// Impl B: PLINQ
// images
//     .AsParallel()
//     .WithDegreeOfParallelism(Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0)))
//     .ForAll(image =>
//     {
//         var fileName = Stacker.process(image._topIdx, image._neckIdx, image._contentTopIdx, image._contentBottomIdx);
//         if (fileName != null)
//         {
//             Console.WriteLine("--- Writing file: " + fileName + ", total: " + totalCount++);
//         }
//         else
//         {
//             Console.WriteLine("### Error creating file: " + image.ToString() + ", at: " + totalCount);
//         }
//     });
Console.WriteLine("Elapsed time: " + stopwatch.Elapsed.TotalSeconds);
stopwatch.Stop();
