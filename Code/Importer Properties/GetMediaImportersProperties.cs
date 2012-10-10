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
