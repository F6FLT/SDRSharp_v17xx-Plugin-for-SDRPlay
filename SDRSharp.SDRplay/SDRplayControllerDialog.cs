using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace SDRSharp.SDRplay
{
    public partial class SDRplayControllerDialog : Form
    {
        private readonly SDRplayIO _Parent;
        int LNATHRESMIN = 24;

        private List<SDRplayIO.mir_sdr_Bw_MHzT> BandWidths = new List<SDRplayIO.mir_sdr_Bw_MHzT>()
        {
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_200,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_300,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_0_600,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_1_536,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_5_000,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_6_000,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_7_000,
            SDRplayIO.mir_sdr_Bw_MHzT.mir_sdr_BW_8_000
        };

        private List<SDRplayIO.mir_sdr_If_kHzT> IFTypes = new List<SDRplayIO.mir_sdr_If_kHzT>()
        {
            SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_Zero,
            SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_0_450,
            SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_1_620,
            SDRplayIO.mir_sdr_If_kHzT.mir_sdr_IF_2_048
        };

        private List<int> MinSampleRatesIndex = new List<int>()
        {
            0,
            1,
            4,
            8,
            11,
            12,
            13,
            14
        };

        private List<float> SuppotedSampleRates = new List<float>()
        {
            0.2f,
            0.3f,
            0.4f,
            0.5f,
            0.6f,
            0.75f,
            0.8f,
            1.0f,
            2.0f,
            3.0f,
            4.0f,
            5.0f,
            6.0f,
            7.0f,
            8.0f,
            8.2f
        };

        public SDRplayControllerDialog(SDRplayIO parent)
        {
            InitializeComponent();
            _Parent = parent;
        }
        
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                cboIFType.SelectedIndex = _Parent.IFType;
                cboBandwidth.SelectedIndex = BandWidths.IndexOf(_Parent.Bandwidth);
                numGR.Value = _Parent.GainReduction;
                FQCorrection.Value = (Decimal)_Parent.FQPPM;              
                LNAGRSlider.Value = 59 - (_Parent.LNAGRThresh - LNATHRESMIN);
                ADCsetpoint.Value = _Parent.AGCsetpoint;
                AGCEnabled.Checked = _Parent.AGCEnabled;
                AGCENState();
            }
        }

        private void SDRplayControlDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SampleRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            float SR;

            SR = (float)Convert.ToDouble(SampleRate.Text);
            for (int i = 0; i < SuppotedSampleRates.Count; i++)
            {
                if (SR == SuppotedSampleRates[i])
                {
                    _Parent.InternalSampleRate = i;
                }
            }
        }

        private void cboBandwidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Zero IF Mode
            _Parent.Bandwidth = BandWidths[cboBandwidth.SelectedIndex];
            if (cboIFType.SelectedIndex == 0)
            {
                SampleRate.Enabled = true;
                SampleRate.Items.Clear();
                for (int i = MinSampleRatesIndex[cboBandwidth.SelectedIndex]; i < SuppotedSampleRates.Count - 1; i++)
                {
                    SampleRate.Items.Add((SuppotedSampleRates[i].ToString("0.00")));
                }
                SampleRate.SelectedIndex = 0;
            }
            //Low IF Mode
            if (cboIFType.SelectedIndex == 1)
            {
                if (cboBandwidth.SelectedIndex == 3)
                {
                    SampleRate.Items.Clear();
                    SampleRate.Items.Add((SuppotedSampleRates[15].ToString("0.00")));
                    SampleRate.SelectedIndex = 0;
                    SampleRate.Enabled = false;
                }
                else
                {
                    SampleRate.Items.Clear();
                    SampleRate.Items.Add((SuppotedSampleRates[8].ToString("0.00")));
                    SampleRate.SelectedIndex = 0;
                    SampleRate.Enabled = false;
                }
            }
        }

        private void cboIFType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _Parent.IFType = cboIFType.SelectedIndex;
            if (cboIFType.SelectedIndex == 0)
            {
                cboBandwidth.Items.Clear();
                cboBandwidth.Items.Add("0.200 MHz");
                cboBandwidth.Items.Add("0.300 MHz");
                cboBandwidth.Items.Add("0.600 MHz");
                cboBandwidth.Items.Add("1.536 MHz");
                cboBandwidth.Items.Add("5.000 MHz");
                cboBandwidth.Items.Add("6.000 MHz");
                cboBandwidth.Items.Add("7.000 MHz");
                cboBandwidth.Items.Add("8.000 MHz");
                cboBandwidth.SelectedIndex = 0;
                _Parent.Bandwidth = BandWidths[cboBandwidth.SelectedIndex];
                SampleRate.Enabled = true;
                SampleRate.Items.Clear();
                for (int i = MinSampleRatesIndex[cboBandwidth.SelectedIndex]; i < SuppotedSampleRates.Count - 1; i++)
                {
                    SampleRate.Items.Add((SuppotedSampleRates[i].ToString("0.00")));
                }
                SampleRate.SelectedIndex = 0;
            }

            if (cboIFType.SelectedIndex == 1)
            {
                cboBandwidth.Items.Clear();
                cboBandwidth.Items.Add("0.200 MHz");
                cboBandwidth.Items.Add("0.300 MHz");
                cboBandwidth.Items.Add("0.600 MHz");
                cboBandwidth.Items.Add("1.536 MHz");
                cboBandwidth.SelectedIndex = 0;
                _Parent.Bandwidth = BandWidths[cboBandwidth.SelectedIndex];
                SampleRate.Items.Clear();
                SampleRate.Items.Add((SuppotedSampleRates[8].ToString("0.00")));
                SampleRate.SelectedIndex = 0;
                SampleRate.Enabled = false;
            }
            
        }

        private void numSampleRate_ValueChanged(object sender, EventArgs e)
        {

        }

        public void SR_IF_BW_OFF()
        {
            cboBandwidth.Enabled = false;
            cboIFType.Enabled = false;
        }

        public void SR_IF_BW_ON()
        {
            cboBandwidth.Enabled = true;
            cboIFType.Enabled = true;
        }

        private void numGR_ValueChanged(object sender, EventArgs e)
        {
            _Parent.GainReduction = (int)numGR.Value;
            numGRSlider.Value = (int)numGR.Maximum - (int)numGR.Value;
            Trace.WriteLine("Slider Value is " + numGRSlider.Value);
            Trace.WriteLine("Gain Reduction Value is " + numGR.Value);
        }

        private void numGRSlider_ValueChanged(object sender, EventArgs e)
        {
            numGR.Value = (int)numGR.Maximum - numGRSlider.Value;
            Trace.WriteLine("Gain Reduction Value is " + numGR.Value);
            Trace.WriteLine("Slider Value is " + numGRSlider.Value);
        }

        private void LNAGRSlider_Scroll(object sender, EventArgs e)
        {
            int LNAValue;

            LNAGRSlider.Minimum = _Parent.LNAGRThreshMin;
            LNAGRSlider.Maximum = 59;
            LNAValue = (59 - LNAGRSlider.Value) + _Parent.LNAGRThreshMin;
            LNAGRThreshTB.Text = LNAValue.ToString();
            _Parent.LNAGRThresh = LNAValue;
        }

        public void LNAGRUpdate(int LNATHRES, int _LNAGRThreshMin)
        {
            int LNAValue;

            LNAGRSlider.Minimum = _LNAGRThreshMin;
            LNAGRSlider.Maximum = 59;
            LNAValue = 59 - (LNATHRES - _LNAGRThreshMin);
            LNAGRSlider.Value = LNAValue;
            LNAGRThreshTB.Text = LNATHRES.ToString();
        }

        private void AGCEnabled_CheckedChanged(object sender, EventArgs e)
        {
            _Parent.AGCEnabled = AGCEnabled.Checked;
            AGCENState();
        }

        private void AGCENState()
        {
            if (AGCEnabled.Checked == true)
            {
                numGR.Enabled = false;
                numGRSlider.Enabled = false;
            }
            if (AGCEnabled.Checked == false)
            {
                numGR.Enabled = true;
                numGRSlider.Enabled = true;
            }
        }

        private void ADCsetpoint_ValueChanged(object sender, EventArgs e)
        {
            _Parent.AGCsetpoint = (int)ADCsetpoint.Value;
        }

        private void DefaultBtn_Click(object sender, EventArgs e)
        {
            cboIFType.SelectedIndex = 0;
            cboBandwidth.SelectedIndex = 3;
            ADCsetpoint.Value = -30;
            AGCEnabled.Checked = true;
            LNAState.Text = "LNA Off";
            LNAGRState.Text = "24";
            MixerStatus.Text = "Mixer On";
            BBGRState.Text = "36";
            numGR.Value = 60;
        }

        public void DefaultBtn_on()
        {
            DefaultBtn.Enabled = true;
        }

        public void DefaultBtn_off()
        {
            DefaultBtn.Enabled = false;
        }

        public void Defaults()
        {
            cboIFType.SelectedIndex = 0;
            cboBandwidth.SelectedIndex = 3;
            ADCsetpoint.Value = -15;
            AGCEnabled.Checked = true;
            LNAState.Text = "LNA Off";
            LNAGRState.Text = "24";
            MixerStatus.Text = "Mixer On";
            BBGRState.Text = "36";
            numGR.Value = 60;
        }
        
        public void InitaliseDisplayValues()
        {
            cboIFType.SelectedIndex = _Parent.IFType;
            if (cboIFType.SelectedIndex == 0)
            {
                cboBandwidth.Items.Clear();
                cboBandwidth.Items.Add("0.200 MHz");
                cboBandwidth.Items.Add("0.300 MHz");
                cboBandwidth.Items.Add("0.600 MHz");
                cboBandwidth.Items.Add("1.536 MHz");
                cboBandwidth.Items.Add("5.000 MHz");
                cboBandwidth.Items.Add("6.000 MHz");
                cboBandwidth.Items.Add("7.000 MHz");
                cboBandwidth.Items.Add("8.000 MHz");
                for (int i = 0; i < BandWidths.Count; i++)
                {
                    if (BandWidths[i] == _Parent.Bandwidth)
                    {                       
                        cboBandwidth.SelectedIndex = i;
                    }
                }
                SampleRate.Enabled = true;
                SampleRate.Items.Clear();
                for (int i = MinSampleRatesIndex[cboBandwidth.SelectedIndex]; i < SuppotedSampleRates.Count - 1; i++)
                {
                    SampleRate.Items.Add((SuppotedSampleRates[i].ToString("0.00")));
                }
                SampleRate.SelectedIndex = _Parent.InternalSampleRate - MinSampleRatesIndex[cboBandwidth.SelectedIndex];
            }

            if (cboIFType.SelectedIndex == 1)
            {
                cboBandwidth.Items.Clear();
                cboBandwidth.Items.Add("0.200 MHz");
                cboBandwidth.Items.Add("0.300 MHz");
                cboBandwidth.Items.Add("0.600 MHz");
                cboBandwidth.Items.Add("1.536 MHz");
                for (int i = 0; i < BandWidths.Count; i++)
                {
                    if (BandWidths[i] == _Parent.Bandwidth)
                    {
                        cboBandwidth.SelectedIndex = i;
                    }
                }
                if (cboBandwidth.SelectedIndex == 3)
                {
                    SampleRate.Items.Clear();
                    SampleRate.Items.Add((SuppotedSampleRates[15].ToString("0.00")));
                    SampleRate.SelectedIndex = 0;
                    SampleRate.Enabled = false;
                }
                else
                {
                    SampleRate.Items.Clear();
                    SampleRate.Items.Add((SuppotedSampleRates[8].ToString("0.00")));
                    SampleRate.SelectedIndex = 0;
                    SampleRate.Enabled = false;
                }
            }
        }
                
        public void GRValues(long Frequency)
        {
            if (Frequency < 60000000)
            {
                numGR.Maximum = 102;
                numGRSlider.Maximum = 102;
                _Parent.LNAGRThreshMin = 24;
            }

            if (Frequency >= 60000000 && Frequency < 249999999)
            {
                numGR.Maximum = 102;
                numGRSlider.Maximum = 102;
                _Parent.LNAGRThreshMin = 24;
            }

            if (Frequency >= 250000000 && Frequency < 419999999)
            {
                numGR.Maximum = 102;
                numGRSlider.Maximum = 102;
                _Parent.LNAGRThreshMin = 24;
            }

            if (Frequency >= 420000000 && Frequency < 1000000000)
            {
                numGR.Maximum = 85;
                numGRSlider.Maximum = 85;
                _Parent.LNAGRThreshMin = 7;
            }

            if (Frequency >= 1000000000)
            {
                numGR.Maximum = 85;
                numGRSlider.Maximum = 85;
                _Parent.LNAGRThreshMin = 5;
            }

            LNAGRUpdate(_Parent.LNAGRThresh, _Parent.LNAGRThreshMin);
            numGR.Value = _Parent.GainReduction;
            numGRSlider.Value = (int)numGR.Maximum - (int)numGR.Value;
        }

        public void GRboxUpdate(int GRUpdate, long Frequency)
        {
            int BBGR;

            if (GRUpdate > numGR.Maximum)
            {
                GRUpdate = (int)numGR.Maximum;
            }
            if (GRUpdate < 0)
            {
                GRUpdate = 0;
            }
            numGR.Value = GRUpdate;

            if (Frequency < 419999999)
            {
                if (GRUpdate < _Parent.LNAGRThresh)
                {
                    BBGRState.Text = GRUpdate.ToString();
                    LNAState.Text = "LNA On";
                    LNAGRState.Text = "0";
                    MXRGRState.Text = "0";
                    MixerStatus.Text = "Mixer On";
                }

                if (GRUpdate >= 83)
                {
                    BBGR = GRUpdate - 24 - 19;
                    BBGRState.Text = BBGR.ToString();
                    LNAState.Text = "LNA Off";
                    LNAGRState.Text = "24";
                    MXRGRState.Text = "19";
                    MixerStatus.Text = "Mixer Off";
                }

                if (GRUpdate >= (_Parent.LNAGRThresh) && GRUpdate < 83)
                {
                    BBGR = GRUpdate - 24;
                    BBGRState.Text = BBGR.ToString();
                    LNAState.Text = "LNA Off";
                    LNAGRState.Text = "24";
                    MXRGRState.Text = "0";
                    MixerStatus.Text = "Mixer On";
                }
            }
            if (Frequency >= 420000000 && Frequency < 1000000000)
            {
                if (GRUpdate < _Parent.LNAGRThresh)
                {
                    BBGRState.Text = GRUpdate.ToString();
                    LNAState.Text = "LNA On";
                    LNAGRState.Text = "0";
                    MXRGRState.Text = "0";
                    MixerStatus.Text = "Mixer On";
                }

                if (GRUpdate >= 66)
                {
                    BBGR = GRUpdate - 7 - 19;
                    BBGRState.Text = BBGR.ToString();
                    LNAState.Text = "LNA Off";
                    LNAGRState.Text = "7";
                    MXRGRState.Text = "19";
                    MixerStatus.Text = "Mixer Off";
                }

                if (GRUpdate >= (_Parent.LNAGRThresh) && GRUpdate < 66)
                {
                    BBGR = GRUpdate - 7;
                    BBGRState.Text = BBGR.ToString();
                    LNAState.Text = "LNA Off";
                    LNAGRState.Text = "7";
                    MXRGRState.Text = "0";
                    MixerStatus.Text = "Mixer On";
                }
            }
            if (Frequency >= 1000000000)
            {
                if (GRUpdate < _Parent.LNAGRThresh)
                {
                    BBGRState.Text = GRUpdate.ToString();
                    LNAState.Text = "LNA On";
                    LNAGRState.Text = "0";
                    MXRGRState.Text = "0";
                    MixerStatus.Text = "Mixer On";
                }

                if (GRUpdate >= 64)
                {
                    BBGR = GRUpdate - 5 - 19;
                    BBGRState.Text = BBGR.ToString();
                    LNAState.Text = "LNA Off";
                    LNAGRState.Text = "5";
                    MXRGRState.Text = "19";
                    MixerStatus.Text = "Mixer Off";
                }

                if (GRUpdate >= (_Parent.LNAGRThresh) && GRUpdate < 64)
                {
                    BBGR = GRUpdate - 5;
                    BBGRState.Text = BBGR.ToString();
                    LNAState.Text = "LNA Off";
                    LNAGRState.Text = "5";
                    MXRGRState.Text = "0";
                    MixerStatus.Text = "Mixer On";
                }
            }
        }

        public int GRboxRead()
        {
            int GRValue;

            GRValue = (int)numGR.Value;
            return GRValue;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FQCorrection_ValueChanged(object sender, EventArgs e)
        {
            _Parent.FQPPM = (double)FQCorrection.Value;
        }
    }
}