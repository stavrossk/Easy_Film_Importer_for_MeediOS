using System;
using System.Collections.Generic;
using EMA.ImportingEngine;
using EMA.SingleItemUpdaters;
using MeediOS;



namespace EMA.MediaSectionUpdaters
{

    class MoviesSectionUpdater
    {




        internal static bool UpdateMoviesSection
            (string pluginpath,
             IBaseSystem iBaseSystem,
             IEnumerable<string> combinedSceneTags,
             IMLSection section)
        {



            if (section.ItemCount == 0)
                return true;



            Debugger.LogMessageToFile
                (Environment.NewLine
                + Environment.NewLine 
                + "Film Updater is enabled.");



            #region Pre-Updating Operations


            Debugger.LogMessageToFile
                ("Initializing section variables...");


            int totalItems = section.ItemCount;


            int currentItem = 1;
            

            MainImportingEngine
                .CurrentProgress = 0;
            

            MainImportingEngine
                .GeneralStatus
                = "Updating Movies Section";
            

            MainImportingEngine
                .SpecialStatus = "";


            #endregion



            #region Main Updating Loop


            Debugger.LogMessageToFile
                ("Beginning to update" +
                 " Films section...");
            

            
                
            section.BeginUpdate();
            



           
            foreach (int id in section.GetAllItemIDs())
            {

                if (!SingleMovieItemUpdater
                    .UpdateFilmItem
                    (id, section,
                     pluginpath,
                     ref currentItem,
                     totalItems,
                    iBaseSystem,
                    combinedSceneTags))
                    return false;

            }



            section.EndUpdate();
            

            Debugger.LogMessageToFile
                ("Completing updating of Films section...");


            #endregion




            return true;
        }





    }//endof class


}//endof namespace
