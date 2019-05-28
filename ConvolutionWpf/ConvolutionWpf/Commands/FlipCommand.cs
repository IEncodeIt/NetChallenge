using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class FlipCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public FlipCommand(Func<WriteableBitmap> imageFactory)
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

            for (int i = 0, k = image.PixelWidth-1; i < image.PixelWidth; i++, k--)
            {
                for (int j = 0; j < image.PixelHeight; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;
                    int indexRes = j * image.BackBufferStride + 4 * k;

                    for (int c = 0; c < 3; c++)
                    {
                       resultPixels[indexRes + c] = pixels[index + c];
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