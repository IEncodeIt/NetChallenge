using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class EdgeDetectionCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public EdgeDetectionCommand(Func<WriteableBitmap> imageFactory)
            : base(() => { })
        {
            _imageFactory = imageFactory;
        }

        public void ExecuteCommand()
        {
            var image = _imageFactory();
            if (image == null)
                return;

            int intencity = 3;
            int kernelSize = ToOddNumber(intencity);
            var resultPixels = DetectEdges(image, kernelSize);

            image.WritePixels(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight), resultPixels, image.BackBufferStride, 0);
        }

        //Проверка и преобразование чётной интенсивности в нечётную
        private int ToOddNumber(int number)
        {
            return number % 2 == 0 ? number + 1 : number;
        }

        //Создание ядра свёртки
        private double[,] CreateEdgeKernel(int kernelSize)
        {
            double[,] kernel = new double[kernelSize, kernelSize];
            for (int w = 0; w < kernelSize; w++)
            {
                for (int h = 0; h < kernelSize; h++)
                {
                    kernel[w, h] = -1.0 / (kernelSize* kernelSize);
                }
            }
            //Определяем центральную точку
            int centralPoint = (int)(kernelSize / 2);
            kernel[centralPoint, centralPoint] = 8.0 / (kernelSize * kernelSize);
            return kernel;
        }
        
        //Рассчёт смещения
        private int DeviationCalc(int kernelSize)
        {
            return (int)((kernelSize - 1) / 2);
        }

        //Обработка изображения
        private byte[] DetectEdges(WriteableBitmap image, int kernelSize)
        {
            var pixels = new byte[image.PixelHeight * image.BackBufferStride];
            image.CopyPixels(pixels, image.BackBufferStride, 0);

            double[,] kernel = CreateEdgeKernel(kernelSize);

            int deviation = DeviationCalc(kernelSize);

            var resultPixels = new byte[image.PixelHeight * image.BackBufferStride];

            for (int i = deviation; i < image.PixelWidth - deviation; i++)
            {
                for (int j = deviation; j < image.PixelHeight - deviation; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        double result = 0;

                        for (int k = i - deviation, w = 0; k <= i + deviation; k++, w++)
                        {
                            for (int l = j - deviation, h = 0; l <= j + deviation; l++, h++)
                            {
                                int indexGrid = l * image.BackBufferStride + 4 * k;

                                result += pixels[indexGrid + c] * kernel[w, h];
                            }
                        }

                        resultPixels[index + c] = (byte)(result);
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