


using System;
using MeediOS;


namespace EMA
{


    internal class GetImporterProperties
    {



        internal static bool GetDiagnosticsProperties
            (int index,
             IMeedioPluginProperty prop,
             ref int counter)
        {




            // ReSharper disable RedundantAssignment
            if (index == counter++)
            // ReSharper restore RedundantAssignment
            {
                prop.Name = "DebugLogProp";
                prop.Caption = "Write debug log file";

                prop.GroupCaption = "Diagnostics:";


                prop.HelpText = "If enabled, E.F.I. will write" +
                                " a debug log containing" + Environment.NewLine +
                                "important information from the last " +
                                "importing session for debugging purposes.";

                prop.DefaultValue = Settings.WriteDebugLog;
                prop.DataType = "bool";
                return true;
            }




            return false;
        }









    } //endof class




} //endof namespace