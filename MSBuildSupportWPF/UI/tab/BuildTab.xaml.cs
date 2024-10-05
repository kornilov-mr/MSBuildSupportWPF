using MSBuildSupport.code.codeBlocks;
using MSBuildSupport.code;
using MSBuildSupport.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Timer = System.Timers.Timer;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.IO;
using System.Diagnostics;
using System.Timers;
using System.Xml;
using System.Reflection.Emit;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using MSBuildSupportWPF.UI.UIComponents;
using System.Security.RightsManagement;

namespace MSBuildSupportWPF.tab
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class BuildTab : Page
    {
        private BlockTree Blocktree { get; }
        private Timer RebuildTimer { get; }
        private string TabName { get; }
        private string FilePath { get; }
        private MainCodeDisplay MainCodeDisplay { get; }


        public BuildTab(string tabName, string filePath)
        {
            InitializeComponent();

            TabName = tabName;
            FilePath = filePath;

            ErrorPopup errorPopup = new ErrorPopup();
            MainBuildGrid.Children.Add(errorPopup);

            RebuildTimer = new Timer(400);
            RebuildTimer.Elapsed += RebuildTreeTimeEvent;
            RebuildTimer.AutoReset = false;

            Blocktree =new BlockTree();
            MainCodeDisplay = new MainCodeDisplay(Blocktree, RebuildTimer, errorPopup);

            MainBuildGrid.Children.Add(MainCodeDisplay);

            XMLDocument xmlDocument = new XMLDocument(File.ReadAllText(filePath));
            MainCodeDisplay.RebuildAndLoadTree(xmlDocument);

            
        }


        private void RebuildTreeTimeEvent(Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(
                    new Action(() => {
                        string xml = new TextRange(MainCodeDisplay.Document.ContentStart, MainCodeDisplay.Document.ContentEnd).Text;
                        MainCodeDisplay.RebuildAndLoadTree(new XMLDocument(xml));
                        })
                    );
        }
        public void SaveXml()
        {
            string xml = new TextRange(MainCodeDisplay.Document.ContentStart, MainCodeDisplay.Document.ContentEnd).Text;
            if (!String.Equals(xml, ""))
            {
                File.WriteAllText(FilePath, xml);
            }
        }
    }
}
