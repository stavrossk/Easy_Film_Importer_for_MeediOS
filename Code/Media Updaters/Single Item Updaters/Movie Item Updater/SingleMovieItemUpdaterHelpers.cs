using System;
using System.Collections.Generic;
using System.IO;
using EMA.ImportingEngine;
using MediaFairy.Code.Media_Updaters.Single_Item_Updaters.Movie_Item_Updater;
using MeediOS;




namespace EMA.SingleItemUpdaters
{


    class SingleMovieItemUpdaterHelpers
    {




                internal static string ExtractTitleFromItem
                (IMLItem item)
                {

                    string itemTitle
                        = Helpers.GetTagValueFromItem
                            (item, "Title");


                    if (!String.IsNullOrEmpty(itemTitle))
                        return itemTitle;

                    itemTitle = item.Name;

                    if (!String.IsNullOrEmpty(itemTitle))
                        return itemTitle;


                    itemTitle = String.Empty;
                    return itemTitle;
                }




        internal static void CleanItemNameSetOriginalTitleTag(IMLItem item)
                        {


                            string itemName = item.Name;

                            if (String.IsNullOrEmpty(itemName))
                                return;

                            itemName = itemName.Replace('_', ' ');
                            itemName = itemName.Replace('.', ' ');
                            item.Name = itemName;


                            string originalTitleTmp = Helpers.GetTagValueFromItem
                                (item, "OriginalTitle");


                            if (!String.IsNullOrEmpty(originalTitleTmp))
                                return;
                    
                            item.Tags["OriginalTitle"] = item.Name;
                            item.SaveTags();
                    
                    
                            Debugger.LogMessageToFile(String.Format(
                                "The item's original name {0} was stored to OrginalTitle tag.",
                                item.Name));
                
                
                        }



                internal static bool ExtractTitleFromItemAndSetSortTitleTag
                            (IMLItem item)
                {

                    

                    if (String.IsNullOrEmpty
                        (ExtractTitleFromItem(item)))
                        return true;


                    SetFilmItemProcessingFlags
                        .SetSortTitleTag
                        (item);


                    return false;
                }





                internal static bool PerformMovieIdentification
                    (IEnumerable<string> combinedSceneTags,
                     DirectoryInfo parent,
                     IMLItem item)
                {
                    return ExtractMovieMetadataFromOfflineSources
                        (combinedSceneTags, item);
                }




        internal static void InitializeItemKeys
            (int itemID, IMLItem item, out string moviehash,
             out string imdbid, out string tmdbID,
             out string year, out string itemTitle,
             out string sortTitle, out string location)
        {


            MainImportingEngine.SpecialStatus
                = String.Format("Updating {0}...", item.Name);


            Debugger.LogMessageToFile
                (String.Format("{0}" +
                "Starting to work with library item:" +
                 " {1} with ID: {2}",
                 Environment.NewLine,
                 item.Name, itemID));
            

            Debugger.LogMessageToFile
                ("Initializing item variables...");
            

            moviehash = Helpers
                .GetTagValueFromItem
                (item, "VideoHash");
            

            imdbid = Helpers
                .GetTagValueFromItem
                (item, "ImdbID");
            

            tmdbID = Helpers
                .GetTagValueFromItem
                (item, "TMDbID");
            

            year = Helpers
                .GetTagValueFromItem
                (item, "Year");
            

            itemTitle = Helpers
                .GetTagValueFromItem
                (item, "Title");
            

            sortTitle = Helpers
                .GetTagValueFromItem
                (item, "SortTitle");


            
            location = item.Location;
            

        }






                internal static bool ExtractMovieMetadataFromOfflineSources
                    (IEnumerable<string> combinedSceneTags, IMLItem item)
                {


                    if (MovieFileMetadataExtractor
                        .ExtractMetadataFromMovieFilename
                        (item, combinedSceneTags))
                        return true;


                    CleanItemNameSetOriginalTitleTag(item);


                    return false;
                }






    }


}
