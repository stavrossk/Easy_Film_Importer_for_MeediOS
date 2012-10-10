using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EMA.Core;
using EMA.ImportingEngine;
using MeediOS;




namespace EMA.MediaSnapshotEngine
{


    class MediaImportingEngineHelpers
    {




        internal static string[] CacheMoviesSection
            (IMLSection section)
        {


            MainImportingEngine.CurrentProgress = 0;
            MainImportingEngine.ThisProgress.Progress
                (MainImportingEngine.CurrentProgress,
                "Caching Movies section...");


            int[] itemIDs = section != null
                         ?  section.GetAllItemIDs() 
                         :  new int[] { };


            int itemsCount = itemIDs.Length;
            string[] filmLocations = new string[itemsCount];
            int cacheCounter = 0;


            foreach (IMLItem item in itemIDs.Select
                (id => section != null ? section.FindItemByID(id) : null))
            {

                filmLocations[cacheCounter]
                    = item.Location;

                cacheCounter++;

            }

            return filmLocations;
        }




        internal static ArrayList CacheMediaExtensionsTables
            (string pluginpath,
            out string[] audioExtensions,
             out string[] videoExtensions, 
            out string[] videoExtensionsCommon)
        {


            Helpers.UpdateProgress
                ("","Loading media extension tables...");

            Debugger.LogMessageToFile
                ("Loading media extension tables...");



            #region Cache media extensions tables

            var extensionsToIgnore
                = new ArrayList();

            string nonMediaFile
                = String.Format
                ("{0}Media extensions\\non-media_extensions.txt",
                pluginpath);



            if (!File.Exists(nonMediaFile))
            {
                FileStream filestream = File.Create(nonMediaFile);
                filestream.Close();
            }

            extensionsToIgnore.AddRange(File.ReadAllLines(nonMediaFile));


            videoExtensions = File.Exists
                (String.Format("{0}Media extensions\\video_extensions.txt", pluginpath))
                                  ? File.ReadAllLines(String.Format("{0}Media extensions\\video_extensions.txt", pluginpath))
                                  : null;

            videoExtensionsCommon = File.Exists
                (String.Format("{0}Media extensions\\video extensions common.txt", pluginpath))
                                  ? File.ReadAllLines(String.Format("{0}Media extensions\\video extensions common.txt", pluginpath))
                                  : null;

            audioExtensions = File.Exists
                (String.Format("{0}Media extensions\\audio_extensions.txt", pluginpath))
                                  ? File.ReadAllLines(pluginpath + "Media extensions\\" + "audio_extensions.txt")
                                  : null;

            #endregion



            return extensionsToIgnore;
        }




        internal static void DeleteSnapshotsOnEmptySectionsDetection
            (IEnumerable<FileInfo> datfiles, int moviesItemcount)
        {


            if (moviesItemcount != 0)
                return;

            Helpers.UpdateProgress
                ("","Deleting media snapshots...");

            Debugger.LogMessageToFile
                ("Deleting media snapshots...");


            foreach (FileInfo datfile in datfiles)
                File.Delete(datfile.FullName);
        }





        internal static void CacheMediaSnapshots
            (IList<FileInfo> datfiles)
        {


            Helpers.UpdateProgress
            ("","Caching media snapshots...");

            Debugger.LogMessageToFile
                ("Caching media snapshots...");



            var datEntries = new ArrayList
                [datfiles.Count];


            if (datfiles.Count == 0) 
                return;



            for (int i = 0; i < datfiles.Count; i++)
            {

                datEntries[i] = new ArrayList();

                try
                {
                    string datfile = datfiles[i].FullName;
                    string[] datfileData = File.ReadAllLines(datfile);
                    datEntries[i].Add(datfileData);
                }
                catch (Exception e)
                {


                    Debugger.LogMessageToFile
                        ("[Media Importing Engine] " +
                         "The Media Importing Engine " +
                         "was unable to open a media snapshot" +
                         " file because of an unexpected error." +
                         " The error was: " + e);

                    throw;
                }


            }

        }




        internal static void CountMediaSectionsItems
            (out int moviesItemcount, IMLSection section )
        {


            Helpers.UpdateProgress
                ("","Enumerating media sections...");


            Debugger.LogMessageToFile
                ("Enumerating media sections...");



            #region Movies
            try
            {
                moviesItemcount = section.ItemCount;
            }
            catch (Exception)
            {
                moviesItemcount = 0;
            }
            #endregion



        }


