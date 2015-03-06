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
