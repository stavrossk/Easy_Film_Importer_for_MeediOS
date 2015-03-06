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
using EMA.ImportingEngine;
using MeediOS;


namespace EMA.MediaSnapshotEngine
{


    internal class MediaSnapshotComparingEngine
    {




        internal static string[]
            LoadAndUpdateStoredMediaSnapshot
            (string datPath, string[] filePaths)
        {



            MainImportingEngine
                .ThisProgress.Progress
                (MainImportingEngine
                .CurrentProgress, 
                "Loading previously " +
                "stored snapshot...");


            string[] oldfilePaths 
                = File.ReadAllLines
                (datPath);


            MainImportingEngine
                .ThisProgress.Progress
                (MainImportingEngine
                .CurrentProgress,
                "Updating media snapshot...");



            #region write new .dat file with the new paths.

            File.WriteAllLines
                (datPath, filePaths);

            #endregion



            return oldfilePaths;

        }





        internal static FileInfo[] CompareMediaSnapshotsAddFilesToScanningList
            (string[] filmLocations, string[] oldfilePaths, string[] filePaths)
        {


            MainImportingEngine
                .ThisProgress.Progress
                (MainImportingEngine
                .CurrentProgress, 
                "Comparing media snapshots...");


            var filesInDir = new FileInfo
                [filePaths.Length];



            for (int i = 0; i < filePaths.Length; i++)
            {


                var matchfound 
                    = FileHasAlreadyBeenScanned
                    (i, filePaths, oldfilePaths);


                var foundInLibrary 
                    = FileExistsInMediaSections
                    (filmLocations, filePaths, i);


                AddFileToScanningList
                    (i, filesInDir,
                    filePaths,
                    matchfound,
                    foundInLibrary);


            }
            return filesInDir;
        }




        private static void AddFileToScanningList
            (int i, IList<FileInfo> filesInDir,
             IList<string> filePaths, bool matchfound,
             bool foundInLibrary)
        {


            #region If no match has been found, add this file to array to scan

            if (
                matchfound &&
                (foundInLibrary || !Settings.RescanFilesNotFoundInLibrary)
                ) return;


            if (File.Exists(filePaths[i]))
                filesInDir[i] = new FileInfo(filePaths[i]);

            #endregion
       
        
        }



        private static bool FileHasAlreadyBeenScanned
            (int i, IList<string> filePaths,
            IEnumerable<string> oldfilePaths)
        {


            bool matchfound = false;

            foreach (string t in oldfilePaths)
                if (filePaths[i] == t)
                    matchfound = true;


            return matchfound;
        }





        internal static string[]
            CacheCurrentFileEntries
            (FileInfo[] allfiles)
        {



            MainImportingEngine
                .ThisProgress.Progress
                (MainImportingEngine
                .CurrentProgress,
                "Caching current file entries...");


            var filePaths 
                = new string
                [allfiles.Length];



            for (int i = 0; i < allfiles.Length; i++)
            {
                try
                {
                    filePaths[i] = allfiles[i].FullName;
                }
                catch (Exception)
                {
                          
                }
                
            }



            return filePaths;
        }






        private static bool FileExistsInMediaSections
            (IEnumerable<string> filmLocations, 
            IList<string> filePaths, int i)
        {

            if (filmLocations != null)
            {

                if (filmLocations.Any
                    (t => filePaths[i] == t))
                    return true;

            }




            return false;
        }






        internal static bool CompareMediaSnapshotsAndImportNewMediaFiles
                    (FileInfo[] allfilesFI, IMLSection moviesSection,
                    ref ArrayList extensionsToIgnore,
                    string[] filmLocations,
                    string[] videoExtensions,
                    string[] audioExtensions,
                    IEnumerable<string> combinedSceneTags,
                    string mediaSnapshotLocation,
                    string pluginPath,
                    IEnumerable<string> videoExtensionsCommon)
        {


            Application.DoEvents();



            var filesToImport
                = CompareSnapshotsAndConstructArrayOfFilesToImport
                (allfilesFI, filmLocations, mediaSnapshotLocation);



            Application.DoEvents();


            return MediaSnapshotEngine
                .ScanConstructedArrayAndImportMediaFiles(
                moviesSection, ref extensionsToIgnore,
                filmLocations, videoExtensions,
                audioExtensions, combinedSceneTags,
                filesToImport, pluginPath,
                videoExtensionsCommon);


        }












        internal static bool CreateNewMediaSnapshotScanDirectoriesImportMediaFiles
                    (string importRootFolder,
                     FileInfo[] allfilesFI,
                     IMLSection moviesSection,
                     ref ArrayList extensionsToIgnore,
                     string[] filmLocations,
                     string[] videoExtensions,
                     string[] audioExtensions,
                     IEnumerable<string> combinedSceneTags,
                     string pluginPath,
                     string mediaSnapshotLocation, 
                     IEnumerable<string> videoExtensionsCommon)
        {

            if (Settings.EnableMediaSnapshots)
            MediaSnapshotEngine
                .ConstructAndWriteNewMediaSnapshot
                (allfilesFI, mediaSnapshotLocation);


            return RecursiveDirectoryScanner
                .ScanSubDirectoriesAndImportMediaFiles
                    (importRootFolder,
                     allfilesFI,
                     moviesSection,
                     ref extensionsToIgnore,
                     filmLocations,
                     videoExtensions,
                     audioExtensions,
                     combinedSceneTags,
                     pluginPath,
                     videoExtensionsCommon);

        
        }






        internal static FileInfo[] CompareSnapshotsAndConstructArrayOfFilesToImport
            (FileInfo[] allfiles, string[] filmLocations, string datPath)
        {


            MainImportingEngine
                .CurrentProgress = 0;



            var filePaths 
                = CacheCurrentFileEntries
                (allfiles);

            var oldfilePaths
                = LoadAndUpdateStoredMediaSnapshot
                (datPath, filePaths);



            FileInfo[] filesInDir
                = CompareMediaSnapshotsAddFilesToScanningList
                (filmLocations,  oldfilePaths, filePaths);




            return filesInDir;    
        }






    }





}
