
using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Inventory_Check
{
    class LPrinter
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        /*=================================================*/

        private IntPtr HandlePrinter;
        public PrinterSettings ps;

        public LPrinter()
        {
            HandlePrinter = IntPtr.Zero;
            ps = new PrinterSettings();
        }
        public PrintDialog pd = new PrintDialog();
        public string PrinterName
        {
            get
            {
                return ps.PrinterName;
            }
            set
            {
                ps.PrinterName = value;
            }
        }

        public bool Open(string DocName)
        {
            // see if printer is already open
            if (HandlePrinter != IntPtr.Zero) return false;

            // opens the printer
            bool risp = OpenPrinter(ps.PrinterName, out HandlePrinter, IntPtr.Zero);
            if (risp == false) return false;

            // starts a print job
            DOCINFOA MyDocInfo = new DOCINFOA();
            MyDocInfo.pDocName = DocName;
            MyDocInfo.pOutputFile = null;
            MyDocInfo.pDataType = "RAW";

            if (StartDocPrinter(HandlePrinter, 1, MyDocInfo))
            {
                StartPagePrinter(HandlePrinter); //starts a page       
                return true;
            }
            else return false;
        }

        public bool Close()
        {
            if (HandlePrinter == IntPtr.Zero) return false;
            if (!EndPagePrinter(HandlePrinter)) return false;
            if (!EndDocPrinter(HandlePrinter)) return false;
            if (!ClosePrinter(HandlePrinter)) return false;
            HandlePrinter = IntPtr.Zero;
            return true;
        }

        public bool Print(string outputstring)
        {
            if (HandlePrinter == IntPtr.Zero) return false;
             
            IntPtr buf = Marshal.StringToCoTaskMemAnsi(outputstring);

            Int32 done = 0;
            bool ok = WritePrinter(HandlePrinter, buf, outputstring.Length, out done);

            Marshal.FreeCoTaskMem(buf);

            if (!ok) return false;
            else return true;
        }
    }
}
