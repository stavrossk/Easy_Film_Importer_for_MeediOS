using MeediOS;



namespace EMA
{



    public class PropertiesSetter
    {



        internal static void SetProperties
            (IMeedioItem properties)
        {




                //if (properties["MovieTitleLocatorProp"] != null)
                //    Settings.OverrideAutomatedMovieTitleLocator = 
                //        (bool) properties["MovieTitleLocatorProp"];
         

                //if (properties["MovieTitleLocationProp"] != null)
                //    Settings.MovieTitleLocationInPath =
                //        (string) properties["MovieTitleLocationProp"];


            //if (properties["WantToImportFilmsProp"] != null)
            //    Settings.FilmImporterIsEnabled = (bool)properties["WantToImportFilmsProp"];


            //SetMovieDetailsProperties(properties);
 

                //if (properties["ExtractMetadataFromFilenamesProp"] != null)
                //    Settings.ExtractMetadataFromFilenames =
                //        (bool) properties["ExtractMetadataFromFilenamesProp"];




                if (properties["DebugLogProp"] != null)
                    Settings.WriteDebugLog =
                        (bool) properties["DebugLogProp"];




                //if (properties["ImportDvdFoldersProp"] != null)
                //    Settings.ImportDvdFolders =
                //         (bool) properties["ImportDvdFoldersProp"];



        }




    }//endof class


}//endof namespace

