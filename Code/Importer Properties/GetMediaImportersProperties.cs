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





using MeediOS;

namespace EMA.ImporterPropertiesGroups
{


    internal class MediaImporters
    {



                internal static bool GetMediaImportersProperties
                    (int index, IMeedioPluginProperty prop,
                     ref int counter)
                {



                    //if (index == counter++)
                    //{
                    //    prop.Name = "WantToImportFilmsProp";
                    //    prop.Caption = "Import Movies";

                    //    prop.GroupCaption = "Media Importers";

                    //    prop.HelpText = " Would you like the Automated Media Importer " +
                    //                    "to import the films it finds in your root folders? ";
                        
                    //    prop.DefaultValue = Settings.FilmImporterIsEnabled;
                    //    prop.DataType = "bool";
                    //    return true;

                    //}


                    //if (MediaSections.GetMovieImportingProperties
                    //    (index, prop, ref counter))
                    //    return true;

                    return false;

                }


    }


} //endof namespace
