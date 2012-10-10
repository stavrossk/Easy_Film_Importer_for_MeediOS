


namespace EMA
{


    public class Settings
    {


                public static string[] RootMediaFolders;

                public static bool UserCancels;

                public static bool ImportingStarted;
                
                public static bool ImportingCompleted;
                
                public static bool WriteDebugLog = true;
        
                public static bool FileInUse;


                internal static bool
                    OverrideAutomatedMovieTitleLocator;

                internal static string
                    MovieTitleLocationInPath 
                    = "Parent folder";
            


                internal static bool
                    ExtractMetadataFromFilenames = true;

                internal static bool 
                    RescanFilesNotFoundInLibrary = true;

                internal static bool
                    ImportDvdFolders = true;


                //TODO: Implement Media Type Detection option.
                internal static bool
                    EnableMediaDetection;



                internal static bool EnableMediaSnapshots;


                internal static string[]
                    FilmsFolders 
                    = new string[1];



                internal static string
                    NowImportingMediaType 
                    = string.Empty;



                internal static bool
                    UpdateMediaSectionOnEachImportedItem = true;
              



    }



}
