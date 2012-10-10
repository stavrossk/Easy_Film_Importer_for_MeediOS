//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                    ''
//''    Copyright (C) 2008-2011  Stavros Skamagkis                               ''
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


using System.Collections.Generic;
using System.Collections;
using System.IO;
using EMA.Core;
using EMA.ImportingEngine;
using EMA.MediaSnapshotEngine;
using MeediOS;


namespace EMA
{


    class MediaImportingEngine
    {

                public static void ImportMediaFilesMain
                    (IEnumerable<string> combinedSceneTags,
                    Importer importer, IMLSection section)
                {


                    string pluginpath;

                    FileInfo[] mediaSnapshotsFI = 
                        MediaImportingEngineHelpers.LoadMediaSnapshots
                        (out pluginpath);



                    Debugger.LogMessageToFile
                            ("Media File Importer is enabled.");
                    
    
                    if (Settings.RootMediaFolders == null 
                        || Settings.RootMediaFolders.Length == 0)
                    {
                        return;
                    }
                        



                    #region Declare Vars
                    string[] filmLocations;
                    string[] musicLocations;
                    string[] tvShowsLocations;

                    ArrayList extensionsToIgnore;
                    string[] videoExtensions;
                    string[] videoExtensionsCommon;
                    string[] audioExtensions;
                    #endregion




                    MediaImportingEngineHelpers.PerformPreImportCaching
                        (mediaSnapshotsFI, pluginpath, out filmLocations,
                        out extensionsToIgnore, out videoExtensions, 
                        out audioExtensions, out videoExtensionsCommon,
                        section);



                    DiskMediaScanInitiator.ScanDiskForEntertainmentMedia
                        (combinedSceneTags, videoExtensions, 
                        audioExtensions, pluginpath,
                        filmLocations, extensionsToIgnore, 
                        videoExtensionsCommon, importer, section);
       
        
        
                }



    }//endof class



} //endof namespace
