using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine.UI;

public class NARSHost : MonoBehaviour
{
    private static NARSHost _instance;
    Process process = null;
    StreamWriter messageStream;

    public OperationOutputGUI UIOutput;
    string lastOperation = "";
    bool operationUpdated = false;

    private void Start()
    {
        _instance = this;
        //LaunchNARS();
        LaunchONA();
    }

    private void Update()
    {
        if (operationUpdated)
        {
            UIOutput.SetOutputText(lastOperation);
            operationUpdated = false;
        }
    }

    public static NARSHost GetInstance()
    {
        return _instance;
    }


    public void LaunchONA()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(@"cmd.exe");
        startInfo.WorkingDirectory = Application.dataPath + @"\NARS";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(ONAOutputReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        process.Start();
        process.StandardInput.WriteLine("NAR shell");
        process.StandardInput.Flush();

        process.BeginOutputReadLine();
        messageStream = process.StandardInput;
        AddInput("*volume=0");
    }

    public void LaunchNARS()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
        startInfo.WorkingDirectory = Application.dataPath + @"\NARS";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(NARSOutputReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        process.Start();
        process.StandardInput.WriteLine("java -jar opennars.jar");
        process.StandardInput.Flush();

        process.BeginOutputReadLine();
        messageStream = process.StandardInput;
        AddInput("*volume=0");
    }

    public void AddInferenceCycles(int cycles)
    {
        AddInput("" + cycles);
    }

    public void AddInput(string message)
    {
        UnityEngine.Debug.Log("SENDING INPUT: " + message);
        messageStream.WriteLine(message);
    }
    void NARSOutputReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        UnityEngine.Debug.Log(eventArgs.Data);
        if (eventArgs.Data.Contains("EXE")) //operation executed
        {
            UnityEngine.Debug.Log("RECEIVED OUTPUT: " + eventArgs.Data);
            string operation = eventArgs.Data.Substring(eventArgs.Data.IndexOf("^"), eventArgs.Data.IndexOf("("));

            if (operation == "^left")
            {
                UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                NARSSensorimotor.GetInstance().MoveLeft();

                lastOperation = operation;
                operationUpdated = true;
            }
            else if (operation == "^right")
            {
                UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                NARSSensorimotor.GetInstance().MoveRight();

                lastOperation = operation;
                operationUpdated = true;
            }
            else if (operation == "^deactivate")
            {
                UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                NARSSensorimotor.GetInstance().DontMove();

                lastOperation = operation;
                operationUpdated = true;
            }
        }

    }

    void ONAOutputReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        //UnityEngine.Debug.Log(eventArgs.Data);
        if (eventArgs.Data.Contains("executed with args")) //operation executed
        {
            string operation = eventArgs.Data.Split(' ')[0];
          
            if (operation == "^left")
            {
               // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                NARSSensorimotor.GetInstance().MoveLeft();

                lastOperation = operation;
                operationUpdated = true;
            }
            else if(operation == "^right")
            {
               // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                NARSSensorimotor.GetInstance().MoveRight();

                lastOperation = operation;
                operationUpdated = true;
            }
            else if (operation == "^deactivate")
            {
               // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                NARSSensorimotor.GetInstance().DontMove();

                lastOperation = operation;
                operationUpdated = true;
            }
        }
        
    }

    void ErrorReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        UnityEngine.Debug.LogError(eventArgs.Data);
    }

    void OnApplicationQuit()
    {
        if (process != null || !process.HasExited )
        {
            process.CloseMainWindow();
        }
    }

}



