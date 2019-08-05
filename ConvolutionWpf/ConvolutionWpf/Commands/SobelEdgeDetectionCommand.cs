using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class SobelEdgeDetectionCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public SobelEdgeDetectionCommand(Func<WriteableBitmap> imageFactory)
            : base(() => { })
        {
            _imageFactory = imageFactory;
        }

        private void ExecuteCommand()
        {
            var image = _imageFactory();
            if (image == null)
                return;

            int intencity = 3;
            int kernelSize = ToOddNumber(intencity);

            byte[] resultPixels = DetectEdges(image, kernelSize);

            image.WritePixels(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight), resultPixels, image.BackBufferStride, 0);
        }

        //Проверка и преобразование чётной интенсивности в нечётную
        private int ToOddNumber(int number)
        {
            return number % 2 == 0 ? number + 1 : number;
        }

        //Создание горизонтального ядра
        private double[,] KernelHorizontalCalc(int kernelSize)
        {
            double[,] kernelNumbers = {
                {-1.0, 0, 1.0},
                {-2.0, 0, 2.0},
                {-1.0, 0, 1.0}};

            double[,] kernel = new double[kernelSize, kernelSize];
            for (int i = 0; i < kernelSize; i++)
            {
                for (int j = 0; j < kernelSize; j++)
                {
                    kernel[i, j] = kernelNumbers[i, j] / (kernelSize * kernelSize);
                }
            }
            return kernel;
        }

        //Создание вертикального ядра
        private double[,] KernelVerticalCalc(int kernelSize)
        {
            double[,] kernelNumbers = {
                {-1.0, -2.0, -1.0},
                {0, 0, 0},
                {1.0, 2.0, 1.0}};

            double[,] kernel = new double[kernelSize, kernelSize];
            for (int i = 0; i < kernelSize; i++)
            {
                for (int j = 0; j < kernelSize; j++)
                {
                    kernel[i, j] = kernelNumbers[i, j] / (kernelSize * kernelSize);
                }
            }
            return kernel;
        }

        //Рассчёт смещения
        private int DeviationCalc(int kernelSize)
        {
            return (int)((kernelSize - 1) / 2);
        }

        private byte[] DetectEdges(WriteableBitmap image, int kernelSize)
        {
            var pixels = new byte[image.PixelHeight * image.BackBufferStride];
            image.CopyPixels(pixels, image.BackBufferStride, 0);
            double[,] kernelHorizontial = KernelHorizontalCalc(kernelSize);
            double[,] kernelVertical = KernelVerticalCalc(kernelSize);
            int deviation = DeviationCalc(kernelSize);
            var resultPixels = new byte[image.PixelHeight * image.BackBufferStride];

            for (int i = deviation; i < image.PixelWidth - deviation; i++)
            {
                for (int j = deviation; j < image.PixelHeight - deviation; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        double resultHorizontal = 0;
                        double resultVertical = 0;

                        //Проход горизонтальным ядром
                        for (int k = i - deviation, w = 0; k <= i + deviation; k++, w++)
                        {
                            for (int l = j - deviation, h = 0; l <= j + deviation; l++, h++)
                            {
                                int indexGrid = l * image.BackBufferStride + 4 * k;
                                resultHorizontal += pixels[indexGrid + c] * kernelHorizontial[w, h];
                            }
                        }

                        //Проход вертикальным ядром
                        for (int k = i - deviation, w = 0; k <= i + deviation; k++, w++)
                        {
                            for (int l = j - deviation, h = 0; l <= j + deviation; l++, h++)
                            {
                                int indexGrid = l * image.BackBufferStride + 4 * k;
                                resultVertical += pixels[indexGrid + c] * kernelVertical[w, h];
                            }
                        }

                        resultPixels[index + c] = (byte)Math.Sqrt(Math.Pow(resultHorizontal, 2) + Math.Pow(resultVertical, 2));
                    }

                    resultPixels[index + 3] = pixels[index + 3];

                }
            }

            return resultPixels;
        }

        protected override void Execute(object parameter, bool ignoreCanExecuteCheck)
        {
            ExecuteCommand();
        }
    }
}