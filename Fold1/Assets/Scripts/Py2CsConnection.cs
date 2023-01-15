using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class Py2CsConnection : MonoBehaviour
{
    static Socket listener;
    private CancellationTokenSource source;
    private ManualResetEvent allDone;
    public Slider scaleSlider;
    public Slider rotationSlider;
    
    public float scaleMinValue;
    public float scaleMaxValue;
    private float scaleValue;
    public float rotateMinValue;
    public float rotateMaxValue;

    public static readonly int PORT = 1755;
    public static readonly int WAITTIME = 1;


    void ScaleIncreament()
    {
        source = new CancellationTokenSource();
        allDone = new ManualResetEvent(false);
    }

    async void Start()
    {
        scaleSlider = GameObject.Find("ScaleSlider").GetComponent<Slider>();
        scaleSlider.minValue = scaleMinValue;
        scaleSlider.maxValue = scaleMaxValue;

        rotationSlider = GameObject.Find("RotationSlider").GetComponent<Slider>();

        await Task.Run(()=> ListenEvents(source.Token)); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }

    private void ListenEvents(CancellationToken token)
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, PORT);

        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (!token.IsCancellationRequested)
            {
                allDone.Reset();
                Debug.Log("Wait");
                
            }
        }
        catch
        { 
            
        }
    }
}
