using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EMA.ImportingEngine;
using MeediOS;



// ReSharper disable CheckNamespace
namespace EMA.MediaSnapshotEngine
// ReSharper restore CheckNamespace
{


    class MediaSnapshotEngine
    {



                internal static void ConstructAndWriteNewMediaSnapshot
                    (IList<FileInfo> allfilesFI, string datPath)
                {



                    MainImportingEngine.ThisProgress.Progress
                        (MainImportingEngine.CurrentProgress,
                        "Scanning directories...");
            

                    Application.DoEvents();


                    var filePaths 
                        = ConstructFilePathsArray
                        (allfilesFI);


                    WriteConstructedFilePathsToMediaSnapshotFile
                        (filePaths, datPath);

                }





                internal static void WriteConstructedFilePathsToMediaSnapshotFile
                    (string[] filePaths, string datPath)
                {


                    MainImportingEngine.ThisProgress.Progress
                        (MainImportingEngine.CurrentProgress,
                        "Saving snapshot...");


                    Application.DoEvents();

                    File.WriteAllLines
                        (datPath, filePaths);
            
                    Application.DoEvents();
        
                }



        internal static string[] ConstructFilePathsArray
            (IList<FileInfo> allfiles)
        {

            var filePaths = new string[allfiles.Count];

            for (int i = 0; i < allfiles.Count; i++)
            {
                try
                {
                    filePaths[i] = allfiles[i].FullName;
                }
                catch (Exception e)
                {

                    Debugger.LogMessageToFile
                        ("[Media Snapshot Constructor]" +
                         " An unexpected error occured while " +
                         "the Media Snapshot Constructor was trying to create" +
                         " a file entry for the media snapshot." +
                         "The error was: " + e);

                }

            }

            return filePaths;

        }


        internal static bool ScanConstructedArrayAndImportMediaFiles
               (IMLSection section,
                ref ArrayList extensionsToIgnore,
                string[] filmLocations,
                string[] videoExtensions,
                string[] audioExtensions,
                IEnumerable<string> combinedSceneTags,
                ICollection<FileInfo> filesToImport,
                string pluginPath,
                IEnumerable<string> videoExtensionsCommon)


        {



            if (!Settings.UpdateMediaSectionOnEachImportedItem)
                ImportingEngineHelpers.BeginUpdatingSections(section);

            if ( !ImportFilesInArray
                (section, ref extensionsToIgnore, filmLocations,
                 videoExtensions, audioExtensions,
                combinedSceneTags, filesToImport, pluginPath, videoExtensionsCommon)
               ) return false;


            if (!Settings.UpdateMediaSectionOnEachImportedItem)
                ImportingEngineHelpers.EndUpdatingSections
                    (section);


            return true;
        }





        private static bool ImportFilesInArray
            (IMLSection section,
             ref ArrayList extensionsToIgnore,
             string[] filmLocations,
             string[] videoExtensions,
             string[] audioExtensions, 
             IEnumerable<string> combinedSceneTags,
             ICollection<FileInfo> filesToImport,
             string pluginPath,
             IEnumerable<string> videoExtensionsCommon)
        {


            if (filesToImport.Count <= 0)
                return true;

            int currentFile = 0;



            foreach (FileInfo file in filesToImport)
            {

                Application.DoEvents();


                if (SingleMediaFileImporter
                    .ImportMediaFile
                       (file,
                        section,
                        ref extensionsToIgnore,
                        ref filmLocations,
                        videoExtensions,
                        audioExtensions,
                        filesToImport.Count,
                        ref currentFile,
                        combinedSceneTags,
                        pluginPath,
                        videoExtensionsCommon))
                    continue;



                ImportingEngineHelpers.EndUpdatingSections
                    (section);


                MainImportingEngine.
                    CurrentProgress = 100;


                Helpers.UpdateProgress
                    ("All operations were cancelled." +
                     " Completed jobs were save to library.", "");


                MainImportingEngine.ThisProgress = null;

                return false;

            }


            return true;
        }




        internal static string ConstructMediaSnapshotLocation
            (string importRootFolder, string pluginpath)
        {

            var rootDirectory = new DirectoryInfo
                (importRootFolder);


            string datName = rootDirectory.Name;


            datName = datName
                .TrimEnd('\\');

            datName = datName
                .TrimEnd(':');



            string datPath = Path.Combine
                (pluginpath, "Media Snapshots\\");
            
            string mediaSnapshotLocation 
                = String.Format
                ("{0}{1}.dat", datPath, datName);

            
            return mediaSnapshotLocation;

        }


        // ReSharper disable InconsistentNaming
        internal static bool IfMediaSnaphotExists_ScanDirectoriesAndImportNewFiles_ElseCreateNewSnapshotAndImportFilesFromScratch
            // ReSharper restore InconsistentNaming
            // ReSharper restore InconsistentNaming
            (string importRootFolder,
             string pluginpath,
             FileInfo[] allfilesFI,
             IMLSection moviesSection,
             ref ArrayList extensionsToIgnore,
             string[] filmLocations,
             string[] videoExtensions,
             string[] audioExtensions,
             IEnumerable<string> combinedSceneTags, 
             IEnumerable<string> videoExtensionsCommon)
        {


            var mediaSnapshotLocation
                = ConstructMediaSnapshotLocation
                (importRootFolder, pluginpath);


            string pluginPath =
                Debugger.GetPluginPath();
           

            bool mediaSnapshotExists
                = File.Exists
                (mediaSnapshotLocation);



            if (mediaSnapshotExists)
            {

                if (!MediaSnapshotComparingEngine
                    .CompareMediaSnapshotsAndImportNewMediaFiles
                    (allfilesFI, moviesSection,
                     ref extensionsToIgnore, filmLocations,
                    videoExtensions, audioExtensions,
                    combinedSceneTags, mediaSnapshotLocation, 
                    pluginPath, videoExtensionsCommon ))
                    return false;

            }
            else
            {

                if (!MediaSnapshotComparingEngine.CreateNewMediaSnapshotScanDirectoriesImportMediaFiles(
                    importRootFolder,
                    allfilesFI,
                    moviesSection,
                    ref extensionsToIgnore,
                    filmLocations, 
                    videoExtensions,
                    audioExtensions,
                    combinedSceneTags,
                    pluginPath,
                    mediaSnapshotLocation,
                    videoExtensionsCommon))
                    return false;

            }


            return true;
        }
    }



}
