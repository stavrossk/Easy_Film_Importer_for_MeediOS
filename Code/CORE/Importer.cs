//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Adder Lite Edition for Meedio/MeediOS for Meedio and MeediOS 
//'' 
//''    An import-type plugin for Meedio and MeediOS Home Theater applications     
//''
//''    Copyright (C) 2008-2012  Stavros Skamagkis                               ''
//''     
//''
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
using System.Windows.Forms;
using EMA.CORE;
using EMA.ImportingEngine;
using MeediOS;




#region Structures



public struct Movie
{
    public string Name;
    public string Year;
    public string ID;
    public string Hash;
    public string Imdbid;
    public string Tagline;
    public string Duration;
    public string Plot;
    public string Directors;
    public string Cast;
    public string Writers;
    public string Genres;

    public string Trivia;
    public string Goofs;

    public string Cover;

}




public struct TargetVideo
{
    public IMLItem Item;
    public string Videofilename;
    public string TargetPath;

}





#endregion




namespace EMA.Core
{


        public class Importer :
            IMLImportPlugin,
            IDisposable,
            IMeedioInitializablePlugin
        {



                public bool Import
                    (IMLSection section,
                     IMLImportProgress progress)
                {

                    return MainImportingEngine.DoImport
                        (this, Ibs, section, progress);
                
                }





                #if USE_MEEDIO 
                
                public bool EditCustomProperty
                    (int Window,
                     string PropertyName,
                     ref string Value)
                
                #elif USE_MEEDIOS

                public bool EditCustomProperty
                    (IntPtr window,
                    string propertyName, 
                    ref string value)
            
                #endif
                {

                    return ImporterPropertiesCore.EditMediaFoldersSettings
                        (this, propertyName, ref value);
         
                }





                #region MediaFolders Settings
                internal string MfSettingsMovies;
                #endregion





                #region Importer Properties (Get/Set)
      
 
                /// <summary>
                /// Retrieving config settings from configuration.exe and at startup from MeediOSApp 
                /// </summary>
                /// <PARAM name="properties">Containing the list of settings</PARAM><PARAM name="errorText">Returns the error text</PARAM>
                /// <returns>
                /// Returns true on success, false on error.
                /// </returns>
                /// 
                /// 
                public bool SetProperties
                    (IMeedioItem Properties,
                    out string errorText)
                {


                    errorText = null;


                    try
                    {


                        //LogPluginSettings(Properties);


                        PropertiesSetter.SetProperties(Properties);
                         

                        #region MediaFolders

                        MfSettingsMovies = Properties
                            ["AdvancedSettingsFoldersa"].ToString().Trim();

                        #endregion


                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show
                            (ex.ToString());

                        return false;
                    }



                    return true;
                }












            private static void LogPluginSettings
                (IMeedioItem Properties)
            {


                string propertiesString =
                    Properties[0].ToString();

                string[] properties
                    = propertiesString.Split('¦');

                string pluginPath
                    = Debugger.GetPluginPath();

                string debugLogPath
                    = pluginPath + "Debug.log";



                DeleteDebugFile(debugLogPath);




                #region Write Plugin Settings to Debug.log

                File.AppendAllText(pluginPath + "Debug.log",
                                   @"------------------------   START OF PLUGIN SETTINGS   ---------------------");
                foreach (string property in properties)
                {
                    File.AppendAllText(pluginPath + "Debug.log", Environment.NewLine);
                    File.AppendAllText(pluginPath + "Debug.log", property);
                }
                File.AppendAllText(pluginPath + "Debug.log", Environment.NewLine);
                File.AppendAllText(pluginPath + "Debug.log",
                                   @"------------------------   END OF PLUGIN SETTINGS   ---------------------");
                File.AppendAllText(pluginPath + "Debug.log", Environment.NewLine);

                #endregion



            }





            private static void DeleteDebugFile
                (string debugLogPath)
            {


                try
                {

                    File.Delete
                        (debugLogPath);

                }
                catch (Exception e)
                {

                    Debugger.LogMessageToFile(
                        "An error occured while trying" +
                        " to delete the debug log file." +
                        " The error was: " + e);

                }


            }




                /// <summary>
                /// Defining config entries
                /// </summary>
                /// <PARAM name="propertyIndex">Parameter number</PARAM><PARAM name="propertyDefinition">Parameter definition</PARAM>
                /// <returns>
                /// Return true if propertyDefinition is filled with valid data, false if no property with index propertyIndex exists.
                /// </returns>
                public bool GetProperty
                    (int index, IMeedioPluginProperty prop)
                {


                    try
                    {



                        int counter = 1;
                        MainImportingEngine
                            .Initialized
                            = Initialize();



                        if (ImporterPropertiesGroups.
                            MediaImporters.GetMediaImportersProperties
                            (index, prop, ref counter))
                            return true;



                        if (GetImporterProperties.GetDiagnosticsProperties
                            (index, prop, ref counter))
                            return true;







                        #region Media Folders

                        if (index == counter++)
                        {
                            prop.Caption = "Films Folders Settings";
                            prop.GroupCaption = "Media Folders:";

                            prop.HelpText = "Customize the Folders settings";
                            prop.DataType = "custom";
                            prop.DefaultValue = "";
                            prop.Name = "AdvancedSettingsFoldersa";
                            return true;
                        }

                        #endregion Media Folders



                    }

                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }


                    return false;
                }




            #endregion
             
        













                private static Boolean Initialize()
                {
                    try
                    {
                        if (!MainImportingEngine.Initialized)
                        new GetImporterProperties();   
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                        return false;
                    }
                    return true;
                }

                public void Initalize(IBaseSystem baseSystem)
                {
                    Ibs = baseSystem;
                }



            #region IDisposable Members

                public void Dispose()
                {
                    Dispose(true);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    //Snapshot();
                    GC.SuppressFinalize(this);
                }

                protected virtual void Dispose(Boolean disposing)
                {
                    if (disposing)
                    {
                        //ImportFunc.Dispose();
                        //ImportFunc = null;
                        //ImporterSettings.Dispose();
                        //ImporterSettings = null;
                        //TagMasks.Clear();
                        //TagMasks = null;
                        //LogMessages.Dispose();
                        //LogMessages = null;
                        //Log = null;
                        //Marshal.ReleaseComObject(Section);
                        //Marshal.ReleaseComObject(Progress);
                        //Progress = null;
                        //Section = null;
                        //Snapshot();
                        // Free other state (managed objects).
                    }
                    // Free your own state (unmanaged objects).
                    // Set large fields to null.
                }

                #endregion




            internal IBaseSystem Ibs;







    }//endof  class





}//endof namespace