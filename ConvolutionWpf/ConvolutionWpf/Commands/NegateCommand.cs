using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class NegateCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public NegateCommand(Func<WriteableBitmap> imageFactory)
            : base(() => { })
        {
            _imageFactory = imageFactory;
        }

        public void ExecuteCommand()
        {
            var image = _imageFactory();
            if (image == null)
                return;

            var pixels = new byte[image.PixelHeight * image.BackBufferStride];
            image.CopyPixels(pixels, image.BackBufferStride, 0);

            var resultPixels = new byte[image.PixelHeight * image.BackBufferStride];

            for (int i = 0; i < image.PixelWidth; i++)
            {
                for (int j = 0; j < image.PixelHeight; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    //Первый вариант
                    /*int red = pixels[index];
                    int green = pixels[index + 1];
                    int blue = pixels[index + 2];

                    int redNew = 255 - red;
                    int greenNew = 255 - green;
                    int blueNew = 255 - blue;

                    resultPixels[index] = (byte)redNew;
                    resultPixels[index + 1] = (byte)greenNew;
                    resultPixels[index + 2] = (byte)blueNew;
                    resultPixels[index + 3] = pixels[index + 3];*/


                    //Вариант компактнее
                    for (int c = 0; c < 3; c++)
                    {
                        resultPixels[index + c] = (byte)(255 - pixels[index + c]);
                    }
                    resultPixels[index + 3] = pixels[index + 3];
                }
                
            }

            image.WritePixels(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight), resultPixels, image.BackBufferStride, 0);
        }

        protected override void Execute(object parameter, bool ignoreCanExecuteCheck)
        {
            ExecuteCommand();
        }
    }
}