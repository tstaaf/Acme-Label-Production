using Domino_Label_Production.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.Entity;
using System;
using System.Windows;
using System.Threading;
using System.Text.RegularExpressions;

namespace Domino_Label_Production.Service
{
    public class OrderService
    {
        bool let = false;
        public void Watch()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Properties.Settings.Default.WatchPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.txt";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (let == false)
            {
                using (var entities = new Entities())
                {
                    List<string> lines = new List<string>();

                    using (StreamReader reader = new StreamReader(WaitForFile(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),System.Text.Encoding.Default))
                    {
                        var line = reader.ReadLine();
                        while (line != null)
                        {
                            lines.Add(line);
                            line = reader.ReadLine();
                        }
                    }

                    List<string> lastread = new List<string>();

                    foreach (var l in lines)
                    {
                        string output = l.Split(';').Last();
                        lastread.Add(output);
                    }
                    lastread[7] = lastread[7].Split(':').Last();
                    lastread[8] = lastread[8].Split(':').Last();
                    lastread[11] = lastread[11].Split(':').Last();
                    lastread[6] = lastread[6].Split(',').First();
                    lastread[10] = lastread[10].Split(':').Last();
                    lastread[12] = lastread[12].Split(':').Last();

                    lastread[7] = lastread[7].Trim();
                    lastread[8] = lastread[8].Trim();
                    lastread[11] = lastread[11].Trim();
                    lastread[6] = lastread[6].Trim();
                    lastread[10] = lastread[10].Trim();
                    lastread[12] = lastread[12].Trim();

                    if (lastread[14].Contains("IKEA"))
                    {
                        lastread[6] = lastread[6].Remove(0, 7);
                        lastread[6] = lastread[6] + " " + lastread[5];
                        //lastread[6] = lastread[6].Trim();
                        lastread[5] = lastread[14];
                    }

                    string antalRullar = Regex.Match(lastread[13], @"\d+").Value;

                    Orders order = new Orders
                    {
                        TillverkningsOrderNummer = lastread[1],
                        KundNummer = lastread[15],
                        INFO2 = lastread[16],
                        Leveransdatum = lastread[4],
                        AntalRulle = lastread[11],
                        Cylinder = lastread[7],
                        Stans = lastread[8],
                        Diameter = lastread[9],
                        ArtikelNummer = lastread[5],
                        ArtikelNamn = lastread[6],
                        RåMaterialNummer = lastread[20],
                        LotNr = antalRullar,
                        VORDNR = lastread[17],
                        INFO1 = lastread[24],
                        INFO4 = lastread[10],
                        INFO6 = lastread[12],
                        TANT = lastread[3]
                    };

                    entities.Orders.Add(order);
                    entities.SaveChanges();
                    let = true;
                    MoveFile(e.FullPath);
                }
            }
            else
            {
                let = false;
            }
            
        }

        FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(50);
                }
            }

            return null;
        }
        public static void MoveFile(string file)
        {
            try
            {
                File.Move(file, Properties.Settings.Default.WatchArchive + Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
