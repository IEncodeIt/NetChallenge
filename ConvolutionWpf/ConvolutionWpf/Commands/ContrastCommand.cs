using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class ContrastCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public ContrastCommand(Func<WriteableBitmap> imageFactory)
            : base(() => { })
        {
            _imageFactory = imageFactory;
        }

        public void ExecuteCommand()
        {
            var image = _imageFactory();
            if (image == null)
                return;

            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int pixelsCount = width * height * 3;

            int min = 255;
            int max = 0;
            float p = 0.005f;

            var pixels = new byte[height * image.BackBufferStride];
            image.CopyPixels(pixels, image.BackBufferStride, 0);

            var resultPixels = new byte[height * image.BackBufferStride];


            //Histogram
            int[] hist = new int[256];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        int a = pixels[index + c];
                        hist[a] += 1;
                    }
                }
            }


            //Cumulative histogram
            int[] cumHist = new int[256];

            for (int i = 1; i < 256; i++)
            {
                cumHist[i] = cumHist[i - 1] + hist[i];
            }


            //Max & Min(Simple)
            /*for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        int a = pixels[index + c];
                        if (a > max)
                            max = a;
                        if (a < min)
                            min = a;
                    }
                }
            }*/

            //Max & Min(Modify)
            int aLow = 0;
            for(int i=0; i < 256; i++)
                if (cumHist[i] >= pixelsCount * p)
                {
                    aLow = i;
                    break;
                }

            int aHigh = 255;
            for (int i = 255; i >= 0; i--)
                if (cumHist[i] <= pixelsCount * (1-p))
                {
                    aHigh = i;
                    break;
                }


            //Autocontrast
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        float b = 0f;
                        int a = pixels[index + c];

                        if (a <= aLow)
                            b = 0;

                        else if (a >= aHigh)
                            b = 255;

                        else
                            b = (float)(a - aLow) / (aHigh - aLow)*255;

                        resultPixels[index + c] = (byte)b;
                    }
                    resultPixels[index + 3] = pixels[index + 3];
                }
            }

            image.WritePixels(new Int32Rect(0, 0, width, height), resultPixels, image.BackBufferStride, 0);
        }

        protected override void Execute(object parameter, bool ignoreCanExecuteCheck)
        {
            ExecuteCommand();
        }
    }
}