using System;
using System.IO;
using System.Collections.Generic;
using EMA.ImportingEngine;
using MediaFairy.Code.Media_Updaters
.Single_Item_Updaters.Movie_Item_Updater;
using MeediOS;





namespace EMA.SingleItemUpdaters
{



    class SingleMovieItemUpdater
    {



                internal static bool UpdateFilmItem
                    (int itemID,
                     IMLSection moviesSection,
                     string pluginpath,
                     ref int currentItem,
                     int totalItems,
                     IBaseSystem iBaseSystem,
                     IEnumerable<string> combinedSceneTags)
                {



                    MainImportingEngine
                        .CurrentProgress =
                        ImportingEngineHelpers
                        .ComputeProgress
                        (currentItem, totalItems);




                    #region item variables


                    Helpers.UpdateProgress("", "Fetching library item...");
                    Debugger.LogMessageToFile( "Fetching library item...");


                    var item = moviesSection
                        .FindItemByID(itemID);

                    if (item == null)
                        return true;


                    DirectoryInfo parent;


                    string moviehash,
                        imdbid,
                        tmdbID,
                        year,
                        itemTitle,
                        sortTitle,
                        location;



                    SingleMovieItemUpdaterHelpers.InitializeItemKeys
                        (itemID, item, out moviehash, out imdbid,
                         out tmdbID, out year, out itemTitle,
                         out sortTitle, out location);


                

                    #endregion



                    Helpers.UpdateProgress("", "Creating filesystem instance...");

                    Debugger.LogMessageToFile(String.Format
                        ("Creating filesystem instances for media file {0}...",
                        location));


                    try
                    {


                        var fi = new FileInfo
                            (location);

                        parent = fi.Directory;


                    }
                    catch (Exception e)
                    {

                        Debugger.LogMessageToFile(String.Format
                            ("Unable to create filesystem instances" +
                             " for this media file." +
                             "An error occured: {0}", e));

                        return true;
                                                            
                    }





                    if (IdentifyFilmAndDownloadMetadata
                        (combinedSceneTags,
                        item, parent))
                        return true;


                    if (Helpers.UserCancels
                        (MainImportingEngine.SpecialStatus))
                        return false;



                    SetFilmItemProcessingFlags
                        .SetUpdateFlag(item);


                    item.SaveTags();
                    currentItem++;
                    return true;

                }




                private static bool IdentifyFilmAndDownloadMetadata
                    (IEnumerable<string> combinedSceneTags,
                     IMLItem item, DirectoryInfo parent)
                    {


                    //TODO: Change this and other instances to "E.F.I.-processed"
                    string processedFlag = Helpers
                        .GetTagValueFromItem
                        (item, "E.F.I.-processed");


                    if (String.CompareOrdinal
                            (processedFlag,
                             "--processed--") == 0)
                        return false;
                    
                    
                    
                    if (SingleMovieItemUpdaterHelpers
                        .PerformMovieIdentification
                        (combinedSceneTags,
                         parent, item))
                        return true;



                    SetFilmItemProcessingFlags
                        .SetDetailsFlag(item);


                    return false;

                    }





    }//endof class




}//endof namespace
