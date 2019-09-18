using System;
using SDRSharp.Radio;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace SDRSharp.SDRplay
{
    public sealed class SDRplayDevice : IDisposable
    {
        private GCHandle _gcHandle;

        // Constants
        private int NUM_BANDS = 10;
        private int LO120MHz = 24576000;
        private int LO144MHz = 22000000;
        private int LO168MHz = 19200000;

        //General
        private volatile bool Running;
        private volatile bool _SET_AGC_Update = false;

        //Frequency Variables
        private double[] band_fmin = { 0.1, 12.0, 30.0, 60.0, 120.0, 250.0, 375.0, 395.0, 420.0, 1000.0 };
        private double[] band_fmax = { 11.999999, 29.999999, 59.999999, 119.999999, 249.999999, 374.999999, 394.999999, 419.999999, 999.999999, 2000.0 };
        private static int CurrentFreqBand, LastFreqBand = 0;

        //Sample Rate Variables
        private float[] SampleRateTable = { 2000, 2000, 3000, 2000, 4000, 3000, 7000, 2000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 8192 };       //Sample Rates Give in kHz to prevent rounding errors
        private int[] DownSampleTable =   { 9,    6,    7,    4,    7,    4,    8,    2,    0,    0,    0,    0,    0,    0,    0,    0,     };
        private int SDRplaySampleRateIndex;
        private uint Decimation;
        private SDRplayIO.mir_sdr_If_kHzT IFTypeDecimation;     
        private bool DownconversionEnabled;

        private readonly SamplesAvailableEventArgs _eventArgs = new SamplesAvailableEventArgs();

        //Thread & Buffers      
        private UnsafeBuffer IBuffer;
        private UnsafeBuffer QBuffer;
        private UnsafeBuffer IQBuffer;
        private UnsafeBuffer IBufferDecimation;
        private UnsafeBuffer QBufferDecimation;
        private unsafe short* IDecimation = null;
        private unsafe short* QDecimation = null;
        private Thread _ReaderThread;

        //Internal SDRplay Variables
        private SDRplayIO.mir_sdr_Bw_MHzT SDRplayIFBandwidth;
        private SDRplayIO.mir_sdr_If_kHzT SDRplayIFType;
        private int SDRplayIFMode;
        private int SDRplayGainReduction;
        private int SDRplaySetpoint;
        private bool SDRplayAGCEnabled;
        private int SDRplayLNAGRThresh;
        private int NoUsbSamplesReturned;
        private int NumberOfSamples;
        private long SDRplayFrequency;
        private double SDRplayFQPPM;

        //USB Latency Toggles
        volatile bool grToggle, LastGrToggle;

        public unsafe void SDRplay_Initalisation()
        {
                int DownSampleFactor;
                bool DownconversionEnabledTS;
                SDRplayIO.mir_sdr_ErrT res;

                Running = true;
                res = NativeMethods.mir_sdr_Init(GainReduction, (SampleRateTable[SampleRateIndex] / 1e3), NominalFrequency, Bandwidth, IFFrequency, out NoUsbSamplesReturned);
                
                if (res == SDRplayIO.mir_sdr_ErrT.mir_sdr_Success)
                {
                    NativeMethods.mir_sdr_SetDcMode(1, 0);                      
                    NativeMethods.mir_sdr_SetGrParams(0, LNAGRThresh);
                    NativeMethods.mir_sdr_SetGr(GainReduction, 1, 0);
                    NativeMethods.mir_sdr_ResetUpdateFlags(1, 1, 1);

                    if (IBuffer != null)
                        IBuffer.Dispose();
                    if (QBuffer != null)
                        QBuffer.Dispose();
                    if (IQBuffer != null)
                        IQBuffer.Dispose();
                    if (IBufferDecimation != null)
                        IBufferDecimation.Dispose();
                    if (QBufferDecimation != null)
                        QBufferDecimation.Dispose();

                    //Adjust Buffer size depending on downsampled or not.
                    DownSampleFactor = SampleRateIndex;
                    DownconversionEnabledTS = DownconversionEnabled;

                    if (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_Zero)
                    {
                        if (DownSampleTable[DownSampleFactor] != 0)
                            NumberOfSamples = NoUsbSamplesReturned / DownSampleTable[DownSampleFactor];
                        else
                            NumberOfSamples = NoUsbSamplesReturned;
                    }
                    else
                    {
                        if ((((SampleRateIndex == 15) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_1_536) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_2_048)) ||
                            ((SampleRateIndex == 8) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_200) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450)) ||
                            ((SampleRateIndex == 8) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_300) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450))) && DownconversionEnabledTS)
                        {
                            Decimation = 4;
                            IFTypeDecimation = IFFrequency;
                            NumberOfSamples = (NoUsbSamplesReturned >> 2);
                            IBufferDecimation = UnsafeBuffer.Create(NumberOfSamples, sizeof(short));
                            QBufferDecimation = UnsafeBuffer.Create(NumberOfSamples, sizeof(short));
                            IDecimation = (short*)IBufferDecimation;
                            QDecimation = (short*)QBufferDecimation;
                        }
                        if (((SampleRateIndex == 8) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_600) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450)) && DownconversionEnabledTS)
                        {
                            Decimation = 2;
                            IFTypeDecimation = IFFrequency;
                            NumberOfSamples = (NoUsbSamplesReturned >> 1);
                            IBufferDecimation = UnsafeBuffer.Create(NumberOfSamples, sizeof(short));
                            QBufferDecimation = UnsafeBuffer.Create(NumberOfSamples, sizeof(short));
                            IDecimation = (short*)IBufferDecimation;
                            QDecimation = (short*)QBufferDecimation;
                        }
                    }

                    // Allocate Buffers
                    IBuffer = UnsafeBuffer.Create(NoUsbSamplesReturned, sizeof(short));
                    QBuffer = UnsafeBuffer.Create(NoUsbSamplesReturned, sizeof(short));
                    IQBuffer = UnsafeBuffer.Create(NumberOfSamples, sizeof(Complex));
                }
                else
                {
                    MessageBox.Show("mir_sdr_Init error code: " + res);
                    Stop();
                    return;
                }           
                _ReaderThread = new Thread(() => ReaderThread(DownSampleFactor, DownconversionEnabledTS));  //, DecimationThreadsafe, DownconversionEnabledTS)
                _ReaderThread.Name = "SDRplay Rx Thread";
                _ReaderThread.Priority = ThreadPriority.Highest;
                _ReaderThread.Start();
        }

        public long Frequency
        {
            get
            {
                return SDRplayFrequency;
            }

            set
            {
                 SDRplayFrequency = value;
                 CurrentFreqBand = 0;
                 while (NominalFrequency > band_fmax[CurrentFreqBand] && CurrentFreqBand < NUM_BANDS)
                     ++CurrentFreqBand;

                 if (CurrentFreqBand == LastFreqBand && Running)
                     NativeMethods.mir_sdr_SetRf(NominalFrequency * 1.0E6, 1, 0);

                 if ((CurrentFreqBand != LastFreqBand) && Running)
                 {
                    Stop();
                    if (NominalFrequency < 60)
                    {
                        NativeMethods.mir_sdr_SetParam(101, LO120MHz);
                        Trace.WriteLine("120MHz LO Selected");
                    }
                    if (NominalFrequency >= 250 && NominalFrequency < 375)
                    {
                        NativeMethods.mir_sdr_SetParam(101, LO120MHz);
                        Trace.WriteLine("120MHz LO Selected");
                    }
                    if (NominalFrequency >= 375 && NominalFrequency < 395)
                    {
                        NativeMethods.mir_sdr_SetParam(101, LO144MHz);
                        Trace.WriteLine("144MHz LO Selected");
                    }
                    if (NominalFrequency >= 395 && NominalFrequency < 420)
                    {
                        NativeMethods.mir_sdr_SetParam(101, LO168MHz);
                        Trace.WriteLine("168MHz LO Selected");
                    }
                    SDRplay_Initalisation();
                }
                LastFreqBand = CurrentFreqBand;
            }
        }

        public double NominalFrequency
        {
            get
            {
                return (Frequency * (1.0E6 + FQPPM) * 1.0E-6) / 1.0e6;
            }
        }

        public double FQPPM
        {
            get
            {
                return SDRplayFQPPM;
            }
            set
            {
                SDRplayFQPPM = value;
                if (Running)
                {
                    NativeMethods.mir_sdr_SetRf((NominalFrequency * 1e6), 1, 0);
                }
            }
        }

        public double SampleRate
        {
            get
            {
                double SR;

                if (DownSampleTable[SampleRateIndex] == 0)
                {
                    SR = SampleRateTable[SampleRateIndex];

                    if ((((SampleRateIndex == 15) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_1_536) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_2_048)) ||
                        ((SampleRateIndex == 8) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_200) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450)) ||
                        ((SampleRateIndex == 8) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_300) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450))) && DownconversionEnabled)
                    {
                        SR = SR / 4;
                        Trace.WriteLine("SampleRate Rate /4 called");
                    }
                    if (((SampleRateIndex == 8) && (Bandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_600) && (IFFrequency == SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450)) && DownconversionEnabled)
                    {
                        SR = SR / 2;
                        Trace.WriteLine("SampleRate Rate /2 called");
                    }
                }
                else
                    SR = SampleRateTable[SampleRateIndex] / DownSampleTable[SampleRateIndex];
                return SR;
            }
        }

        public int SampleRateIndex
        {
            get
            {
                return SDRplaySampleRateIndex;
            }
            set
            {
                Trace.WriteLine("SDRplay SampleRate Called");
                SDRplaySampleRateIndex = value;
            }
        }

        public SDRplayIO.mir_sdr_Bw_MHzT Bandwidth
        {
            get
            {
                return SDRplayIFBandwidth;
            }
            set
            {
                Trace.WriteLine("SDRplay Bandwidth Called");
                SDRplayIFBandwidth = value;
                if (IFType == 0)
                {
                    IFFrequency = SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_Zero;
                }
                else
                {
                    if (SDRplayIFBandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_200 ||
                        SDRplayIFBandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_300 ||
                        SDRplayIFBandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_600)
                    {
                        IFFrequency = SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450;
                    }
                    if (SDRplayIFBandwidth == SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_1_536)
                        IFFrequency = SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_2_048;
                }
            }
        }

        private SDRplayIO.mir_sdr_If_kHzT IFFrequency
        {
            get
            {
                return SDRplayIFType;
            }
            set
            {
                SDRplayIFType = value;
            }
        }
        
        public int IFType
        {
            get
            {
                return SDRplayIFMode;
            }
            set
            {
                Trace.WriteLine("SDRplay IF Frequency Called");
                SDRplayIFMode = value;
                if(SDRplayIFMode == 0)
                {
                    DownconversionEnabled = false;
                }
                if(SDRplayIFMode == 1)
                {
                    DownconversionEnabled = true;
                }
            }
        }

        public int GainReduction
        {
            get
            {
                return SDRplayGainReduction;
            }
            set
            {
                SDRplayGainReduction = value;
                if (Running && AGCEnabled == false)
                {
                    int elapsed = 0;
                    while ((LastGrToggle == grToggle) && (elapsed < 100))
                    {
                        Thread.Sleep(10);
                        elapsed++;
                    }
                    NativeMethods.mir_sdr_SetGr(SDRplayGainReduction, 1, 0);
                    LastGrToggle = grToggle;
                }
            }
        }

        public int Setpoint
        {
            get
            {
                return SDRplaySetpoint;
            }
            set
            {
                SDRplaySetpoint = value;
                if (Running)
                {
                    _SET_AGC_Update = true;
                }
            }
        }

        public bool AGCEnabled
        {
            get
            {
                return SDRplayAGCEnabled;
            }
            set
            {
                SDRplayAGCEnabled = value;
            }
        }

        public int LNAGRThresh
        {
            get
            {
                return SDRplayLNAGRThresh;
            }
            set
            {
                SDRplayLNAGRThresh = value;
                if (Running)
                {
                    NativeMethods.mir_sdr_SetGrParams(0, SDRplayLNAGRThresh);
                    NativeMethods.mir_sdr_SetGr(GainReduction, 1, 0);
                }
            }
        }

        public void Stop()
        {
            if (Running != true)
            {
                return;
            }
            else
            {
                Running = false;
                Thread.Sleep(200);
                _ReaderThread.Join();
                _ReaderThread = null;
                NativeMethods.mir_sdr_Uninit();
            }
        }

        public event SamplesAvailableDelegate SamplesAvailable;

        private unsafe void ComplexSamplesAvailable(Complex* buffer, int length)
        {
            if (SamplesAvailable != null)
            {
                _eventArgs.Buffer = buffer;
                _eventArgs.Length = length;
                SamplesAvailable(this, _eventArgs);
            }
        }

        public event EventHandler GainReductionUpdate;

        public void OnGainReductionUpdate()
        {
            if (GainReductionUpdate != null)
            {
                GainReductionUpdate(this, EventArgs.Empty);
            }
        }

        public event EventHandler SampleRateChanged;

        public void OnSampleRateChanged()
        {
            if (SampleRateChanged != null)
                SampleRateChanged(this, EventArgs.Empty);
        }

        ~SDRplayDevice()
        {
            Dispose();
        }

        public void Dispose()
        {
            Stop();
            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }
            GC.SuppressFinalize(this);
        }

        unsafe private void ReaderThread(int DownSampleFactor, bool DownconversionEnabledTS)
        {
            int i = 0;
            float ONE_OVER_32768 = 0.000030517578125f;
            short* I = (short*)IBuffer;
            short* Q = (short*)QBuffer;
            Complex* IQ = (Complex*)IQBuffer;

            double Power = 0.0;
            double PowerAve = 0.0;
            int PowerAveCount = 0;
            double gRdB = 0.0;
            double setpoint;
            double sp_p2;
            double sp_m2;
            float dcoffi;
            float dcoffq;
            int grChanged = 0;

            setpoint = (Math.Pow(10, (double)(Setpoint / 10.0)));
            sp_p2 = (Math.Pow(10, (double)((Setpoint + 2) / 10.0)));
            sp_m2 = (Math.Pow(10, (double)((Setpoint - 2) / 10.0)));

            Trace.WriteLine("Thread Procedure STarted");
            LastGrToggle = !grToggle;
            while (Running)
            {
                uint FirstSample;
                
                int rfChanged;
                int fsChanged;
                SDRplayIO.mir_sdr_ErrT res = SDRplayIO.mir_sdr_ErrT.mir_sdr_Success;

                if (_SET_AGC_Update)
                {
                    lock (this)
                    {
                        setpoint = (Math.Pow(10, (double)(Setpoint / 10.0)));
                        sp_p2 = (Math.Pow(10, (double)((Setpoint + 2) / 10.0)));
                        sp_m2 = (Math.Pow(10, (double)((Setpoint - 2) / 10.0)));
                        _SET_AGC_Update = false;
                    }
                }

                lock (this)
                {
                   res =  NativeMethods.mir_sdr_ReadPacket(I, Q, out FirstSample, out grChanged, out rfChanged, out fsChanged);
                }

                if (res != SDRplayIO.mir_sdr_ErrT.mir_sdr_Success)
                    Trace.WriteLine("Failed to readpackets " + res);

                if (grChanged == 1)
                    grToggle = !grToggle;

                if (DownconversionEnabledTS == true)
                {
                    res = NativeMethods.mir_sdr_DownConvert(I, IDecimation, QDecimation, (uint)NoUsbSamplesReturned, IFTypeDecimation, Decimation, 0);  //IFType threadsafe??
                    if (res != SDRplayIO.mir_sdr_ErrT.mir_sdr_Success)
                    {
                        Trace.WriteLine("Downconvert failed");
                    }
                    for (i = 0; i < NumberOfSamples; i++)
                    {
                        IQ[i].Real = IDecimation[i] * ONE_OVER_32768;
                        IQ[i].Imag = QDecimation[i] * ONE_OVER_32768;
                    }
                }
                else
                {
                    if (DownSampleTable[DownSampleFactor] == 0)
                    {
                        for (i = 0; i < NumberOfSamples; i++)
                        {
                            IQ[i].Real = I[i] * ONE_OVER_32768;
                            IQ[i].Imag = Q[i] * ONE_OVER_32768;
                        }
                    }
                    else
                    {
                        int j = 0;
                        for (i = 0; i < NoUsbSamplesReturned; i++)
                        {
                            IQ[j].Real = I[i] * ONE_OVER_32768;
                            IQ[j].Imag = Q[i] * ONE_OVER_32768;
                            j++;
                            i = i + (DownSampleTable[DownSampleFactor] - 1);
                        }
                    }
                }
                ComplexSamplesAvailable(IQ, NumberOfSamples);
                    
                if (AGCEnabled == true)
                {
                    dcoffi = 0;
                    dcoffq = 0;
                    for (i = 0; i < NumberOfSamples; i++)
                    {
                        dcoffi += IQ[i].Real;
                        dcoffq += IQ[i].Imag;
                    }
                    dcoffi /= NumberOfSamples;
                    dcoffq /= NumberOfSamples;
                    for (i = 0; i < NumberOfSamples; i++)
                    {
                        IQ[i].Real -= dcoffi;
                        IQ[i].Imag -= dcoffq;
                    }
                
                    if (grChanged == 0)
                    {
                        for (i = 0; i < NumberOfSamples; i++)
                        {
                            Power += (double)(IQ[i].Real * IQ[i].Real) + (double)(IQ[i].Imag * IQ[i].Imag);
                        }
                        Power /= NumberOfSamples;
                        PowerAve = PowerAve += Power;
                        PowerAveCount++;
                        if (PowerAveCount == 2000)
                        {
                            PowerAve = PowerAve / 2000;
                            if ((PowerAve > sp_p2) || (PowerAve < sp_m2))
                            {
                                gRdB = 10 * Math.Log10(PowerAve / setpoint);
                                gRdB += (gRdB >= 0.0) ? 0.5 : -0.5;
                                NativeMethods.mir_sdr_SetGr((int)gRdB, 0, 0);
                                GainReduction = GainReduction + (int)gRdB;
                                OnGainReductionUpdate();
                            }
                            PowerAveCount = 0;
                            PowerAve = 0.0;
                        }
                    }
                }
            }
            if (Running == false)
            {
                Trace.WriteLine("NO LONGER RUNNING");
            }
        }
    }

    public delegate void SamplesAvailableDelegate(object sender, SamplesAvailableEventArgs e);

    public unsafe sealed class SamplesAvailableEventArgs : EventArgs
    {
        public int Length { get; set; }
        public Complex* Buffer { get; set; }
    }
}
