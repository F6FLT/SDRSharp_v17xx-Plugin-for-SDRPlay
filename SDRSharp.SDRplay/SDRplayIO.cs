using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using SDRSharp.Radio;

namespace SDRSharp.SDRplay
{
   //public class LimeSDRIO : IFrontendController, IIQStreamController, IDisposable, IFloatingConfigDialogProvider, ITunableSource, ISampleRateChangeSource
//old   public class SDRplayIO : IFrontendController, ITunableSource, IIQStreamController, IDisposable, IFloatingConfigDialogProvider, ISampleRateChangeSource
   public class SDRplayIO : IFrontendController, IIQStreamController, IDisposable, IFloatingConfigDialogProvider, ITunableSource, ISampleRateChangeSource
    {
        public enum mir_sdr_ErrT : int
        {
            mir_sdr_Success = 0,
            mir_sdr_Fail = 1,
            mir_sdr_InvalidParam = 2,
            mir_sdr_OutOfRange = 3,
            mir_sdr_GainUpdateError = 4,
            mir_sdr_RfUpdateError = 5,
            mir_sdr_FsUpdateError = 6,
            mir_sdr_HwError = 7,
            mir_sdr_AliasingError = 8,
            mir_sdr_NotInitialised
        }

        public enum mir_sdr_Bw_MHzT : int
        {
            mir_sdr_BW_0_200 = 200,
            mir_sdr_BW_0_300 = 300,
            mir_sdr_BW_0_600 = 600,
            mir_sdr_BW_1_536 = 1536,
            mir_sdr_BW_5_000 = 5000,
            mir_sdr_BW_6_000 = 6000,
            mir_sdr_BW_7_000 = 7000,
            mir_sdr_BW_8_000 = 8000
        }

        public enum mir_sdr_If_kHzT : int
        {
            mir_sdr_IF_Zero = 0,
            mir_sdr_IF_0_450 = 450,
            mir_sdr_IF_1_620 = 1620,
            mir_sdr_IF_2_048 = 2048
        }

        private const string _displayName = "SDRplay RSP";
        private  SDRplayControllerDialog _gui;
        private SDRplayDevice SDRplayDevice;

        // SDRplay Variables
        private long _frequency;
        private int _GainReduction;
        private double _FqOffsetPPM;
        private int _SP;
        private bool _AGCEnabled = true;
        private int _LNAGRThresh;
        private int _LNAGRThreshMin;
        private mir_sdr_Bw_MHzT _IFBandwidth;
        private int _IFType;
        private int SampleRateIndex;

        //Event Handlers
        private SDRSharp.Radio.SamplesAvailableDelegate _callback;
        public event EventHandler SampleRateChanged;

        public SDRplayIO()
        {
            RegistryKey ApiPathkey = null;
            string APIPath, UpdatedPaths, ExistingPaths;

            ApiPathkey = Registry.LocalMachine.OpenSubKey("Software\\MiricsSDR\\API", false);
            if (ApiPathkey == null)
            {
                MessageBox.Show("Cannot Locate API");
            }
            else
            {
                APIPath = (string)ApiPathkey.GetValue("Install_Dir", 0);
                APIPath = APIPath + "\\x86";
                ExistingPaths = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                UpdatedPaths = ExistingPaths + Path.PathSeparator.ToString() + APIPath;
                Environment.SetEnvironmentVariable("PATH", UpdatedPaths);
                ApiPathkey.Close();
            }
            _gui = new SDRplayControllerDialog(this);
            SDRplayDevice = new SDRplayDevice();

            IFType = (int)Utils.GetIntSetting("SDRplay.IFType", 0);
            Bandwidth = (SDRplayIO.mir_sdr_Bw_MHzT)Utils.GetIntSetting("SDRplay.Bandwidth", (int)SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_1_536);
            InternalSampleRate = (int)Utils.GetIntSetting("SDRplay.SampleRate", 8);
            GainReduction = Utils.GetIntSetting("SDRplay.GainReduction", 60);
            LNAGRThresh = Utils.GetIntSetting("SDRplay.LNAGRThresh", 59);        
            AGCsetpoint = Utils.GetIntSetting("SDRplay.AGCsetpoint", -30);
            FQPPM = Utils.GetDoubleSetting("SDRplay.PPM", 0);
            int AGCINT= Utils.GetIntSetting("SDRplay.AGCEnabled", 1);
            if (AGCINT == 1)
                AGCEnabled = true;
            if (AGCINT != 1)
                AGCEnabled = false;
            _gui.InitaliseDisplayValues();
            SDRplayDevice.SamplesAvailable += SDRplayDevice_SamplesAvailable;          
            SDRplayDevice.SampleRateChanged += SDRplayDevice_SampleRateChanged;         
            SDRplayDevice.GainReductionUpdate += SDRplayDevice_GainReductionUpdate;
        }

        ~SDRplayIO()
        {
            Dispose();
        }

