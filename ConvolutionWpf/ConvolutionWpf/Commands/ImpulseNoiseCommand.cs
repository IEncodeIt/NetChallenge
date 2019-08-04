using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class ImpulseNoiseCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public ImpulseNoiseCommand(Func<WriteableBitmap> imageFactory)
            : base(() => { })
        {
            _imageFactory = imageFactory;
        }

        private void ExecuteCommand()
        {
            var image = _imageFactory();
            if (image == null)
                return;

            byte[] resultPixels = RemoveNoise(image, sensetivity: 3);

            image.WritePixels(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight), resultPixels, image.BackBufferStride, 0);

        }

        //Рассчёт смещения
        private int DeviationCalc(int kernelSize)
        {
            return (int)((kernelSize - 1) / 2);
        }

        private byte[] RemoveNoise(WriteableBitmap image, int sensetivity)
        {
            var pixels = new byte[image.PixelHeight * image.BackBufferStride];
            image.CopyPixels(pixels, image.BackBufferStride, 0);

            int deviation = DeviationCalc(sensetivity);
            var resultPixels = new byte[image.PixelHeight * image.BackBufferStride];

            List<byte> pixelList = new List<byte>();

            for (int i = deviation; i < image.PixelWidth - deviation; i++)
            {
                for (int j = deviation; j < image.PixelHeight - deviation; j++)
                {
                    int index = j * image.BackBufferStride + 4 * i;

                    for (int c = 0; c < 3; c++)
                    {
                        for (int k = i - deviation, w = 0; k <= i + deviation; k++, w++)
                        {
                            for (int l = j - deviation, h = 0; l <= j + deviation; l++, h++)
                            {
                                int indexGrid = l * image.BackBufferStride + 4 * k;
                                pixelList.Add(pixels[indexGrid + c]);
                            }
                        }
                        pixelList.Sort();
                        byte result = pixelList.ElementAt(4);
                        resultPixels[index + c] = (result);
                        pixelList.Clear();
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