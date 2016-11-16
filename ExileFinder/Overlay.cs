using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ExileFinder.internals;
using Overlay.NET.Common;
using Overlay.NET.Wpf;
using Process.NET.Native.Types;
using Process.NET.Windows;

namespace ExileFinder
{
    class Overlay : WpfOverlayPlugin
    {
        // Used to limit update rates via timestamps 
        // This way we can avoid thread issues with wanting to delay updates
        readonly TickEngine _tickEngine = new TickEngine();

        bool _isDisposed;
        bool _isSetup;
        public ISettings<OverlaySettings> Settings { get; } = new SerializableSettings<OverlaySettings>();

        public override void Enable()
        {
            _tickEngine.IsTicking = true;
            base.Enable();
        }

        public override void Disable()
        {
            _tickEngine.IsTicking = false;
            base.Disable();
        }

        public override void Initialize(IWindow targetWindow)
        {
            // Set target window by calling the base method
            base.Initialize(targetWindow);

            OverlayWindow = new OverlayWindow(targetWindow);

            // For demo, show how to use settings
            var current = Settings.Current;
            var type = GetType();

            current.UpdateRate = 1000 / 60;
            current.Author = GetAuthor(type);
            current.Description = GetDescription(type);
            current.Identifier = GetIdentifier(type);
            current.Name = GetName(type);
            current.Version = GetVersion(type);

            // File is made from above info
            Settings.Save();
            Settings.Load();

            // Set up update interval and register events for the tick engine.
            _tickEngine.Interval = Settings.Current.UpdateRate.Milliseconds();
            _tickEngine.PreTick += OnPreTick;
            _tickEngine.Tick += OnTick;
        }

        void OnTick(object sender, EventArgs eventArgs)
        {
            // This will only be true if the target window is active
            // (or very recently has been, depends on your update rate)
            if (OverlayWindow.IsVisible)
            {
                OverlayWindow.Update();
            }
        }

        void OnPreTick(object sender, EventArgs eventArgs)
        {
            // Only want to set them up once.
            if (!_isSetup)
            {
                SetUp();
                _isSetup = true;
            }

            var activated = TargetWindow.IsActivated;
            var visible = OverlayWindow.IsVisible;

            // Ensure window is shown or hidden correctly prior to updating
            if (!activated && visible)
            {
                OverlayWindow.Hide();
            }

            else if (activated && !visible)
            {
                OverlayWindow.Show();
            }
        }

        public override void Update()
        {
            // Raises the events only when the given interval has
            // passed since the last event, so it is okay to call every frame
            _tickEngine.Pulse();
        }

        // Clear objects
        public override void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (IsEnabled)
            {
                Disable();
            }

            OverlayWindow?.Hide();
            OverlayWindow?.Close();
            OverlayWindow = null;
            _tickEngine.Stop();
            Settings.Save();

            base.Dispose();
            _isDisposed = true;
        }

        ~Overlay()
        {
            Dispose();
        }

        // Random shapes.. thanks Julian ^_^
        void SetUp()
        {
           
            

        }
    }
}
