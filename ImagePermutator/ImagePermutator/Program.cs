﻿using System;
using System.Drawing;
using System.IO;

namespace ImagePermutator
{
    class ImageSheet
    {
        public Image SourceImage { get; private set; }
        public SheetSpecification Specification { get; private set; }

        public Image OutputImage { get; private set; }

        public ImageSheet(Image SourceImage, SheetSpecification Specification)
        {
            this.SourceImage = SourceImage;
            this.Specification = Specification;
        }

        public void Create()
        {

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

            ImageSheet sheet = new ImageSheet(inputImage, specification);
            sheet.Create();
            
            Console.WriteLine("SheetFormat.Width: {0}", specification.SheetFormat.Width);

            int numberOfPhotos = 2;
            int width_mm = 102;
            int height_mm = 51;
            float aspectRatio = width_mm / (float)height_mm;


            Bitmap inputImageBMP = new Bitmap(inputImage);
            Bitmap outputImageBMP = new Bitmap((int)(aspectRatio * inputImage.Width * numberOfPhotos), (inputImage.Width));


            Size size = new Size((int)(aspectRatio * inputImage.Width), inputImage.Height);

            Point location = new Point(1000, 1400);
            Rectangle selectionArea = new Rectangle(location, size);



            Point destination = new Point(0, 0);
            Rectangle inputArea = new Rectangle(destination, size);

            Graphics imageHelper = Graphics.FromImage(outputImageBMP);
            imageHelper.DrawImage(inputImageBMP, inputArea, selectionArea, GraphicsUnit.Pixel);
            //imageHelper.DrawImage(inputImageBMP, inputArea, selectionArea, GraphicsUnit.Pixel);
            outputImageBMP.Save("C:\\Users\\Jonathan Greve\\Pictures\\TestImages\\DSC01325Outtt.jpg");
        }
    }
}
