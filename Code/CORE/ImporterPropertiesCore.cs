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
