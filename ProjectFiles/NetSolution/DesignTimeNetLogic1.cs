#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.DataLogger;
using FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.ODBCStore;
using FTOptix.Recipe;
using FTOptix.OPCUAServer;
using FTOptix.Alarm;
using FTOptix.WebUI;
#endregion

public class DesignTimeNetLogic1 : BaseNetLogic
{
    private IUAVariable numAllarmi;

    [ExportMethod]
    public void MioPrimoMetodo()
    {
        // Insert code to be executed by the method
        Log.Info("This is an info message");

    }

    [ExportMethod]
    public void MioSecondoMetodo()
    {
        // Insert code to be executed by the method
        if (Project.Current.GetVariable("Model/MyVar") == null)
        {
            var myVar = InformationModel.MakeVariable("MyVar", OpcUa.DataTypes.Float);
            Project.Current.Get("Model").Add(myVar);
        }
        else
            Log.Error("La variabile già esiste!");

        var varDaCollegare = Project.Current.GetVariable("Model/Temperatura");
        Project.Current.GetVariable("Model/MyVar").SetDynamicLink(varDaCollegare,DynamicLinkMode.ReadWrite);
    }

    [ExportMethod]
    public void CreaAllarmi()
    {
        numAllarmi = LogicObject.GetVariable("NumAllarmi");

        for (int i = 0; i <= numAllarmi.Value; i++)
        {
            var mioAllarme = InformationModel.Make<DigitalAlarm>("Allarme_" + i);
            mioAllarme.Message = "Test message " + i;
            mioAllarme.Severity = (ushort)i;
            Project.Current.Get("Alarms").Add(mioAllarme);
        }
    }

    [ExportMethod]
    public void CancellaAllarmi()
    {
        foreach (var item in Project.Current.Get("Alarms").Children)
        {
            item.Delete();
        }
    }

    }
