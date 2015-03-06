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
