using Microsoft.Win32;
using MSBuildSupportWPF.tab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace MSBuildSupportWPF.UI.UIComponents
{
    public class BuildTabControl : TabControl
    {
        private Dictionary<string, TabItem> alreadyInTab = new Dictionary<string, TabItem>();
        private List<BuildTab> BuildTabs = new List<BuildTab>();
        public void OpenNewMSBuildFile()
        {
            string defaultPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\examples"));
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = defaultPath;
            openFileDialog.Filter = "MSBuild files (*.csproj; *.xml)|*.csproj;*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (alreadyInTab.ContainsKey(filePath))
                {
                    SelectedItem = alreadyInTab[filePath];
                    return;
                }
                string tabName = Path.GetFileName(filePath);
                TabItem newTabItem = new TabItem
                {
                    Header = tabName,
                    Name = "Test"
                };
                Frame pageFrame = new Frame();
                Page buildTab = new BuildTab(tabName, filePath);
                pageFrame.Navigate(buildTab);
                BuildTabs.Add((BuildTab)buildTab);
                newTabItem.Content = pageFrame;
                Items.Add(newTabItem);
                SelectedItem = newTabItem;
                alreadyInTab.Add(filePath, newTabItem);
            }
        }
        public void SaveAll()
        {
            foreach (BuildTab tab in BuildTabs) {
                tab.SaveXml();
            }
        }

    }
}