        internal static void PerformPreImportCaching
            (
            IList<FileInfo> datfiles, string pluginpath,
            out string[] filmLocations,
            out ArrayList extensionsToIgnore,
            out string[] videoExtensions,
            out string[] audioExtensions,
            out string[] videoExtensionsCommon,
            IMLSection section
            )
        {

            int moviesItemcount;


            CountMediaSectionsItems
                (out moviesItemcount, section);


            DeleteSnapshotsOnEmptySectionsDetection
                (datfiles, moviesItemcount);


            CacheMediaSnapshots(datfiles);



            extensionsToIgnore = CacheMediaExtensionsTables
                (pluginpath, out audioExtensions,
                out videoExtensions, out videoExtensionsCommon);


            #region Cache Media Sections


            Helpers.UpdateProgress("","Caching media sections...");
            Debugger.LogMessageToFile("Caching media sections...");


            try
            {
                filmLocations 
                    = CacheMoviesSection(section);
               
            }
            catch (Exception e)
            {

                Helpers.UpdateProgress("","Unable to cache media sections.");
                throw;
            }



            #endregion
        
        
        }



        internal static ArrayList TestDirectoryAccessGetFileList
            (string directoryStr, int totalFiles,
            int currentFile, out DirectoryInfo directory)
        {


            if (TestDirectoryAccess
                (directoryStr, out directory))
                return null;


            Application.DoEvents();



            var filesInDirectory 
                = ImportingEngineHelpers
                .GetDirectoryFileList
                (directoryStr, totalFiles,
                currentFile, directory);



            Application.DoEvents();

            return filesInDirectory;

        }




        internal static bool TestDirectoryAccess
            (string directoryStr,
            out DirectoryInfo directory)
        {

            Application.DoEvents();

            directory = ImportingEngineHelpers
                .TestDirectoryAccess(directoryStr);

            return directory == null;
        
        }



        internal static void WriteNonMediaExtensionsToFile
            (ArrayList extensionsToIgnore, string pluginpath)
        {

            File.WriteAllLines
                (pluginpath + "Media extensions\\" + 
                 "non-media_extensions.txt",
                (string[]) extensionsToIgnore
                .ToArray(typeof (string)));

        }



        internal static bool ImportEachFileInDirectory
            (IMLSection section,
             ref ArrayList extensionsToIgnore,
             ref string[] filmLocations,
             string[] videoExtensions,
             string[] audioExtensions,
             int totalFiles,
             ref int currentFile,
             IEnumerable<string> combinedSceneTags,
             ArrayList filesInDirectory,
             string pluginPath,
             IEnumerable<string> videoExtensionsCommon)
        {


            foreach (FileInfo file in filesInDirectory)
            {

                Application.DoEvents();


                if (!SingleMediaFileImporter.ImportMediaFile
                       (file, section, 
                        ref extensionsToIgnore,
                        ref filmLocations,
                        videoExtensions,
                        audioExtensions,
                        totalFiles,
                        ref currentFile,
                        combinedSceneTags,
                        pluginPath,
                        videoExtensionsCommon))
                    return false;


            }


            return true;


        }



        internal static FileInfo[] LoadMediaSnapshots
            (out string pluginpath)
        {


            pluginpath = Debugger.GetPluginPath();


            Debugger.LogMessageToFile
                ("Creating media snapshots path...");

            string snapshotsPath
                = Path.Combine
                (pluginpath, @"Media Snapshots\" );




            var datfiles = new FileInfo[] {};

            try
            {
                
                Helpers.UpdateProgress
                    ("","Retrieving media snapshots...");

                Debugger.LogMessageToFile
                    ("Retrieving media snapshots...");


                var snapshotsPathFI 
                    = new DirectoryInfo(snapshotsPath);

                datfiles = snapshotsPathFI.GetFiles("*.dat");

            }
            catch (Exception)
            {

                Helpers.UpdateProgress
                    ("","Unable to load media snapshots." +
                        " Import sequence terminated.");

                Debugger.LogMessageToFile
                    ("Unable to load media snapshots." +
                     " Import sequence terminated.");

            }


            return datfiles;
        }



        internal static bool RunMediaSectionsUpdatingEngine
            (Importer importer,
             IBaseSystem iBaseSystem,
             IMLSection section,
             IEnumerable<string> combinedSceneTags)
        {

            return !MediaUpdaters
                .UpdateMediaSections
                (iBaseSystem, section,
                combinedSceneTags);


        }






    }


}
