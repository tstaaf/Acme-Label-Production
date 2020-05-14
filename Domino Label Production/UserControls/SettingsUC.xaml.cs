using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Domino_Label_Production.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsUC.xaml
    /// </summary>
    public partial class SettingsUC : UserControl
    {
        public SettingsUC()
        {
            InitializeComponent();
            MSerieIP.Text = Properties.Settings.Default.MSerieIP;
            PLC1IP.Text = Properties.Settings.Default.PLC1IP;
            PLC2IP.Text = Properties.Settings.Default.PLC2IP;
            QDFilePath.Text = Properties.Settings.Default.QDFilePath;
            FileWatchPath.Text = Properties.Settings.Default.WatchPath;
            FileWatchArchivePath.Text = Properties.Settings.Default.WatchArchive;
            Log1Path.Text = Properties.Settings.Default.Maskin1LogPath;
            Log2Path.Text = Properties.Settings.Default.Maskin2LogPath;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MSerieIP = MSerieIP.Text;
            Properties.Settings.Default.PLC1IP = PLC1IP.Text;
            Properties.Settings.Default.PLC2IP = PLC2IP.Text;
            Properties.Settings.Default.QDFilePath = QDFilePath.Text;
            Properties.Settings.Default.WatchPath = FileWatchPath.Text;
            Properties.Settings.Default.WatchArchive = FileWatchArchivePath.Text;
            Properties.Settings.Default.Maskin1LogPath = Log1Path.Text;
            Properties.Settings.Default.Maskin2LogPath = Log2Path.Text;
            Properties.Settings.Default.Save();
        }
    }
}
