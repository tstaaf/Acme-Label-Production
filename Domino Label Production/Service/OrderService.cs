using Domino_Label_Production.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.Entity;
using System;
using System.Windows;
using System.Threading;

namespace Domino_Label_Production.Service
{
    public class OrderService
    {
        bool let = false;
        public void Watch()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\Users\timmy.staaf\Desktop\Misc\EtikettProduktion\Watcher";
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

                    using (StreamReader reader = new StreamReader(WaitForFile(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
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

                    Orders order = new Orders
                    {
                        TillverkningsOrderNummer = lastread[17],
                        KundNummer = lastread[15],
                        Leveransdatum = lastread[2],
                        AntalRulle = lastread[3],
                        Cylinder = lastread[7],
                        Stans = lastread[8],
                        Diameter = lastread[9],
                        ArtikelNummer = lastread[5],
                        ArtikelNamn = lastread[6],
                        RåMaterialNummer = lastread[20],
                        LotNr = lastread[22]
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
                File.Move(file, @"C:\Users\timmy.staaf\Desktop\Misc\EtikettProduktion\Watcher\Archive\" + Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
