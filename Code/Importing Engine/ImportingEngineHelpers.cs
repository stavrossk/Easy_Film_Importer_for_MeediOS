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
using System.IO;
using System.Collections;
using MeediOS;



namespace EMA.ImportingEngine
{


    public static class ImportingEngineHelpers
    {



                internal static bool
                    PerformInitializationTasks
                    (IMLSection section)
                {


                    MainImportingEngine
                        .CurrentProgress = 0;
            
                    MainImportingEngine.GeneralStatus 
                        = "Performing startup operations...";
          
                    MainImportingEngine.SpecialStatus 
                        = String.Empty;


                    if (Helpers.UserCancels
                        ("E.F.I. is initializing..."))
                        return true;



                    #region Write first debug lines for the importing proccess
                    
                    Debugger.LogMessageToFile
                        (Environment.NewLine);
                    
                    Debugger.LogMessageToFile
                        (Environment.NewLine);
                    
                    Debugger.LogMessageToFile
                        (@"----------------------   
                           START OF MAIN IMPORTING PROCCESS
                           ----------------");

                    Debugger.LogMessageToFile
                        ("Plugin path: "
                        + Debugger.GetPluginPath());


                    #endregion



                    return false;
                }



                internal static void FinishImport()
                {
           
                    MainImportingEngine.CurrentProgress = 100;
            
                    MainImportingEngine.ThisProgress.Progress
                        (MainImportingEngine.CurrentProgress,
                        "E.F.I. completed successfully!");
            
                    Debugger.LogMessageToFile
                        ("E.F.I. completed successfully!");
           
                }





                internal static void BeginUpdatingSections
                    (IMLSection section)
                {

                    section.BeginUpdate();
                                  
                }




                internal static void EndUpdatingSections
                    (IMLSection moviesSection)
                {

                    moviesSection.EndUpdate();

                }



                internal static int ComputeProgress
                    (int currentItemInt, int totalItemsInt)
                {

                    double divider = 0;
                    
                    double currentItem 
                        = Convert.ToDouble(currentItemInt);
                    
                    double totalItems 
                        = Convert.ToDouble(totalItemsInt);
                    
                    //double Remaining
                    //= 100 - Importer.CurrentProgress;
                    
                    double currentProgress;

                    if (Math.Abs(currentItem - 0)
                        > double.Epsilon)
                        divider = totalItems
                            / currentItem;

                    if (Math.Abs(currentItem - 0) < double.Epsilon 
                        || Math.Abs(totalItems - 0) < double.Epsilon
                        || Math.Abs(divider - 0) < double.Epsilon)
                        currentProgress = 0;
                    else currentProgress = 100 / divider;


                    int currentProgressInt 
                        = Convert.ToInt32
                        (currentProgress);
                    
                    
                    return currentProgressInt;

                }



                internal static ArrayList GetDirectoryFileList
                    (string directoryStr, int totalFiles, 
                     int currentFile, DirectoryInfo directory)
                {

                    MainImportingEngine.CurrentProgress
                        = ComputeProgress
                        (currentFile, totalFiles);
            
                    Debugger.LogMessageToFile
                        ("Retrieving the files " +
                         "contained in directory "
                        + directoryStr + "...");
            

                    Helpers.UpdateProgress
                        ("Importing media files...",
                        "Getting file list from directory " 
                        + directoryStr + "...");
            
            
                    var filesInDirectory 
                        = new ArrayList();

                       filesInDirectory.AddRange
                           (directory.GetFiles("*.*"));



                    return filesInDirectory;
                }



                internal static DirectoryInfo 
                    TestDirectoryAccess
                    (string dirStr)
                {

                    Debugger.LogMessageToFile
                        ("Testing access for directory "
                        + dirStr + "...");
            

                    Helpers.UpdateProgress
                        ("Importing media files...",
                        "Testing access for directory "
                        + dirStr + "...");
           

                    DirectoryInfo dir;
                    
                    try
                    {

                        dir = 
                            new DirectoryInfo
                                (dirStr);
                    
                    }
                    catch (Exception e)
                    {
                    
                        Debugger.LogMessageToFile
                            ("Unable to access directory. " +
                             "The following error occured: " + e);
                      
                        return null;
                    
                    }


                    return dir;
        
                }



    }//endof class



}//endof namespace