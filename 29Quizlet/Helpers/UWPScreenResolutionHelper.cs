using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace _29Quizlet.Helpers
{
    public static class UWPScreenResolutionHelper
    {
        public static Size GetResolution()
        {
            var applicationView = ApplicationView.GetForCurrentView();
            var displayInformation = DisplayInformation.GetForCurrentView();
            var bounds = applicationView.VisibleBounds;
            var scale = displayInformation.RawPixelsPerViewPixel;
            var size = new Size(bounds.Width * scale, bounds.Height * scale);
            return size;
        }
    }
}
