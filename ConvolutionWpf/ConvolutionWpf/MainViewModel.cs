using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.Data;
using Catel.MVVM;
using ConvolutionWpf.Commands;
using Microsoft.Win32;

namespace ConvolutionWpf
{
    public class MainViewModel: ViewModelBase
    {
        public MainViewModel()
        {
            LoadCmd = new Command(LoadCommand);
            SaveCmd = new Command(SaveCommand);
            ResetCmd = new Command(ResetCommand);

            GrayScaleCmd = new GrayScaleCommand(() => Image);
            BlurCmd = new BlurCommand(() => Image);
            LogCmd = new LogCommand(() => Image);
            ContrastCmd = new ContrastCommand(() => Image);
            FlipCmd = new FlipCommand(() => Image);
            NegateCmd = new NegateCommand(() => Image);

            EdgesCmd = new Command(() => { });
        }

        private WriteableBitmap _originalImage;

        public Command GrayScaleCmd { get; }

        public Command BlurCmd { get; }

        public Command LogCmd { get; }

        public Command ContrastCmd { get; }

        public Command FlipCmd { get; }

        public Command EdgesCmd { get; }

        public Command NegateCmd { get; }

        public Command LoadCmd { get; }

        public Command SaveCmd { get; }

        public Command ResetCmd { get; }

        public static PropertyData ImagePathProperty = RegisterProperty("Image", typeof(WriteableBitmap));

        public WriteableBitmap Image
        {
            get => GetValue<WriteableBitmap>(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        private void LoadCommand()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Images|*.png;*.jpg;*jpeg;*.bmp";
            if (dlg.ShowDialog() == true)
            {
                Image = new WriteableBitmap(new BitmapImage(new Uri(dlg.FileName)));
            }

            _originalImage = Image.Clone();
        }

        private void ResetCommand()
        {
            Image.WritePixels(new Int32Rect(0, 0, _originalImage.PixelWidth, _originalImage.PixelHeight), 
                _originalImage.BackBuffer, _originalImage.BackBufferStride * _originalImage.PixelHeight,_originalImage.BackBufferStride, 0, 0);
        }

        private void SaveCommand()
        {
            var dlg = new SaveFileDialog() { DefaultExt = ".png" };
            dlg.Filter = "Png|*.png";
            if (dlg.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Image));

                using (var fileStream = new FileStream(dlg.FileName, FileMode.Create))
                    encoder.Save(fileStream);
            }
        }
    }
}