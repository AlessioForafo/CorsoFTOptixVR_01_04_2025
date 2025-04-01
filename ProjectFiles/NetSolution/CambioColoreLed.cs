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

public class CambioColoreLed : BaseNetLogic
{
    private IUAVariable variabilePLC;
    private PeriodicTask myPeriodicTask;

    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        variabilePLC = Project.Current.GetVariable("Model/ColoreLed");
        variabilePLC.VariableChange += VariabilePLC_VariableChange;
        cambioColore(variabilePLC.Value);

        myPeriodicTask = new PeriodicTask(IncrementVariable, 1000, LogicObject);
        myPeriodicTask.Start();
    }

    private void VariabilePLC_VariableChange(object sender, VariableChangeEventArgs e)
    {
       cambioColore(e.NewValue);
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        variabilePLC = Project.Current.GetVariable("Model/ColoreLed");
        variabilePLC.VariableChange -= VariabilePLC_VariableChange;
        myPeriodicTask.Dispose();
    }

    private void IncrementVariable()
    {
        variabilePLC.Value = variabilePLC.Value + 1;
    }

    public void cambioColore(int numColore)
    {
        var mioled = (Led)Owner;
        switch (numColore)
        {
            case 0:
                mioled.Color = Colors.Red;
                break;
            case <10:
                mioled.Color = Colors.Yellow;
                break;
            case < 20:
                mioled.Color = Colors.Lime;
                break;
            default:
                break;
        }
    }
}
