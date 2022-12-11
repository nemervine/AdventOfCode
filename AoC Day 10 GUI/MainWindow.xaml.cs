using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Windows.Media.Ocr;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace AoC_Day_10_GUI
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        /*private async void PerformOCR()
        {
            
            OcrEngine ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            //OcrResult ocrResult = ocrEngine.RecognizeAsync();
        }*/
        public MainWindow()
        {
            InitializeComponent();
            string[] rows = ProcessInput();
            int padding = 2;
            WriteableBitmap writeableBitmap = new WriteableBitmap(CRT.rowWidth+padding,CRT.rowCount+padding);
            byte[] pixels = new byte[] { };
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j=0; j < rows[i].Length; j++)
                {
                    Int32Rect rect = new Int32Rect(j+ padding/2, i + padding / 2, 1, 1);
                    byte[] ColorData;
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
            }
            CreateImage(writeableBitmap, pixels);
            Output.Source = writeableBitmap;
        }
    }
}
