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
