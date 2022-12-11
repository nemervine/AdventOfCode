using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AoC_Day_10_OCR
{
    public struct CPU
    {
        public int cycle = 0;
        public int register = 1;
        public int[] cyclesCheck = new int[6] { 20, 60, 100, 140, 180, 220 };
        public int[] signalStrengths = new int[] { };
        public CPU()
        {
            cycle = 0;
            register = 1;
        }
        public void NextCycle()
        {
            cycle++;
            if (cyclesCheck.Contains(cycle))
            {
                signalStrengths = signalStrengths.Append(this.cycle * this.register).ToArray();
                Console.WriteLine(String.Format("Cycle: {0} Register: {1}", this.cycle, this.register));
            }
        }
    }
    public struct CRT
    {
        internal const int spriteWidth = 3;
        internal const int rowWidth = 40;
        internal const int rowCount = 6;
        public string[] rows = new string[rowCount];
        char[] blankRow = new char[rowWidth];
        public int position = 0;
        public CRT()
        {
            Array.Fill<char>(blankRow, ' ');
            Array.Fill<string>(rows, new string(blankRow));
        }
        public void Light(int reg)
        {
            if (((position % rowWidth) - 1 <= reg) && (reg <= (position % rowWidth) + 1))
            {
                char[] newRow = rows[position / rowWidth].ToCharArray();
                newRow[position % rowWidth] = '#';
                rows[position / rowWidth] = new string(newRow);
            }
        }
        public void NextPos()
        {
            this.position++;
            Console.WriteLine(this.position);
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public String[] ProcessInput()
        {
            string[] inputText = System.IO.File.ReadAllLines("input.txt");
            CPU cpu = new CPU();
            CRT crt = new CRT();
            for (int i = 0; i < inputText.Length; i++)
            {
                switch (inputText[i].Split(" ")[0])
                {
                    case "noop":
                        cpu.NextCycle();
                        crt.Light(cpu.register);
                        crt.NextPos();
                        break;
                    case "addx":
                        int val = Int32.Parse(inputText[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
                        cpu.NextCycle();
                        crt.Light(cpu.register);
                        crt.NextPos();
                        cpu.NextCycle();
                        crt.Light(cpu.register);
                        crt.NextPos();
                        cpu.register += val;
                        break;
                }
            }
            return crt.rows;
        }
        private async void CreateImage(WriteableBitmap bitmap, byte[] pixels)
        {
            using (Stream stream = bitmap.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(pixels, 0, pixels.Length);
            }
        }
        public async void PerformOCR(SoftwareBitmap bitmap)
        {

            OcrEngine ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            var ocrResults = await ocrEngine.RecognizeAsync(bitmap);
            Debug.WriteLine(ocrResults.Text);
            ocrText.Text= ocrResults.Text;
        }
        private async void SaveImage(byte[] pixel)
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Image", new List<string>() { ".png" });
            savePicker.SuggestedFileName = "text " + DateTime.Now.ToString("yyyyMMddhhmmss");
            StorageFile savefile = await savePicker.PickSaveFileAsync();
            if (savefile == null)
                return;

            using (IRandomAccessStream stream = await savefile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, 42, 8, 96, 96, pixel);
                await encoder.FlushAsync();
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
            string[] rows = ProcessInput();
            WriteableBitmap writeableBitmap = new WriteableBitmap(40+2, 6+2);
            byte[] pixels = new byte[] { };
            byte[] ColorData = new byte[] { 255, 255, 255, 255 };
            for (int j = 0; j < rows[0].Length + 2; j++)
            {
                pixels = pixels.Concat(ColorData).ToArray();
            }
            for (int i = 0; i < rows.Length; i++)
            {
                pixels = pixels.Concat(ColorData).ToArray();
                for (int j = 0; j < rows[i].Length; j++)
                {
                    
                    switch (rows[i].ToCharArray()[j])
                    {
                        default:
                            ColorData = new byte[] { 255, 255, 255, 255 };
                            pixels = pixels.Concat(ColorData).ToArray();
                            break;
                        case '#':
                            ColorData = new byte[] { 0, 0, 0, 255 };
                            pixels = pixels.Concat(ColorData).ToArray();
                            break;
                    }
                }
                ColorData = new byte[] { 255, 255, 255, 255 };
                pixels = pixels.Concat(ColorData).ToArray();
            }
            for (int j = 0; j < rows[0].Length + 2; j++)
            {
                pixels = pixels.Concat(ColorData).ToArray();
            }
            CreateImage(writeableBitmap, pixels);
            imageOutput.Source = writeableBitmap;
            SaveImage(pixels);
        }
    }
}
