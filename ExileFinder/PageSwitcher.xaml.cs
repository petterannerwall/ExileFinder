using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExileFinder.Parts;

namespace ExileFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PageSwitcher : Window, ISwitchable
    {

        private WinEventDelegate dele;
        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        public PageSwitcher()
        {
            InitializeComponent();
            Switcher.PageSwitcher = this;
            Switcher.Switch(new Start());

            dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }

        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
                s.UtilizeState(state);
            else
                throw new ArgumentException("NextPage is not ISwitchable! "
                  + nextPage.Name.ToString());
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        #region GetActiveWindow code

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            string activeWindow = GetActiveWindowTitle();

            if (activeWindow != null)
            {
                if (!activeWindow.Contains("Notepad") && !activeWindow.Contains("Exile") && !activeWindow.Contains("menu"))
                {
                    this.WindowState = WindowState.Minimized;
                    this.Topmost = false;
                }
                if (activeWindow.Contains("Notepad") || activeWindow.Contains("Exile"))
                {
                    this.WindowState = WindowState.Normal;
                    this.Topmost = true;
                }
            }
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        
        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        #endregion



    }
}
