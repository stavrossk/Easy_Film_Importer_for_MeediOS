//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                    ''
//''    Copyright (C) 2008-2012  Stavros Skamagkis                               ''
//''                                                                             ''
//''    This program is free software: you can redistribute it and/or modify     ''
//''    it under the terms of the GNU General Public License as published by     ''
//''    the Free Software Foundation, either version 3 of the License, or        ''
//''    (at your option) any later version.                                      ''
//''                                                                             ''
//''    This program is distributed in the hope that it will be useful,          ''
//''    but WITHOUT ANY WARRANTY; without even the implied warranty of           ''
//''    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the            ''
//''    GNU General Public License for more details.                             ''
//''                                                                             ''
//''    You should have received a copy of the GNU General Public License        ''
//''    along with this program.  If not, see <http://www.gnu.org/licenses/>.    ''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



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


    internal class DirectoryScanner
    {



        internal static bool ScanMediaDirectories
            (
            string[] mediaFolders, ref ArrayList extensionsToIgnore, 
            string[] filmLocations, string[] videoExtensions,
            string[] audioExtensions, IEnumerable<string> combinedSceneTags,
            IEnumerable<string> videoExtensionsCommon,
            Importer importer, IMLSection section 
            )
        {


            string pluginpath
                = Debugger.GetPluginPath();


            foreach (string importRootFolder in mediaFolders)


                if (
                    !CalculateExaminationtimeAndScanMediaFolder
                    (section,
                    ref extensionsToIgnore, filmLocations,
                    videoExtensions, audioExtensions,
                    importRootFolder, pluginpath, combinedSceneTags,
                    videoExtensionsCommon )
                    )
                    return false;
            

            return true;
        }



        private static bool CalculateExaminationtimeAndScanMediaFolder
            (IMLSection moviesSection,
            ref ArrayList extensionsToIgnore,
            string[] filmLocations,
            string[] videoExtensions, 
            string[] audioExtensions,
            string importRootFolder,
            string pluginpath,
            IEnumerable<string> combinedSceneTags, 
            IEnumerable<string> videoExtensionsCommon)
        {


            if (ValidateRootDirectory
                (importRootFolder))
                return true;



            #region File Importer varibales

            MainImportingEngine.CurrentProgress = 0;
            MainImportingEngine.GeneralStatus = "Importing Media files";
            MainImportingEngine.SpecialStatus = String.Empty;

            #endregion




            try
            {


                var allfiles
                    = ScanDirectoriesConstructIncludedDirectoriesAndFilesArrays
                    (importRootFolder);

                Application.DoEvents();


                if (
                    !MediaSnapshotEngine
                    .IfMediaSnaphotExists_ScanDirectoriesAndImportNewFiles_ElseCreateNewSnapshotAndImportFilesFromScratch
                            (importRootFolder, pluginpath, allfiles, moviesSection,
                             ref extensionsToIgnore,filmLocations, videoExtensions,
                            audioExtensions, combinedSceneTags, videoExtensionsCommon))
                    return false;


            }
            catch (Exception e)
            {

                Debugger
                    .LogMessageToFile
                    (e.ToString());

            }

            return true;
        
        }




        private static bool ValidateRootDirectory
            (string importRootFolder)
        {


            MainImportingEngine.ThisProgress.Progress
                (MainImportingEngine.CurrentProgress,
                "Validating root directory...");


            if (!Directory.Exists
                (importRootFolder))
                return true;


            Debugger.LogMessageToFile
                ("Import root directory" +
                 " was validated.");

            return false;

        }



        private static FileInfo[] ScanDirectoriesConstructIncludedDirectoriesAndFilesArrays
            (string importRootFolder)
        {


            Application
                .DoEvents();


            var dirsTotal 
                = CalculateTotalDirectoryCount
                (importRootFolder);



            Helpers.UpdateProgress
                ("Importing Media Files...",
                "Total Directories: "
                + dirsTotal);

            Application.DoEvents();
            //Thread.Sleep(2000);


            int dirCounter = 0;
            FileInfo[] allfiles;


            ExamineDirectoryConstructFilesArray
                (importRootFolder, out allfiles,
                 dirsTotal, ref dirCounter);


            Helpers.UpdateProgress
                ("Importing Media Files...",
                "Total files: " + allfiles.Length);


            Application.DoEvents();
            //Thread.Sleep(2000);

            return allfiles;
        }



        private static int CalculateTotalDirectoryCount
            (string importRootFolder)
        {


            MainImportingEngine.ThisProgress
                .Progress(MainImportingEngine.CurrentProgress,
                "Calculating remaining time...");

            Debugger.LogMessageToFile
                ("Calculating root media" +
                 " directory examination time...");


            int dirsTotal = 0;
            int currentDir = 0;

            try
            {

                dirsTotal = GetDirectoryCount
                    (importRootFolder,
                    ref currentDir);


            }
            catch (Exception e)
            {

                Debugger.LogMessageToFile
                    ("The root directory enumerator" +
                     " caused an exception: " + e);

            }

            return dirsTotal;

        }



        internal static int GetDirectoryCount
            (string dir, ref int dirCount)
        {


            if (!dir.EndsWith("\\"))
                dir = dir + "\\";

            string[] allFiles
                = Directory
                .GetFileSystemEntries(dir);


            //loop through all items
            foreach (string file in allFiles
                .Where(Directory.Exists))
            {

                //recursive call
                dirCount++;

                Helpers.UpdateProgress
                    ("Importing Media Files...",
                    "[Total Directories: " + dirCount + 
                    "] Counting directories...");


                Application.DoEvents();

                GetDirectoryCount
                    (file, ref dirCount);
            
            }
       


            return dirCount;

        }



        internal static int ExamineDirectoryConstructFilesArray
            (string strDir, out FileInfo[] allfiles,
            int dirsTotal, ref int i)
        {


            var directoriesArray = new ArrayList();
            var filesList = new ArrayList();


            string[] allDirs
                = Directory.GetDirectories
                (strDir);


            #region ...for each directory in Root...
            foreach (string strDirName in allDirs)
            {


                try
                {

                    MainImportingEngine.CurrentProgress
                        = ImportingEngineHelpers
                        .ComputeProgress(i, dirsTotal);


                    MainImportingEngine.ThisProgress
                        .Progress(MainImportingEngine
                        .CurrentProgress, "[files found: "
                        + filesList.Count + " ]" +
                        " Examining directory " 
                        + strDirName + "...");



                    var dirI = new DirectoryInfo
                        (strDirName);



                    try
                    {
                        filesList.AddRange
                            (dirI.GetFiles
                            ("*.*", SearchOption.AllDirectories));

                        directoriesArray.AddRange
                            (dirI.GetDirectories
                            ("*.*", SearchOption
                            .AllDirectories));


                    }
                    catch
                    {
                        continue;
                    }


                    i++;

                    ExamineDirectoryConstructFilesArray
                        (strDirName, out allfiles,
                        dirsTotal, ref i);



                }
                catch (Exception e)
                {

                    Debugger.LogMessageToFile
                        ("An unexpected error occured" +
                         " in the media directories examination process." +
                         " The error was: " + e);

                }


            }

            #endregion


            allfiles = (FileInfo[])
                filesList.ToArray
                (typeof(FileInfo));

           
            return allfiles.Length;
        
        }



        internal static bool ImportFilesFromDirectory
            (
            string directoryStr,
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

            DirectoryInfo directory;


            var filesInDirectory =
                MediaImportingEngineHelpers
                .TestDirectoryAccessGetFileList
                (directoryStr, totalFiles,
                 currentFile, out directory);


            if (filesInDirectory == null)
                return true;



            return MediaImportingEngineHelpers
                .ImportEachFileInDirectory
                (moviesSection,
                ref extensionsToIgnore,
                ref filmLocations,
                videoExtensions, 
                audioExtensions,
                totalFiles,
                ref currentFile, 
                combinedSceneTags, 
                filesInDirectory, 
                pluginPath,
                videoExtensionsCommon);
      

        
        }
 
    
    
    }



}
