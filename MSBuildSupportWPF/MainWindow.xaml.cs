using MSBuildSupport.code;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupport.XML;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Reflection.Metadata;
using System.Formats.Tar;
using Microsoft.Win32;
using System;
using MSBuildSupportWPF.tab;
using System.ComponentModel;

namespace MSBuildSupportWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            BuildTabControl.SaveAll();
        }

        private void OpenNewMSBuildFile(object sender, RoutedEventArgs e)
        {
            BuildTabControl.OpenNewMSBuildFile();
        }
    }
}