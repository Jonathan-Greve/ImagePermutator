using System;
using System.Drawing;
using System.IO;

namespace ImagePermutator
{
    class ImageSheet
    {
        private int numRows;
        private int numCols;

        public Image SourceImage { get; private set; }
        public Image OutputImage { get; private set; }
        public SheetSpecification Specification { get; private set; }
        public Rectangle CropArea { get; private set; }

        public void SetSpecification(SheetSpecification specification)
        {
            Specification = specification;
            numRows = Specification.SheetFormat.Height / Specification.ImageFormat.Height;
            numCols = Specification.SheetFormat.Width / Specification.ImageFormat.Width;
        }

        public void SetCropArea(int xStart, int yStart, int xEnd, int yEnd)
        {
            Point rectangleStartPoint = new Point(xStart, yStart);
            Size rectangleSize = new Size(xEnd- xStart, yEnd- yStart);
            CropArea = new Rectangle(rectangleStartPoint, rectangleSize);
        }

        public void Create()
        {
            int borderThicknessW = CropArea.Width / 30;
            double aspectRatio = (double)Specification.SheetFormat.Width / Specification.SheetFormat.Height;
            int OutputImageWidth = numCols * CropArea.Width + borderThicknessW * (numCols + 1);
            int OutputImageHeight = (int)(OutputImageWidth / aspectRatio);
            numCols = OutputImageWidth / CropArea.Width;
            int borderThicknessH = (OutputImageHeight - numRows * CropArea.Height) / (numRows + 1);
            OutputImage = new Bitmap(OutputImageWidth,OutputImageHeight);

            Graphics imageHelper = Graphics.FromImage(OutputImage);

            for (int col = 0; col < numCols; col++)
            {
                for (int row = 0; row  < numRows; row ++)
                {
                    Rectangle insertPosition = new Rectangle(borderThicknessW + col * (CropArea.Width + borderThicknessW), borderThicknessH + row * (CropArea.Height + borderThicknessH), CropArea.Width, CropArea.Height);
                    imageHelper.DrawImage(SourceImage, insertPosition, CropArea, GraphicsUnit.Pixel);
                }
            }

        }

        static public ImageSheet FromImage(Image sourceImage)
        {
            return new ImageSheet(sourceImage);
        }

        private ImageSheet(Image SourceImage)
        {
            this.SourceImage = SourceImage;
        }
    }

    public abstract class SheetFormat
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
    }

    public class A4 : SheetFormat
    {
        override public int Width { get; } = 210;
        override public int Height { get; } = 297;
    }
    public class CustomSheet : SheetFormat
    {
        static private int width;
        static private int height;
        override public int Width { get; } = width;
        override public int Height { get; } = height;

        public CustomSheet(int Width, int Height)
        {
            width = Width;
            height = Height;
        }
    }

    public abstract class ImageFormat
    {
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
    }
    public class PassportPhotoFormat : ImageFormat
    {
        override public int Width { get; set; } = 51;
        override public int Height { get; set; } = 51;
    }

    public class SheetSpecification
    {
        public SheetFormat SheetFormat { get; set; }
        public ImageFormat ImageFormat { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string imagePath = "C:\\Users\\Jonathan Greve\\Pictures\\TestImages\\DSC01325.jpg";
            Image inputImage = Image.FromFile(imagePath);

            SheetSpecification specification = new SheetSpecification
            {
                SheetFormat = new A4(),
                ImageFormat = new PassportPhotoFormat()
            };

            ImageSheet sheet = ImageSheet.FromImage(inputImage);
            sheet.SetSpecification(specification);
            sheet.SetCropArea(2100, 1400, 4100, 3400);
            sheet.Create();
            
            sheet.OutputImage.Save("C:\\Users\\Jonathan Greve\\Pictures\\TestImages\\DSC01325Outitttt.jpg");
        }
    }
}
