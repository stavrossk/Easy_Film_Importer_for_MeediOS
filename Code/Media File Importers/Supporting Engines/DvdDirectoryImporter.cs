using System.IO;
using System.Threading;
using System.Windows.Forms;
using EMA.ImportingEngine;
using EMA.MediaSnapshotEngine;
using MeediOS;
using System;

namespace EMA.MediaImporters
{


    class DvdDirectoryImporter
    {


        internal static bool ImportDvdDirectory
            (FileSystemInfo file, IMLSection moviesSection, 
            string parentName, DirectoryInfo parent)
        {



            if (!Settings.ImportDvdFolders)
                return false;


            if (!file.Name.Contains
                ("video_ts.ifo")
                && 
                !file.Name.Contains
                ("VIDEO_TS.IFO"))
                return false;



            string filmTitle;
            IMLItem item;

            if (String.Compare(parentName, "video_ts", true) == 0)
            {
                var secondParent = parent.Parent;

                if (secondParent != null)
                {
                    filmTitle = secondParent.Name;

                    Application.DoEvents();

                    MainImportingEngine.ThisProgress.Progress
                        (MainImportingEngine.CurrentProgress, 
                        String.Format("Importing dvd film {0}...", filmTitle));

                    Thread.Sleep(1500);
                   
                    MediaSectionPopulator.AddFileToSection(out item, moviesSection,
                        filmTitle, file.FullName, file.FullName);

                    return true;
                }

                return false;
            }

            filmTitle = parent.Name;

            Application.DoEvents();
            
            MainImportingEngine.ThisProgress.Progress
                (MainImportingEngine.CurrentProgress, String.Format
                ("Importing dvd film {0}...", filmTitle));

            Thread.Sleep(1500);

            MediaSectionPopulator.AddFileToSection(out item, moviesSection,
                filmTitle, file.FullName, file.FullName);

            return true;

        }
    }


}
