using System;
using System.Windows.Forms;
using EMA.Core;


namespace EMA.ImportingEngine
{


    class MediaLocationsRetriever
    {


        protected internal static void RetrieveMediaLocations
            (Importer importer)
        {


                Helpers.UpdateProgress
                    ("","Retrieving media locations...");


                Debugger.LogMessageToFile(
                    "Retrieving media locations...");


                string filmsMediaFolder 
                    =  MediaFolders.API.Folders
                    (importer.Ibs, importer.MfSettingsMovies);





            string importRootFoldersStr = filmsMediaFolder;

                Settings.RootMediaFolders = 
                    importRootFoldersStr.Split(new[] { '|' },
                    StringSplitOptions.None);


                RetrieveFilmFolders(importer);


        }





                private static void RetrieveFilmFolders(Importer importer)
                {

                    try
                    {

                        Settings.FilmsFolders = MediaFolders.API.Folders
                            (importer.Ibs, importer.MfSettingsMovies).Split
                            (new[] {'|'}, StringSplitOptions.None);


                    }
                    catch (Exception e)
                    {


                        // ReSharper disable LocalizableElement
                        MessageBox.Show(
                            "The media importing engine was unable to retrieve" +
                            " the directories containing your Movies media files. " +
                            "Please make sure that you have provided your media locations" +
                            "for this type of your media files in the plugin's" +
                            " 'Media Folders' settings group and try again.",
                            "Movies Media Folders were not specified", MessageBoxButtons.OK);
                        // ReSharper restore LocalizableElement


                        Debugger.LogMessageToFile("[Media Importing Engine] Unable to retrieve" +
                                                  " user's media files locations." +
                                                  "The error was: " + Environment.NewLine + e);

                    }


                }


    }


}
