using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Overlay.NET.Common;
using Process.NET;
using Process.NET.Memory;
using System.Runtime.InteropServices;

namespace ExileFinder
{


    class Program
    {
        private static Overlay _overlay;
        private static ProcessSharp _processSharp;
        private static bool _work;


        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);


            var process = System.Diagnostics.Process.GetProcessesByName("Notepad").FirstOrDefault();
            if (process == null)
            {
                Log.Warn($"No process by the name of Notepad was found.");
                Log.Warn("Please open one or use a different name and restart the demo.");
                Console.ReadLine();
                return;
            }

            _processSharp = new ProcessSharp(process, MemoryType.Remote);
            _overlay = new Overlay();

            var wpfOverlay = (Overlay)_overlay;

            // This is done to focus on the fact the Init method
            // is overriden in the wpf overlay demo in order to set the
            // wpf overlay window instance
            wpfOverlay.Initialize(_processSharp.WindowFactory.MainWindow);
            wpfOverlay.Enable();


            _work = true;

            // Log some info about the overlay.
            Log.Debug("Starting update loop (open the process you specified and drag around)");
            Log.Debug("Update rate: " + wpfOverlay.Settings.Current.UpdateRate.Milliseconds());

            var info = wpfOverlay.Settings.Current;

            Log.Debug($"Author: {info.Author}");
            Log.Debug($"Description: {info.Description}");
            Log.Debug($"Name: {info.Name}");
            Log.Debug($"Identifier: {info.Identifier}");
            Log.Debug($"Version: {info.Version}");

            Log.Info("Note: Settings are saved to a settings folder in your main app folder.");

            Log.Info("Give your window focus to enable the overlay (and unfocus to disable..)");

            Log.Info("Close the console to end the demo.");

            wpfOverlay.OverlayWindow.Draw += OnDraw;


            // Do work
            while (_work)
            {
                _overlay.Update();
            }

            Log.Debug("Demo complete.");
        }

        static void OnDraw(object sender, DrawingContext context)
        {
            var demoForm = new DemoForm();
            var brush = new VisualBrush(demoForm);
            context.DrawRectangle(brush, null, new Rect(0, 0, 100, 25));
        }
    }
    
}
