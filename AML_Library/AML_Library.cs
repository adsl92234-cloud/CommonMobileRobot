using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace AML
{

    public enum WaferState { NoWafer, WaferMapped, WaferError, WaferProcessing, WaferProcessEnd, WaferReserved, WaferUnknown };
    public enum FrameState { NoWafer, WaferMapped, WaferTilt, WaferThicknessError, WaferProcessing, WaferProcessEnd, WaferReserved, WaferUnknown };
    public enum BoatState { NoBoat, BoatMapped, BoatError, BoatProcessing, BoatProcessEnd, BoatReserved, BoatUnknown };
    public enum LoadportState { OutOfServ, TransBlock, RdyToLoad, RdyToUnload };
    public enum E84AccessMode { Unknown, Auto, Manual };
    public enum LoadportE84AccessMode { Unknown, Auto, Manual };
    public enum Product { Frame, Wafer };
    #region AlarmID
    public enum ALID
    {
        //7碼 FluxClean "1" 開頭("10"開頭自定義；"12"開頭小台CDBC；"13"開頭大台CDCL)； Aligner "2" 開頭； 其他自定義 "3" 開頭
        
        //SECSAgent "1"開頭
        SA_Disconnect = 1000001,
        ChangeAccessModeFail = 1000002,
        ChangeAccessModeTimeout = 1000003,
        CheckBoatIDTimeout = 1000004,
        CheckFourPortRunBindLPFail = 1000005,
        ReceiveTAPSendRcmdStopForEarthquakeAlarm = 1000006,
        DieTypeMismatch = 1000007,

        //Core "2"開頭
        ChangeRecipeFail = 2000001,
        LoadportBusyTimeout = 2000002,
        Aligner1BusyTimeout = 2000003,
        Aligner2BusyTimeout = 2000004,
        Robot1BusyTimeout = 2000005,
        Robot2BusyTimeout = 2000006,
        RobotPutFail = 2000007,
        RobotGetFail = 2000008,
        RobotGetExtendFail = 2000009,
        RobotGetRetractFail = 2000010,
        RobotPutExtendFail = 2000011,
        RobotPutRetractFail = 2000012,
        RobotGetStandbyFail = 2000013,
        RobotPutStandbyFail = 2000014,
        CheckandChangeRecipeFail = 2000015,
        FrameBarcodeMismatch = 2000016,
        FramePositionError = 2000017,
        FrameFullPartError = 2000018,
        FramePart6KeyValueError = 2000019,
        WaitClearFrameTimeout = 2000020,
        FrameStageNotReadyToUnload = 2000021,
        FrameStageNotReadyToLoad = 2000022,
        NoFrameMatchWafer = 2000023,
        FindFrameFail = 2000024,
        StretcherPosError = 2000025,
        CheckFrameIDFail = 2000026,
        FrameInfoError = 2000027,
        TVOC_StatusError = 2000028,
        TVOC_Alarm = 2000029,
        DoLineScanFail = 2000030,
        FlipMonFail = 2000031,
        WaferNoProcessEnd = 2000032,
        SetRobotSpeedFail = 2000033,

        //EFEM "3"開頭
        FFU_Alarm = 3000001,
        Ionizer_Alarm = 3000002,

        //BESI "4"開頭
        WaitS2F38Timeout = 4000001,
        WaitS2F36Timeout = 4000002,
        WaitS2F34Timeout = 4000003,
        WaitFrameStageReadyToLoadTimeout = 4000004,
        WaitFrameStageReadyToUnloadTimeout = 4000005,
        WaitWaferStageReadyToLoadTimeout = 4000006,
        WaitWaferStageReadyToUnloadTimeout = 4000007,
        WaitStopReplyTimeout = 4000008,
        WaitHomeReplyTimeout = 4000009,
        WaitStartReplyTimeout = 4000010,
        Qry_FCB_SV_Fail = 4000011,
        SetWaferInfoFormatError = 4000012,
        SetFrameInfoFormatError = 4000013,
        SetWaferInfoFail = 4000014,
        SetFrameInfoFail = 4000015,
        WaitPPListTimeout = 4000016,
        FrameStageUpTimeout = 4000017,
        FrameStageDownTimeout = 4000018,
        QryWaferOrientationFail = 4000019,
        RcmdFail = 4000020,
        WaitUnloadFrameReplyTimeout = 4000021,
        CmdReadBarcodeReplyTimeout = 4000022,
        WaitBinRequestTimeout = 4000023,
        WaitProcessStateReadyTimeout = 4000024,
        WaitProcessStateReadyLoopTimeout = 4000025,
        CmdStartFail = 4000026,
        CmdUnloadFrameFail = 4000027,
        CmdPauseFail = 4000028,
        BESI_Disconnect = 4000029,
        InputMapNotExist = 4000030,
        ReadInputMapLinesFail = 4000031,
        CmdSetBPOFail = 4000032,
        WaitCJEndTimeout = 4000033,
        Robot1VacuumOnOffFail = 4000034,
        PrepareForWaferFail = 4000035,
        PrepareForFrameFail = 4000036,
        VOCSensorStateChangedFail = 4000037,

        //TVOC
        TVOC_Disconnect = 5000001,

    }
    #endregion 
    #region SVID
    public enum SVID
    {
        //Auto RS
        WaferID = 8070001,
        LotID = 8070002,
        Recipe = 8070005,
        CarrierID = 8070011,
        Port = 8070012,
        Slot = 8070013,
    }
    #endregion
    #region ECID
    public enum ECID
    {
        Robot1_Z_Speed = 6910003,
        Robot1_Move_Speed = 6910004,
        Robot2_Z_Speed = 6910005,
        Robot2_Move_Speed = 6910006,
    }
    #endregion
    #region CEID
    public enum CEID
    {
        //Auto RS
        Robot1_UpperArm_Occupied = 9510101,
        Robot1_UpperArm_Unoccupied = 9510102,
        Robot1_LowerArm_Occupied = 9510201,
        Robot1_LowerArm_Unoccupied = 9510202,

        Robot2_UpperArm_Occupied = 9520101,
        Robot2_UpperArm_Unoccupied = 9520102,
        Robot2_LowerArm_Occupied = 9520201,
        Robot2_LowerArm_Unoccupied = 9520202,

        Aligner1_Occupied = 9610101,
        Aligner1_Unoccupied = 9610102,
        Aligner1_Alignment_Start = 9610103,
        Aligner1_Alignment_End = 9610104,

        Aligner2_Occupied = 9620101,
        Aligner2_Unoccupied = 9620102,
        Aligner2_Alignment_Start = 9620103,
        Aligner2_Alignment_End = 9620104,

        FrameStage_ReadyToLoad = 9770171,
        FrameStage_Occupied = 9770172,
        FrameStage_ReadyToUnload = 9770173,
        FrameStage_Unoccupied = 9770174,

        WaferStage_ReadyToLoad = 9770175,
        WaferStage_Occupied = 9770176,
        WaferStage_ReadyToUnload = 9770177,
        WaferStage_Unoccupied = 9770178,
    }
    #endregion

    public delegate void BackGroundProgressEvent(string ThreadName);
    public delegate void AlarmEvent(string Code, string Description);
    public delegate void MotionEvent(string CEID);
    public delegate void MotionWithValueEvent(string CEID, string[] SVIDList, string[] ValueList);
    public delegate void SAEvent(string CEID);
    public delegate void SaDieLevelEvent(string CEID, string[] SVIDList, string[] ValueList);
    public delegate void CreateWaferInfoEvent(string CJID);
    public delegate void UpdateCycleCountEvent(string CycleCount, Product FrameWafer);
    public delegate void RefreshAutoRunEvent();

    public class AML_Library
    {
        public static bool CheckFile(string targetdir, string filename) //目前filename需附加副檔名，未加入預設值
        {
            string TargetFile = Path.Combine(targetdir, filename);
            return File.Exists(TargetFile);
        }
        public static bool CheckFile(string targetdir, string[] filename)
        {
            for (int i = 0; i < filename.Length; i++)
            {
                if (!CheckFile(targetdir, filename[i]))
                    return false;
            }
            return true;
        }
    }
}
