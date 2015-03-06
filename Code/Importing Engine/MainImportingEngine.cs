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
using System.Collections.Generic;
using EMA.Core;
using EMA.MediaSnapshotEngine;
using MeediOS;



namespace EMA.ImportingEngine
{


    public class MainImportingEngine
    {



                internal static void StartMainImportingEngine
                    (Importer importer,
                     IBaseSystem iBaseSystem,
                     IMLSection section, 
                     IEnumerable<string> combinedSceneTags)
                {


                    GeneralStatus = "Performing media importing...";
                    SpecialStatus = String.Empty;



                    MediaImportingEngine.ImportMediaFilesMain
                        (combinedSceneTags, importer, section);



                    MediaImportingEngineHelpers
                        .RunMediaSectionsUpdatingEngine
                        (importer, iBaseSystem, section,
                        combinedSceneTags);


                }




                public static bool DoImport
                    (Importer importer,
                     IBaseSystem iBaseSystem,
                     IMLSection section,
                     IMLImportProgress progress)
                {


                    try
                    {

                        ThisProgress = progress;
                        Settings.ImportingStarted = false;
                        Settings.ImportingCompleted = false;


                        MediaLocationsRetriever
                            .RetrieveMediaLocations
                            (importer);


                        if (ImportingEngineHelpers
                            .PerformInitializationTasks(section))
                            return true;


                        var combinedSceneTags = 
                            VideoFilenameCleaner
                            .ReadDictionaryReturnAllTags();



                        StartMainImportingEngine
                            (importer,
                             iBaseSystem,
                             section,
                             combinedSceneTags);



                        ImportingEngineHelpers
                            .FinishImport();



                        return true;

                    }
                    catch (Exception e)
                    {

                        Debugger.LogMessageToFile
                            ("An unexpected error ocurred " +
                             "in the main import method DoImport(). " +
                             "The error was: " + e);
                    }


                    return true;
                }




                internal static bool Initialized;
        
                internal static IMLImportProgress
                    ThisProgress;
        
                internal static int CurrentProgress;
        
                internal static string GeneralStatus 
                    = String.Empty;
        
                internal static string SpecialStatus 
                    = String.Empty;




    }//endof class


}//endof namespace
