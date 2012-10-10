using System;
using MeediOS;


namespace EMA.ImporterPropertiesGroups
{



    internal class MediaSections
    {





        internal static bool GetMovieImportingProperties
            (int index, IMeedioPluginProperty prop, ref int counter)
        {


            if (index == counter++)
            {

                prop.Name = "ImportDvdFoldersProp";
                //set name shown to user
                prop.Caption
                    = "Import DVD film back-ups" +
                      "(preserved disc structure)";


                prop.GroupCaption = "Movie Importer Settings";

                //set the tool tip
                prop.HelpText = "";
                prop.DefaultValue = Settings.ImportDvdFolders;
                prop.DataType = "bool";
                return true;

            }


            if (index == counter++)
            {

                prop.Name = "MovieTitleLocatorProp";
                //set name shown to user

                prop.Caption = "Override automated movie title locator";

                prop.GroupCaption = "Movie Importer Settings";

                //set the tool tip
                prop.HelpText =
                    " If the Automated film title locator" +
                    " fails to correctly locate the titles of your films" +
                    " in their paths (file name or folder name)," +
                    Environment.NewLine + "please check" +
                    " this setting to manually override it.";


                prop.DefaultValue = Settings
                    .OverrideAutomatedMovieTitleLocator;

                prop.DataType = "bool";
                return true;
            }


            if (index == counter++)
            {

                prop.Name = "MovieTitleLocationProp";
                //set name shown to user


                prop.Caption = "Movie's title location in path: ";

                prop.GroupCaption = "Movie Importer Settings";


                prop.HelpText =
                    "Please specify here the location" +
                    " of your movies' titles in their path.";
          
                prop.DefaultValue = Settings
                    .MovieTitleLocationInPath;

                prop.DataType = "string";
                
                var twochoices = new string[2];
                twochoices[0] = "Parent folder";
                twochoices[1] = "Video filename";

                prop.CanTypeChoices = false;
                prop.Choices = twochoices;
                prop.IsMandatory = false;
                return true;
            }

            return false;
        
        
        }





    }


}
