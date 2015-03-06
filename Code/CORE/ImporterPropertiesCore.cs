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
using EMA.Core;




namespace EMA.CORE
{


    class ImporterPropertiesCore
    {


                protected internal static bool EditMediaFoldersSettings
                    (Importer importer, string propertyName, ref string value)
                {
                        return GetEntertainmentMediaLocations
                               (importer, propertyName, ref value);
                }







                private static bool GetEntertainmentMediaLocations
                    (Importer importer, string propertyName, ref string value)
                {


                    string oldValue = value;


                    if (String.Compare(propertyName,
                        "AdvancedSettingsFoldersa",
                        StringComparison.OrdinalIgnoreCase) == 0)
                    {


                        MediaFolders.API.OpenSettingsWindow
                            (ref value, importer.Ibs);

                        if (String.CompareOrdinal
                            (value, "Canceled") == 0)
                            value = oldValue;

                        return true;

                    }








                    return false;
                }



    }


}
