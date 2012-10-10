using System;
using EMA;
using MeediOS;
 

namespace MediaFairy.Code.Media_Updaters
    .Single_Item_Updaters.Movie_Item_Updater
{


    class SetFilmItemProcessingFlags
    {

        //REFACTOR: 
        internal static void SetUpdateFlag(IMLItem item)
        {



            if (String.IsNullOrEmpty
                    (Helpers.GetTagValueFromItem
                         (item, "ImdbID")))
            {
                ClearProcessedFlag(item);
                return;
            }



            if (String.IsNullOrEmpty
                    (Helpers.GetTagValueFromItem
                         (item, "Year")))
            {
                ClearProcessedFlag(item);
                return;
            }



            if (String.IsNullOrEmpty
                    (Helpers.GetTagValueFromItem
                         (item, "Title")))
            {
                ClearProcessedFlag(item);
                return;
            }



            if (String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                     (item, "OriginalTitle")))
            {
                ClearProcessedFlag(item);
                return;
            }
                

            if (String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                     (item, "SortTitle")))
            {
                ClearProcessedFlag(item);
                return;
            }
                   


            item.Tags["E.F.I.-processed"]
                = "--processed--";
                
            item.SaveTags();
            
        }


        //REFACTOR: 
        internal static void SetDetailsFlag(IMLItem item)
        {


            if (
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Title")))
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Year"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Actors"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "ActorRoles"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Director")))
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Genre"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "LongOverview")))
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Overview")))
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Rating")))
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "ReleaseDate"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Review"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Runtime"))) 
                &&
                (!String.IsNullOrEmpty
                (Helpers.GetTagValueFromItem
                (item, "Studio")))
                )
            {

                item.Tags
                    ["HasDetails"]
                    = "True";

                item.SaveTags();

            }
            else
            {
                item.Tags
                    ["HasDetails"] 
                    = String.Empty;

                item.SaveTags();

            }

        }



        private static void ClearProcessedFlag(IMLItem item)
        {

            item.Tags["E.F.I.-processed"]
                = String.Empty;

            item.SaveTags();
        }




        internal static void SetSortTitleTag
            (IMLItem item)
        {


            string title = Helpers
                .GetTagValueFromItem
                (item, "Title");


            string sortTitle = Helpers
                .GetTagValueFromItem
                (item, "SortTitle");



            if (String.IsNullOrEmpty(title))
                return;


            if (!String.IsNullOrEmpty(sortTitle))
                return;




            if (title.StartsWith("The"))
            {

                sortTitle = title.Remove
                    (0, 4) + ", The ";


                Debugger.LogMessageToFile
                    (String.Format
                         ("Set item's SortTitle to:" +
                          " '{0}'.", sortTitle));
            


            }
            else
                sortTitle = title;


            item.Tags["SortTitle"]
                = sortTitle;

            item.SaveTags();
        
        
        }



    }



}
