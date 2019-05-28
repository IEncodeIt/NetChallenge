using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class BlurCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public BlurCommand(Func<WriteableBitmap> imageFactory)
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

            int kernelSize = 5;
            int depth = 1;
            int deviation = (int)((kernelSize - 1) / 2);


            //Grid
            int normIndex = 0;

            int[,] grid = new int[kernelSize, kernelSize];

            for (int w = 0; w < kernelSize; w++)
            {
                for (int h = 0; h < kernelSize; h++)
                {
                    grid[w, h] = depth;
                    normIndex += depth;
                }
            }

            //Black border disable
            //Horizontal
            for (int i = 0; i < image.PixelWidth; i++)
            {
                for (int j = 0, l = image.PixelHeight - deviation; j < deviation; j++, l++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        int indexBorder = j * image.BackBufferStride + 4 * i;
                        int indexBorder2 = l * image.BackBufferStride + 4 * i;

                        resultPixels[indexBorder + c] = pixels[indexBorder + c];
                        resultPixels[indexBorder2 + c] = pixels[indexBorder2 + c];
                    }
                }
            }

            //Vertical
            for (int j = 0; j < image.PixelHeight; j++)
            {
                for (int i = 0, k = image.PixelWidth - deviation; i < deviation; i++, k++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        int indexBorder3 = j * image.BackBufferStride + 4 * i;
                        int indexBorder4 = j * image.BackBufferStride + 4 * k;

                        resultPixels[indexBorder3 + c] = pixels[indexBorder3 + c];
                        resultPixels[indexBorder4 + c] = pixels[indexBorder4 + c];
                    }
                }
            }

            //Work
            for (int i = deviation; i < image.PixelWidth - deviation; i++)
            {
                for (int j = deviation; j < image.PixelHeight - deviation; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        int result = 0;

                        for (int k = i - deviation, w = 0; k <= i + deviation; k++, w++)
                        {
                            for (int l = j - deviation, h = 0; l <= j + deviation; l++, h++)
                            {
                                int indexGrid = l * image.BackBufferStride + 4 * k;

                                result += pixels[indexGrid + c] * grid[w, h];
                            }
                        }

                        result /= normIndex;

                        resultPixels[index + c] = (byte)(result);
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