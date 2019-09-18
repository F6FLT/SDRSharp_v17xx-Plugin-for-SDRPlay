using System.Runtime.InteropServices;

namespace SDRSharp.SDRplay
{
    public class NativeMethods
    {
        const string APIDLL = "mir_sdr_api.dll";

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_Init(int gRdB, double fsMHz, double rfMHz, SDRplayIO.mir_sdr_Bw_MHzT bwType, SDRplayIO.mir_sdr_If_kHzT ifType, out int samplesPerPacket);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_Uninit();

        [DllImport(APIDLL)]
        public unsafe static extern SDRplayIO.mir_sdr_ErrT mir_sdr_ReadPacket(short* xi, short* xq, out uint firstSampleNum, out int grChanged, out int rfChanged, out int fsChanged);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetRf(double drfHz, int abs, int syncUpdate);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetFs(double dfsHz, int abs, int syncUpdate, int reCal);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetGr(int gRdB, int abs, int syncUpdate);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetGrParams(int minimumGr, int lnaGrThreshold);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetDcMode(int dcCal, int speedUp);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetDcTrackTime(int trackTime);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetSyncUpdateSampleNum(uint sampleNum);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetSyncUpdatePeriod(uint period);

        [DllImport(APIDLL)]
        public unsafe static extern SDRplayIO.mir_sdr_ErrT mir_sdr_DownConvert(short* xin, short* xi, short* xq,  uint SamplesPerPacket, SDRplayIO.mir_sdr_If_kHzT ifType, uint Decimation, uint Preset);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_SetParam(int ParameterId, int Value);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_ResetUpdateFlags(int ResetGainUpdate, int ResetRFUpdate, int ResetFsUpdate);

        [DllImport(APIDLL)]
        public static extern SDRplayIO.mir_sdr_ErrT mir_sdr_ApiVersion(out float version);    // Called by application to retrieve version of API used to create Dll
    }
}