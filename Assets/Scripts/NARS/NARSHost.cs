using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using static UnityEngine.Networking.UnityWebRequest;
using System.Text.RegularExpressions;

public class NARSHost : MonoBehaviour
{

    public static NARSType NARS1 = NARSType.OpenNARS;
    public static NARSType NARS2 = NARSType.OpenNARS;



    public enum NARSType : int
    {
        OpenNARS, ONA, NARSPython, PyNARS
    }

    public NARSType type;
    NARSSensorimotor _sensorimotor;
    Process process = null;
    StreamWriter messageStream;

    //UI output text
    public OperationOutputGUI UIOutput;
    string lastOperationTextForUI = "";
    bool operationUpdated = false;

    //Babbling
    float babbleTimer = 0;
    [SerializeField] int babblesRemaining;
  
    private void Start()
    {
        Application.targetFrameRate = 60;
        babblesRemaining = 60;
        switch (type)
        {
            case NARSType.OpenNARS:
                LaunchNARS();
                break;
            case NARSType.ONA:
                LaunchONA();
                break;
            case NARSType.NARSPython:
                LaunchPython();
                babblesRemaining = 0;
                break;
            case NARSType.PyNARS:
                LaunchPyNARS();
                babblesRemaining = 0;
                break;
            default:
                return;
        }

        _sensorimotor = GetComponent<NARSSensorimotor>();
        _sensorimotor.SetNARSHost(this);

        string text = this.type.ToString() + " Operation:\n";
        text += "NONE";
        UIOutput.SetOutputText(text);
    }

    public NARSSensorimotor GetSensorimotor()
    {
        return _sensorimotor;
    }

    private void Update()
    {
        if (operationUpdated)
        {
            string text = this.type.ToString() + " Operation:\n";
            text += lastOperationTextForUI;
            UIOutput.SetOutputText(text);
            operationUpdated = false;
        }


        babbleTimer -= Time.deltaTime;
        if (babblesRemaining > 0 && babbleTimer <= 0f)
        {
            switch (type)
            {
                case NARSType.OpenNARS:
                    NARSBabble();
                    break;
                case NARSType.NARSPython:
                    PythonBabble();
                    break;
                default:
                    break;
            }
                 
            babbleTimer = 1.00f;
            babblesRemaining--;
        }
        
    }

    void PythonBabble()
    {
        int randInt = UnityEngine.Random.Range(1, 4);
        string input = "";

        if (randInt == 1)
        {
            GetSensorimotor().MoveRight();

            lastOperationTextForUI = "(babble) ^right";
            operationUpdated = true;

            input = "<(*,{SELF}) --> right>. :|:";
        }
        else if (randInt == 2)
        {
            GetSensorimotor().MoveLeft();

            lastOperationTextForUI = "(babble) left";
            operationUpdated = true;

            input = "<(*,{SELF}) --> left>. :|:";
        }
        else if (randInt == 3)
        {
            GetSensorimotor().DontMove();

            lastOperationTextForUI = "(babble) deactivate";
            operationUpdated = true;

            input = "<(*,{SELF}) --> deactivate>. :|:";
        }

        if (input != "")
        {
            this.AddInput(input);
        }

    }
    void NARSBabble()
    {
        int randInt = UnityEngine.Random.Range(1, 4); 
        string input = "";

        if (randInt == 1)
        {
            GetSensorimotor().MoveRight();

            lastOperationTextForUI = "(babble) ^right";
            operationUpdated = true;

            input = "<(*,{SELF}) --> ^right>. :|:";
        }
        else if (randInt == 2)
        {
            GetSensorimotor().MoveLeft();

            lastOperationTextForUI = "(babble) ^left";
            operationUpdated = true;

            input = "<(*,{SELF}) --> ^left>. :|:";
        }
        else if (randInt == 3) 
        {
            GetSensorimotor().DontMove();

            lastOperationTextForUI = "(babble) ^deactivate";
            operationUpdated = true;

            input = "<(*,{SELF}) --> ^deactivate>. :|:";
        }

        if (input != "")
        {
            this.AddInput(input);
        }
    }

    public void LaunchNARS()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
        startInfo.WorkingDirectory = Application.dataPath + "/NARS/OpenNARS";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(NARSOutputReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.StandardInput.WriteLine("java -Xmx1024m -jar opennars.jar");
        process.StandardInput.Flush();

        messageStream = process.StandardInput;
        AddInput("*volume=0");
    }

    public void LaunchONA()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
        startInfo.WorkingDirectory = Application.dataPath + "/NARS/ONA";
        UnityEngine.Debug.Log(startInfo.WorkingDirectory);
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(ONAOutputReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.StandardInput.WriteLine("NAR shell");
        process.StandardInput.Flush();

        messageStream = process.StandardInput;
        AddInput("*volume=0");
    }


