//
// Copyright (C) 2014 Sixense Entertainment Inc.
// All Rights Reserved
//
// SixenseCore Unity Plugin
// Version 0.1
//

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace SixenseCore
{
    #region Public Types
    public enum System
    {
        NONE         = 0,
        UNKNOWN      = (0x01 << 0),
        DEVKIT_3_2   = (0x01 << 1),
        HYDRA        = (0x01 << 2),
        REV_C_2      = (0x01 << 3),
        STEM         = (0x01 << 4),
        PROTOTYPE_01 = (0x01 << 5),
        PROTOTYPE_02 = (0x01 << 6),
        STEM_HANDS   = (0x01 << 7),
    }

    internal enum BaseStatus
    {
        NONE = 0,
        CONNECTED = (0x01 << 0),
    }
    
    /// <summary>
    /// Type of device
    /// </summary>
    public enum Hardware
    {
        NONE                       = 0,
        UNKNOWN                    = 1,

        // Razer Hydra
        HYDRA_CONTROLLER           = 2,

        // Sixense STEM System
        STEM_CONTROLLER            = 3,
        STEM_PACK                  = 4,

        // Prototype Devices
        REV_C2_CONTROLLER          = 5,
        DEVKIT_3_2_CONTROLLER      = 6,
        PROTOTYPE_01_CONTROLLER    = 7,
        PROTOTYPE_02_CONTROLLER    = 8,
        PROTOTYPE_02_DAUGHTERBOARD = 9,
        PROTOTYPE_02_CONTROLLER_P1 = 10,
        PROTOTYPE_02_CONTROLLER_P2 = 11,
        PROTOTYPE_02_CONTROLLER_P3 = 12,
        PROTOTYPE_02_CONTROLLER_P4 = 13,
        STEM_BASE                  = 14,

        // STEM Hands
        STEM_HANDS_CONTROLLER      = 15,
        STEM_HANDS_DAUGHTERBOARD   = 16,
    }

    /// <summary>
    /// Dock position tracker is bound to.
    /// </summary>
    public enum TrackerID
    {
        NONE                        = 0,
        CONTROLLER_LEFT             = 1,
        CONTROLLER_RIGHT            = 2,
        PACK_LEFT                   = 3,
        PACK_CENTER                 = 4,
        PACK_RIGHT                  = 5,
        IMU_HMD                     = 6,
        AUTO_SYNCED                 = 7,
    }

    /// <summary>
    /// Device status flags
    /// </summary>
    public enum Status
    { 
        NONE                                        = 0,
        ENABLED                                     = (0x01<<0),
        DOCKED                                      = (0x01<<1),
        HAS_EXTERNAL_POWER                          = (0x01<<2),
        BATTERY_CHARGING                            = (0x01<<3),
        BATTERY_LOW                                 = (0x01<<4),
        MODE_WIRED                                  = (0x01<<5),
        MODE_WIRELESS                               = (0x01<<6),
        MODE_BLUETOOTH                              = (0x01<<7),
        MODE_WIFI                                   = (0x01<<8),
        FILTER_HEMI_TRACKING_ENABLED                = (0x01<<9),
        FILTER_MOVING_AVERAGE_ENABLED               = (0x01<<10),
        FILTER_IMU_DISTORTION_CORRECTION_ENABLED    = (0x01<<11),
        FILTER_JITTER_ENABLED                       = (0x01<<12),
        IMU_POWERING_UP                             = (0x01<<13),
        CONVERT_COORD_ENABLED                       = (0x01<<14),
        CONVERT_QUAT_ENABLED                        = (0x01<<15),
        SYNC_REQUESTED                              = (0x01<<16),
        POWER_OFF_BUTTON_MODE                       = (0x01<<17),
        AUTO_SYNCING                                = (0x01<<18),
        BASE_TX_STATUS                              = (0x01<<19)
    }
    
    /// <summary>
    /// Filter types
    /// </summary>
    public enum Filter
    {
        NONE                                    = 0,
        HEMI_TRACKING                           = 1,
        MOVING_AVERAGE                          = 2,
        IMU_DISTORTION_CORRECTION               = 3,
        JITTER                                  = 4,
        POWER_CHANGE                            = 5,
        CONVERT_COORD                           = 6,
        CONVERT_QUAT                            = 7,
        AUTO_HEMI                               = 8,
        MOVING_AVERAGE_WINDOW                   = 9,
        CORRECT_COIL_OFFSET                     = 10,
        DOUBLE_MOVING_AVERAGE_WINDOW            = 11,
    }

    /// <summary>
    /// Controller button mask.
    /// </summary>
    /// <remarks>
    /// The TRIGGER button is set when the Trigger value is greater than Tracker.TriggerButtonThreshold.
    /// </remarks>
    public enum Buttons
    {
        START       = (1<<0),
        
        PREV        = (1<<1),
        NEXT        = (1<<2),

        A           = (1<<3),
        B           = (1<<5),
        X           = (1<<6),
        Y           = (1<<4),
        
        BUMPER      = (1<<7),
        JOYSTICK    = (1<<8),

        TRIGGER     = (1<<9),
    }

    /// <summary>
    /// Power off types
    /// </summary>
    public enum PowerState
    {
        SLEEP = 1,
        SHIPPING_MODE = 2,
        TX_OFF = 3,
        TX_ON = 4,
    }

    /// <summary>
    /// Parameters for the moving average filter
    /// 
    /// TODO: explain parameters
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MovingAverageFilterParams
    {
        public float NearRange;
        public float NearPosExp;
        public float NearRotExp;

        public float FarRange;
        public float FarPosExp;
        public float FarRotExp;
    }

    /// <summary>
    /// Parameters for the moving average window filter
    /// 
    /// TODO: explain parameters
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MovingAverageWindowFilterParams
    {
        public float NearRange;
        public uint NearWindowSize;
        public float FarRange;
        public uint FarWindowSize;
    }

    /// <summary>
    /// Parameters for the double moving average window filter
    /// 
    /// TODO: explain parameters
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DoubleMovingAverageWindowFilterParams
    {
        public float NearRange;
        public uint NearWindowSize;
        public float FarRange;
        public uint FarWindowSize;
    }

    /// <summary>
    /// Parameters for the adaptive moving average filter
    /// 
    /// TODO: explain parameters
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct JitterFilterParams
    {
        public float NearRange;
        public float NearPosDelta;
        
        public float FarRange;
        public float FarPosDelta;

        public float PosExp;
        public float RawPosExp;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AutoSyncStatus
    {
        public int id;
        public int stable;
        public int gyro_stable;
        public int accel_stable;
        public uint mag_outliers;
    }
    #endregion

    #region Native Interface
    internal class PluginTypes
    {
        internal enum Result
        { 
            SUCCESS											= 0,
            FAILURE											= -1,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TrackedDeviceData
        {
            public byte sequence_number;                // sequential index that wraps to 0 at 256
            public byte device_type;                    // device type id
            public byte dsp_status;                     // DSP status bits
            public byte battery_percent;                // approximate battery level
            public ushort tracker_id;                   // identifies which docking station the device was synchronized to
            public ushort tracked_device_index;         // unique index
            public uint status;                         // tracked device status bitmask
            public uint hardware_status;                // hardware status bits (MCU, IMU, etc.)
            public uint buttons;                        // button status bitmask
            public float trigger;                       // value is 0.0 to 1.0 from unpressed to pressed
            public float joystick_x;                    // value is -1.0 to 1.0 from left to right
            public float joystick_y;                    // value is -1.0 to 1.0 from down to up
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] pos;                         // actual position of the magnetic tracker inside the device
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] rot_quat;                    // actual orientation of the magnetic tracker inside the device
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public float[] rot_mat;                     // rotation in matrix format
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] rot_euler;                   // rotation in euler angles
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] imu_acc;                     // IMU accelerometer value
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] imu_gyro;                    // IMU gyroscope value
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] imu_mag;                     // IMU magnetometer value
            public byte imu_sequence_number;            // IMU sequential index
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] imu_gravity;                 // IMU fused local gravity vector
            public double packet_time;                  // time in ms from when sxCoreInit() was called when packet was measured on the device
            public double imu_time;                     // time in ms from when sxCoreInit() was called when IMU data was measured on the device
            public ushort mcu_timestamp;                // MCU timestamp for UI and IMU data
            public ushort dsp_timestamp;                // DSP timestamp for PnO data
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TrackedDeviceInfo
        {
            public byte runtime_version_major;
            public byte runtime_version_minor;
            public byte hardware_revision;
            public byte device_type_board;
            public ushort magnetic_frequency;
            public short tracked_device_index;
            public byte device_type;
            public byte device_type_extended;
            public ushort serial_number;
            public byte mcu_firmware_version_major;
            public byte mcu_firmware_version_minor;
            public byte dsp_firmware_version_major;
            public byte dsp_firmware_version_minor;
            public byte board_hardware_gpio;
            public byte battery_adc_level;
            public float battery_voltage;
            public byte imu_calibration_status;
            public byte dsp_gain_mode;
            public byte active_matrix_set;
            public byte debug_info;
            public byte mcu_build;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] product_string;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] serial_number_string;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] port_string;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] coil_rotation_quat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] coil_offset_pos;
        }
    }
    #endregion
}