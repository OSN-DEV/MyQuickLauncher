using MyQuckLauncher.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace MyQuckLauncher.Util {
    class AppUtil {
        /// <summary>
        /// create bitmap image from icon file
        /// </summary>
        /// <param name="icon">icon file</param>
        public static BitmapImage CreateImgeFromIconFile(string icon) {
            // https://fkmt5.hatenadiary.org/entry/20130729/1375090831
            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.CacheOption = BitmapCacheOption.OnLoad;
            bmpImage.CreateOptions = BitmapCreateOptions.None;
            bmpImage.UriSource = new Uri(icon);
            bmpImage.EndInit();
            bmpImage.Freeze();
            return bmpImage;
        }

        /// <summary>
        /// create app icon
        /// </summary>
        /// <param name="inputFile">input file name</param>
        /// <param name="iconFile">icon file name</param>
        public static void CreateAppIcon(string inputFile, string iconFile) {
            using (var icon = System.Drawing.Icon.ExtractAssociatedIcon(inputFile)) {
                if (System.IO.File.Exists(iconFile)) {
                    System.IO.File.Delete(iconFile);
                }
                icon.ToBitmap().Save(iconFile, ImageFormat.Png);
            }
        }

        /// <summary>
        /// create directory icon
        /// </summary>
        /// <param name="inputFile">input file name</param>
        /// <param name="iconFile">icon file name</param>
        public static void CreateDirectoryIcon(string inputFile, string iconFile) {
            var shinfo = new NativeMethod.SHFILEINFO();
            IntPtr hImg = NativeMethod.SHGetFileInfo(
              inputFile, 0, out shinfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeMethod.SHFILEINFO)),
              (uint)(NativeMethod.SHGFI.SHGFI_ICON | NativeMethod.SHGFI.SHGFI_LARGEICON));
            if (IntPtr.Zero != hImg) {
                using (var icon = System.Drawing.Icon.FromHandle(shinfo.hIcon)) {
                    icon.ToBitmap().Save(iconFile, ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// show error message
        /// </summary>
        /// <param name="message">message</param>

        public static void ShowErrorMsg(string message) {
            MessageBox.Show(message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