        public void Dispose()
        {
            Close();
            if (_gui != null)
            {
                _gui.Close();
                _gui.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public SDRplayDevice Device
        {
            get
            {
                return SDRplayDevice;
            }
        }

        public void Open()
        {

        }

        public void Close()
        {

        }

        public void Start(Radio.SamplesAvailableDelegate callback)
        {
            _callback = callback;
            SDRplayDevice.SDRplay_Initalisation();
        }

        public void Stop()
        {
            SDRplayDevice.Stop();

        }

        public void ShowSettingGUI(IWin32Window parent)
        {
            if (this._gui.IsDisposed)
                return;
            _gui.Show();
            _gui.Activate();
        }

        public void HideSettingGUI()
        {
            if (_gui.IsDisposed)
                return;
            _gui.Hide();
        }

        public double Samplerate
        {
            get
            {
                return (SDRplayDevice.SampleRate * 1e3);
            }
        }

        internal int InternalSampleRate
        {
            get
            {
                return SampleRateIndex;
            }
            set
            {
                SampleRateIndex = value;
                SDRplayDevice.SampleRateIndex = value;
                Utils.SaveSetting("SDRplay.SampleRate", value);
                SDRplayDevice.OnSampleRateChanged();
            }
        }

        public long Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
                _gui.GRValues(value);
                SDRplayDevice.Frequency = value;
            }
        }

        public bool IsSoundCardBased
        {
            get
            {
                return false;
            }
        }

        public string SoundCardHint
        {
            get
            {
                return string.Empty;
            }
        }

        private void SDRplayDevice_SampleRateChanged(object sender, EventArgs e)
        {
            if (SampleRateChanged != null)
                SampleRateChanged(this, EventArgs.Empty);
        }

        private void SDRplayDevice_GainReductionUpdate(object sender, EventArgs e)
        {
            _gui.GRboxUpdate(SDRplayDevice.GainReduction, Frequency);
            _GainReduction = SDRplayDevice.GainReduction;
        }

        private unsafe void SDRplayDevice_SamplesAvailable(object sender, SamplesAvailableEventArgs e)
        {
            _callback(this, e.Buffer, e.Length);
        }

        public int GainReduction
        {
            get
            {
                return _GainReduction;
            }
            set
            {
                SDRplayDevice.GainReduction = value;
                _GainReduction = value;
                _gui.GRboxUpdate(_GainReduction, Frequency);
                Trace.WriteLine("Gain Reduction is" + _GainReduction);
                Utils.SaveSetting("SDRplay.GainReduction", value);
            }
        }

        public double FQPPM
        {
            get
            {
                return _FqOffsetPPM;
            }
            set
            {
                _FqOffsetPPM = value;
                SDRplayDevice.FQPPM = value;
                Utils.SaveSetting("SDRplay.PPM", (double)_FqOffsetPPM);
            }
        }

        public double NominalFrequency
        {
            get
            {
                return (Frequency * (1.0E6 + FQPPM) * 1.0E-6) / 1.0e6;
            }
        }

        public int AGCsetpoint
        {
            get
            {
                return _SP;
            }
            set
            {
                _SP = value;
                SDRplayDevice.Setpoint = value;
                Utils.SaveSetting("SDRplay.AGCsetpoint", value);
            }
        }

        public bool AGCEnabled
        {
            get
            {
                return _AGCEnabled;
            }
            set
            {
                _AGCEnabled = value;
                SDRplayDevice.AGCEnabled = value;
                if (_AGCEnabled == true)
                {
                    Utils.SaveSetting("SDRplay.AGCEnabled", 1);
                }
                if (_AGCEnabled == false)
                {
                    Utils.SaveSetting("SDRplay.AGCEnabled", 0);
                }
            }
        }

        public int LNAGRThreshMin
        {
            get
            {
                return _LNAGRThreshMin;
            }
            set
            {
                _LNAGRThreshMin = value;
                if (Frequency < 419999999 && LNAGRThresh < 24)
                {
                    _LNAGRThresh = 24;
                }

                if (Frequency >= 420000000 && Frequency < 1000000000 && LNAGRThresh < 7)
                {
                    _LNAGRThresh = 7;
                }

                if (Frequency >= 1000000000 && LNAGRThresh < 5)
                {
                    _LNAGRThresh = 5;
                }
            }
        }

        public int LNAGRThresh
        {
            get
            {
                return _LNAGRThresh;
            }
            set
            {
                _LNAGRThresh = value;
                _gui.GRValues(Frequency);
                SDRplayDevice.LNAGRThresh = value;
                _gui.GRboxUpdate(GainReduction, Frequency);            
                Utils.SaveSetting("SDRplay.LNAGRThresh", value);
            }
        }

        public mir_sdr_Bw_MHzT Bandwidth
        {
            get
            {
                return _IFBandwidth;
            }
            set
            {
                _IFBandwidth = value;
                SDRplayDevice.Bandwidth = value;
                Utils.SaveSetting("SDRplay.Bandwidth", (int)_IFBandwidth);
            }
        }

        public int IFType
        {
            get
            {
                return _IFType;
            }
            set
            {
                _IFType = value;
                SDRplayDevice.IFType = value;
                Utils.SaveSetting("SDRplay.IFType", (int)_IFType);
            }
        }

        public bool CanTune
        {
            get
            { return true; }
        }

        public long MaximumTunableFrequency
        {

            get { return 2000000000; }
        }

        public long MinimumTunableFrequency
        {
            get { return 100000; }
        }

    }
}
