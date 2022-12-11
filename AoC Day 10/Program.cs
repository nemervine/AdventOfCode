using System.Diagnostics;

namespace AoC_Day_10
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
                signalStrengths = signalStrengths.Append(this.cycle*this.register).ToArray();
                Debug.WriteLine(String.Format("Cycle: {0} Register: {1}", this.cycle, this.register));
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
            Debug.WriteLine(this.position);
        }
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        // [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            /*ApplicationConfiguration.Initialize();
            Application.Run(new Form1());*/
            string[] inputText = File.ReadAllLines("input.txt");
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
            Debug.WriteLine(cpu.signalStrengths.Sum());
            Debug.WriteLine("");
            foreach(string line in crt.rows)
            {
                Debug.WriteLine(line);
            }
            Debug.WriteLine("");
        }
    }
}