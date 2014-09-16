using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Bargool.Acad.Library.WPF
{
    class PalettesetTextBox : TextBox
    {
        public PaletteSet Paletteset { get; set; }

        public PalettesetTextBox()
            : base()
        {
            GotFocus += PalettesetTextBox_GotFocus;
            LostFocus += PalettesetTextBox_LostFocus;
        }

        void PalettesetTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Paletteset != null)
                this.Paletteset.KeepFocus = false;
        }

        void PalettesetTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Paletteset != null)
                this.Paletteset.KeepFocus = true;
        }
    }
}
