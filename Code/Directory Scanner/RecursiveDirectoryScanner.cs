using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using EMA.ImportingEngine;
using MeediOS;



namespace EMA.MediaSnapshotEngine
{




    class RecursiveDirectoryScanner
    {




        internal static bool ScanDirectoryRecursively
            (
            string strDir,
            IMLSection moviesSection,
            ref ArrayList extensionsToIgnore,
            ref string[] filmLocations,
            string[] videoExtensions,
            string[] audioExtensions, 
            int totalFiles,
            ref int currentFile,
            string pluginPath,
            IEnumerable<string> combinedSceneTags,
            IEnumerable<string> videoExtensionsCommon              
            
            )
        {


                MainImportingEngine.GeneralStatus
                    = "Performing media importing...";



                if (Helpers.UserCancels
                    ("Scanning directory "
                    + strDir + "..."))
                    return false;



                if (!DirectoryScanner
                    .ImportFilesFromDirectory
                    (strDir,
                     moviesSection, 
                     ref extensionsToIgnore,
                     ref filmLocations, 
                     videoExtensions,
                     audioExtensions,
                     totalFiles,
                     ref currentFile,
                     pluginPath,
                     combinedSceneTags,
                     videoExtensionsCommon ))

   



                    return false;


                IEnumerable<string> directories
                    = new BindingList<string>();



                try
                {

                    directories
                        = GetSubDirectories
                        (strDir);


                }
                catch (PathTooLongException e)
                {


                    //MessageBox.Show
                    //    (@"Path too long");


                    Debugger.LogMessageToFile
                        ("[Directory scanner]" +
                         " An unexpected error occured " +
                         "while the Directory scanner" +
                         " was trying to retrieve " +
                         "sub-directories of the directory "
                         + strDir + 
                         ". The error was: " + e);


                }
               

                return
                    directories == null || 
                    ScanSubDirectories
                    (moviesSection,
                    ref extensionsToIgnore,
                    ref filmLocations,
                    videoExtensions,
                    audioExtensions,
                    totalFiles,
                    ref currentFile,
                    pluginPath,
                    directories,
                    combinedSceneTags,
                    videoExtensionsCommon);


       
        }


        private static IEnumerable<string>
            GetSubDirectories
            (string strDir)
        {


            string[] directories;

            try
            {


                directories 
                    = Directory
                    .GetDirectories
                    (strDir);


            }
            catch
            {
                return null;
            }



            return directories;
        }







        //TODO: make all these parameters a struct
        private static bool ScanSubDirectories
            (IMLSection moviesSection,
            ref ArrayList extensionsToIgnore,
            ref string[] filmLocations,
            string[] videoExtensions, 
            string[] audioExtensions,
            int totalFiles,
            ref int currentFile, 
            string pluginPath, 
            IEnumerable<string> directories,
            IEnumerable<string> combinedSceneTags,
            IEnumerable<string> videoExtensionsCommon)

        {


            foreach (string strDirSub in directories)
            {

                if (!ScanDirectoryRecursively
                    (strDirSub, moviesSection,
                    ref extensionsToIgnore,
                    ref filmLocations,
                    videoExtensions,
                    audioExtensions,
                    totalFiles,
                    ref currentFile,
                    pluginPath,
                    combinedSceneTags,
                    videoExtensionsCommon))

                    return false;
            }

            return true;
        }


        internal static bool ScanSubDirectoriesAndImportMediaFiles
                   (string importRootFolder, 
                    ICollection<FileInfo> allfilesFI,
                    IMLSection moviesSection,
                    ref ArrayList extensionsToIgnore,
                    string[] filmLocations,
                    string[] videoExtensions,
                    string[] audioExtensions,
                    IEnumerable<string> combinedSceneTags,
                    string pluginPath,
                    IEnumerable<string> videoExtensionsCommon)
        {
            try
            {
                Application.DoEvents();

                int currentFile = 0;

                if (
                    !ScanDirectoryRecursively
                         (
                             importRootFolder,
                             moviesSection,
                             ref extensionsToIgnore,
                             ref filmLocations,
                             videoExtensions,
                             audioExtensions,
                             allfilesFI.Count,
                             ref currentFile,
                             pluginPath,
                             combinedSceneTags,
                             videoExtensionsCommon
                         )
                    )
                    return false;

                Application.DoEvents();
            }
            catch (Exception e)
            {

                Debugger.LogMessageToFile
                    ("The root media directory scanner" +
                     " returned an exception: " + e);

            }

            return true;
        }
   
    
    }




}