    public void LaunchPython()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
        startInfo.WorkingDirectory = Application.dataPath + "/NARS/NARS-Python";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(PythonOutputReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        messageStream = process.StandardInput;
        process.StandardInput.WriteLine("main.exe");

        process.StandardInput.Flush();
    }

    public void LaunchPyNARS()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
        startInfo.WorkingDirectory = Application.dataPath + "/NARS/PyNARS";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(PyNARSOutputReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        messageStream = process.StandardInput;
        process.StandardInput.WriteLine("Console.exe");

        process.StandardInput.Flush();

        if (File.Exists(pynars_log_filename)) File.Delete(pynars_log_filename); // delete old log
    }

    public void AddInferenceCycles(int cycles)
    {
        AddInput("" + cycles);
    }

    public void AddInput(string message)
    {
        
        if(type == NARSType.NARSPython)
        {
            int index = message.IndexOf("<");
            int lastindex = message.LastIndexOf(">");
            if (index != -1 && lastindex != -1){
                message = message.Substring(0, index) + "(" + message.Substring(index + 1, lastindex - (index + 1)) + ")" + message.Substring(lastindex + 1);
            }
            
        }
        UnityEngine.Debug.Log("SENDING INPUT: " + message);
        messageStream.WriteLine(message);
    }
    void NARSOutputReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        UnityEngine.Debug.Log(eventArgs.Data);
        if (eventArgs.Data.Contains("EXE:")) //operation executed
        {
            //UnityEngine.Debug.Log("RECEIVED OUTPUT: " + eventArgs.Data);
            int length = eventArgs.Data.IndexOf("(") - eventArgs.Data.IndexOf("^");
            string operation = eventArgs.Data.Substring(eventArgs.Data.IndexOf("^"), length);
            //UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

            if (operation == "^left")
            {
                GetSensorimotor().MoveLeft();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^right")
            {
                GetSensorimotor().MoveRight();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^deactivate")
            {
                GetSensorimotor().DontMove();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
        }

    }

    void ONAOutputReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        UnityEngine.Debug.Log(eventArgs.Data);
        if (eventArgs.Data.Contains("executed with args")) //operation executed
        {
            string operation = eventArgs.Data.Split(' ')[0];
          
            if (operation == "^left")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().MoveLeft();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if(operation == "^right")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().MoveRight();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^deactivate")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().DontMove();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
        }
        
    }

    void PythonOutputReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        UnityEngine.Debug.Log(eventArgs.Data);
        if (eventArgs.Data.Contains("EXE:")) //operation executed
        {

            string[] words = eventArgs.Data.Split(' ');
            string operation = null;
            foreach(string op in words)
            {
                if(op[0] == '^')
                {
                    operation = op;
                }
            }
            UnityEngine.Debug.Log("RECEIVED OPERATION: " + operation);
            if (operation == "^left")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().MoveLeft();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^right")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().MoveRight();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^deactivate")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().DontMove();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
        }

    }

    StreamWriter log_streamWriter;
    string pynars_log_filename = "PyNARS_Log.txt";
    void PyNARSOutputReceived(object sender, DataReceivedEventArgs eventArgs)
    {
        string ansi_escaped_text = Regex.Replace(eventArgs.Data, @"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])", "");



        if(log_streamWriter == null)
        {
            log_streamWriter = new(pynars_log_filename);
        }
        log_streamWriter.WriteLine(ansi_escaped_text);



        UnityEngine.Debug.Log(ansi_escaped_text);
        if (eventArgs.Data.Contains("EXE:")) //operation executed
        {

            string[] words = eventArgs.Data.Split(' ');
            string operation = null;
            foreach (string op in words)
            {
                if (op[0] == '^')
                {
                    operation = op;
                }
            }
            UnityEngine.Debug.Log("RECEIVED OPERATION: " + operation);
            if (operation == "^left")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().MoveLeft();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^right")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().MoveRight();

                lastOperationTextForUI = operation;
                operationUpdated = true;
            }
            else if (operation == "^deactivate")
            {
                // UnityEngine.Debug.Log("RECEIVED OUTPUT: " + operation);

                GetSensorimotor().DontMove();

                lastOperationTextForUI = operation;
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
        log_streamWriter.Close();
        process.CloseMainWindow();
    }


    public static void SetNARS1(string nars)
    {
        NARS1 = (NARSType)Enum.Parse(typeof(NARSType), nars, true);
    }
    public static void SetNARS2(string nars)
    {
        NARS2 = (NARSType)Enum.Parse(typeof(NARSType), nars, true);
    }

}



