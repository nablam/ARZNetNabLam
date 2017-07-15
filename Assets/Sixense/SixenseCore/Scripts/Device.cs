﻿//
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
    /// <summary>
    /// SixenseCore.Device provides a centralized interface for accessing Sixense trackers.
    /// </summary>
    /// <remarks>
    /// This script should be bound to a GameObject in the scene so that its Start(), Update() and OnApplicationQuit() methods are called.  This can be done by adding the SixenseInput prefab to a scene.  The public static interface to the Controller objects provides a user friendly way to integrate Sixense controllers into your application.
    /// </remarks>
    public class Device : MonoBehaviour
    {
        #region Public Variables
        [Tooltip("If true, the Unity physics timestep will be changed to match the update rate of the STEM sensor, currently 240hz, " +
                "and the physics simulation will be advanced for every update of the sensor hardware.")]
        public bool m_fixedUpdate = false;

        [Tooltip("Number of millimeters per world unit.  A value of 1000 corresponds to a world scale of 1 unit per meter, the Unity default.")]
        public float m_worldUnitScaleInMillimeters = 1000;
        #endregion

        #region Private Variables
        private static Tracker[] ms_Trackers = null;
        private static uint ms_maxTrackers = 0;
        private static uint ms_historySize = 0;
        private static uint ms_activeTrackers = 0;
        private static bool ms_driverDistortionCorrection = false;
        private static bool ms_initialized = false;
        private static string ms_version = null;
        public static string[] NetworkBridgeAddresses = { "0.0.0.0", "0.0.0.0", "0.0.0.0", "0.0.0.0" };

        float m_fixedTime = 0;
        bool m_driverInitialized = false;
        static bool ms_globalInitialized = false;
        #endregion

        #region Properties
        /// <summary>
        /// Returns a version string for the Core API
        /// </summary>
        public static string APIVersion
        {
            get
            {
                if (ms_version == null)
                {
                    if (!ms_globalInitialized)
                    {
                        // load sxCore dependent libraries
#if UNITY_STANDALONE_WIN
                            try { Plugin.pthread_load(); }
                            catch (Exception e) { }
#endif
#if !WINDOWS_UWP
                        try { Plugin.libusb_load(); }
                        catch (Exception e) { }
                        try { Plugin.ftdi_load(); }
                        catch (Exception e) { }
#endif
                    }

                    ms_version = Marshal.PtrToStringAnsi(Plugin.sxCoreGetAPIVersion());
                }
                return ms_version;
            }
        }

        /// <summary>
        /// Returns true if the device has been initialized and is ready to use
        /// </summary>
        public static bool Initialized
        {
            get { return ms_initialized; }
        }

        public static void StopSystem()
        {
            Plugin.sxCoreExit();
        }

        public static void StartSystem()
        {
            Plugin.sxCoreInit();
        }

        public static int AutoSync
        {
            get
            {
                if (!ms_initialized)
                    return 0;

                int val = 0;
                Plugin.sxCoreRequestAutoSyncEnabled(out val);
                return val;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetAutoSyncEnabled(value);
            }
        }

        /// <summary>
        /// Type of tracking system
        /// </summary>
        public static System SystemType
        {
            get
            {
                return Plugin.sxCoreGetSystemType(0);
            }
        }

        /// <summary>
        /// Total number of trackers currently enabled and ready to use
        /// </summary>
        public static uint NumberActiveTrackers
        {
            get
            {
                return ms_activeTrackers;
            }
        }

        /// <summary>
        /// Maximum number of trackers supported by this system
        /// </summary>
        public static uint MaxNumberTrackers
        {
            get
            {
                return ms_maxTrackers;
            }
        }

        /// <summary>
        /// Number of historical samples available to read
        /// </summary>
        public static uint HistorySize
        {
            get
            {
                return ms_historySize;
            }
        }

        /// <summary>
        /// Microseconds since initialization
        /// </summary>
        public static ulong TrackingTime
        {
            get
            {
                ulong t;
                Plugin.sxCoreGetCurrentTime(out t);
                return t;
            }
        }

        public static void SetSingleHemisphere(int tracked_device_index, float x, float y, float z, bool use_hemisphere_tracking, bool save_to_nvram)
        {
            var hemiTracking = Convert.ToInt32(use_hemisphere_tracking);
            Plugin.sxCoreSetSingleHemisphere(Convert.ToUInt32(tracked_device_index), x, y, z, hemiTracking, Convert.ToInt32(save_to_nvram));
        }

        public static void SetEMFrequency(uint tracked_device_index, int channel)
        {
            Plugin.sxCoreSetEMFrequencyChannel(tracked_device_index, channel);
        }

        public static void SetDeviceAutoSync(uint tracked_device_index, bool on_or_off)
        {
            Plugin.sxCoreSetDeviceAutoSyncEnabled(tracked_device_index, Convert.ToInt32(on_or_off));
        }

        public static AutoSyncStatus eGetDeviceAutoSyncStatus(uint tracked_device_index)
        {
            var status = new AutoSyncStatus();
            Plugin.sxCoreGetDeviceAutoSyncStatus(tracked_device_index, out status);
            return status;
        }

        /*public static int GetEMFrequency(uint tracked_device_index)
        {
            Plugin.sxCoreRequestEMFrequencyChannel(tracked_device_index, channel);
        }*/

        /// <summary>
        /// Enable smoothing of the position and orientation based on distance to reduce noise, on by default
        /// </summary>
        public static bool FilterJitterEnabled
        {
            get
            {
                if (!ms_initialized)
                    return false;

                int e = 0;
                Plugin.sxCoreGetFilterEnabled((uint)Filter.JITTER, out e);
                return e != 0;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterEnabled((uint)Filter.JITTER, value ? 1 : 0);
            }
        }

        /// <summary>
        /// Enable smoothing of the position and orientation based on distance to reduce noise, on by default
        /// </summary>
        public static bool FilterMovingAverageEnabled
        {
            get
            {
                if (!ms_initialized)
                    return false;

                int e = 0;
                Plugin.sxCoreGetFilterEnabled((uint)Filter.MOVING_AVERAGE, out e);
                return e != 0;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterEnabled((uint)Filter.MOVING_AVERAGE, value ? 1 : 0);
            }
        }

        /// <summary>
        /// Enable smoothing of the position and orientation based on distance to reduce noise, on by default
        /// </summary>
        public static bool FilterMovingAverageWindowEnabled
        {
            get
            {
                if (!ms_initialized)
                    return false;

                int e = 0;
                Plugin.sxCoreGetFilterEnabled((uint)Filter.MOVING_AVERAGE_WINDOW, out e);
                return e != 0;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterEnabled((uint)Filter.MOVING_AVERAGE_WINDOW, value ? 1 : 0);
            }
        }

        /// <summary>
        /// Enable smoothing of the position and orientation based on distance to reduce noise, on by default
        /// </summary>
        public static bool FilterDoubleMovingAverageWindowEnabled
        {
            get
            {
                if (!ms_initialized)
                    return false;

                int e = 0;
                Plugin.sxCoreGetFilterEnabled((uint)Filter.DOUBLE_MOVING_AVERAGE_WINDOW, out e);
                return e != 0;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterEnabled((uint)Filter.DOUBLE_MOVING_AVERAGE_WINDOW, value ? 1 : 0);
            }
        }

        /// <summary>
        /// The parameters of the noise filter
        /// </summary>
        public static JitterFilterParams FilterJitterSettings
        {
            get
            {
                if (!ms_initialized)
                    return new JitterFilterParams();

                JitterFilterParams p;
                Plugin.sxCoreGetFilterParams((uint)Filter.JITTER, out p);
                return p;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterParams((uint)Filter.JITTER, ref value);
            }
        }

        /// <summary>
        /// The parameters of the noise filter
        /// </summary>
        public static MovingAverageFilterParams FilterMovingAverageSettings
        {
            get
            {
                if (!ms_initialized)
                    return new MovingAverageFilterParams();

                MovingAverageFilterParams p;
                Plugin.sxCoreGetFilterParams((uint)Filter.MOVING_AVERAGE, out p);
                return p;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterParams((uint)Filter.MOVING_AVERAGE, ref value);
            }
        }

        /// <summary>
        /// The parameters of the noise filter
        /// </summary>
        public static MovingAverageWindowFilterParams FilterMovingAverageWindowSettings
        {
            get
            {
                if (!ms_initialized)
                    return new MovingAverageWindowFilterParams();

                MovingAverageWindowFilterParams p;
                Plugin.sxCoreGetFilterParams((uint)Filter.MOVING_AVERAGE_WINDOW, out p);
                return p;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterParams((uint)Filter.MOVING_AVERAGE_WINDOW, ref value);
            }
        }

        /// <summary>
        /// The parameters of the noise filter
        /// </summary>
        public static DoubleMovingAverageWindowFilterParams FilterDoubleMovingAverageWindowSettings
        {
            get
            {
                if (!ms_initialized)
                    return new DoubleMovingAverageWindowFilterParams();

                DoubleMovingAverageWindowFilterParams p;
                Plugin.sxCoreGetFilterParams((uint)Filter.DOUBLE_MOVING_AVERAGE_WINDOW, out p);
                return p;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterParams((uint)Filter.DOUBLE_MOVING_AVERAGE_WINDOW, ref value);
            }
        }

        /// <summary>
        /// Uses an accelerometer and gyroscope to correct for magnetic distortion in the environment
        /// </summary>
        public static bool DistortionCorrectionEnabled
        {
            get
            {
                if (!ms_initialized)
                    return false;

                int e = 0;
                Plugin.sxCoreGetFilterEnabled((uint)Filter.IMU_DISTORTION_CORRECTION, out e);
                return e != 0;
            }
            set
            {
                if (!ms_initialized)
                    return;

                Plugin.sxCoreSetFilterEnabled((uint)Filter.IMU_DISTORTION_CORRECTION, value ? 1 : 0);
            }
        }

        /// <summary>
        /// Enable network bridge for mobile developement.
        /// </summary>
        public static bool NetworkBridgeEnabled
        {
            get
            {
                if (!ms_initialized)
                    return false;

                int e = 0;
                Plugin.sxCoreGetNetworkBridgeEnabled(out e);
                return e != 0;
            }
            set
            {
                if (!ms_initialized)
                    return;

                // set addresses
                uint max_addresses = 0;
                Plugin.sxCoreGetNetworkBridgeAddressIndexMaxAddresses(out max_addresses);
                if (max_addresses >= 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Plugin.sxCoreSetNetworkBridgeAddressIndex(NetworkBridgeAddresses[i], (uint)i);
                    }
                }

                Plugin.sxCoreSetNetworkBridgeEnabled(value ? 1 : 0);

                if (value)
                {
                    PlayerPrefs.SetString("sxCoreSetNetworkBridgeAddress0", NetworkBridgeAddresses[0]);
                    PlayerPrefs.SetString("sxCoreSetNetworkBridgeAddress1", NetworkBridgeAddresses[1]);
                    PlayerPrefs.SetString("sxCoreSetNetworkBridgeAddress2", NetworkBridgeAddresses[2]);
                    PlayerPrefs.SetString("sxCoreSetNetworkBridgeAddress3", NetworkBridgeAddresses[3]);
                }
            }
        }
#endregion

#region Public Methods
        /// <summary>
        /// Gets the Tracker object bound to the specified tracker ID.
        /// </summary>
        public static Tracker GetTracker(TrackerID id)
        {
            if (!ms_initialized)
                return null;

            for (int i = 0; i < ms_maxTrackers; i++)
            {
                if ((ms_Trackers[i] != null) && (ms_Trackers[i].ID == id))
                {
                    return ms_Trackers[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the Tracker object with the given device index
        /// </summary>
        public static Tracker GetTrackerByIndex(int index)
        {
            if (!ms_initialized)
                return null;

            if (index >= ms_maxTrackers || index < 0)
                return null;

            return ms_Trackers[index];
        }

        /// <summary>
        /// Returns true if the base for zero-based index i is connected.
        /// </summary>
        public static bool BaseConnected
        {
            get
            {
                if (!ms_initialized)
                    return false;

                return (Plugin.sxCoreGetBaseStatus(0) == BaseStatus.CONNECTED);
            }
        }

        /// <summary>
        /// Returns true if any tracker in this system has a button pressed
        /// </summary>
        public static bool GetAnyButton()
        {
            if (!ms_initialized)
                return false;

            foreach (var c in ms_Trackers)
            {
                if (c.GetAnyButton())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if any tracker in this system has a button pressed this frame
        /// </summary>
        public static bool GetAnyButtonDown()
        {
            if (!ms_initialized)
                return false;

            foreach (var c in ms_Trackers)
            {
                if (c.GetAnyButtonDown())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if any tracker in this system has a button released this frame
        /// </summary>
        public static bool GetAnyButtonUp()
        {
            if (!ms_initialized)
                return false;

            foreach (var c in ms_Trackers)
            {
                if (c.GetAnyButtonUp())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns hemi tracking init vector for the given tracker id
        /// </summary>
        public static Vector3 GetHemiTrackingInitVector(uint tracker_id)
        {
            Vector3 v = new Vector3();
            Plugin.sxCoreGetHemisphereTrackingInitVector(tracker_id, out v.x, out v.y, out v.z);
            return v;
        }

        /// <summary>
        /// Sets the hemi tracking init vector for the given tracker id
        /// </summary>
        public static void SetHemiTrackingInitVector(uint tracker_id, Vector3 v)
        {
            Plugin.sxCoreSetHemisphereTrackingInitVector(tracker_id, v.x, v.y, v.z);
        }
        #endregion

        #region Script Events
        /// <summary>
        /// Initialize the sixense driver
        /// </summary>
        void Awake()
        {
            NetworkBridgeAddresses[0] = PlayerPrefs.GetString("sxCoreSetNetworkBridgeAddress0", "0.0.0.0");
            NetworkBridgeAddresses[1] = PlayerPrefs.GetString("sxCoreSetNetworkBridgeAddress1", "0.0.0.0");
            NetworkBridgeAddresses[2] = PlayerPrefs.GetString("sxCoreSetNetworkBridgeAddress2", "0.0.0.0");
            NetworkBridgeAddresses[3] = PlayerPrefs.GetString("sxCoreSetNetworkBridgeAddress3", "0.0.0.0");

            if (!ms_globalInitialized)
            {
                // load sxCore dependent libraries
#if UNITY_STANDALONE_WIN
                    try { Plugin.pthread_load(); } catch (Exception e) { }
                    try { Plugin.libusb_load(); } catch (Exception e) { }
                    try { Plugin.ftdi_load(); } catch (Exception e) { }
#endif
                if (Plugin.sxCoreInit() == PluginTypes.Result.SUCCESS)
                {
                    string version = SixenseCore.Device.APIVersion;
                    Debug.Log("Sixense Core Init: Version "+version, gameObject);

                    m_driverInitialized = true;
                    ms_globalInitialized = true;
                }
                else
                {
                    enabled = false;
                    Debug.LogError("Sixense Core Init Failed", gameObject);
                }
            }
            else
            {
                m_driverInitialized = true;
            }
        }

        /// <summary>
        /// Exit sixense driver when the application quits.
        /// </summary>
        void OnApplicationQuit()
        {
            if (ms_globalInitialized)
            {
                if (Plugin.sxCoreExit() == PluginTypes.Result.SUCCESS)
                    Debug.Log("Sixense Core Exit", gameObject);
                else
                    Debug.LogWarning("Sixense Core Exit Failed", gameObject);

                m_driverInitialized = false;
            }
            else
            {
                Debug.LogWarning("Sixense Exit called while driver is not initialized");
            }
        }

        /// <summary>
        /// allocate the controllers, set fixed timestep if desired
        /// </summary>
        void OnEnable()
        {
            if (m_fixedUpdate)
            {
                m_fixedTime = Time.fixedDeltaTime;

                Time.fixedDeltaTime = 1.0f / 240 - Mathf.Epsilon;
            }

            if (ms_initialized)
            {
                Debug.LogWarning("Multiple SixenseCore Devices present. Ensure that you only have one instance in your scene.", gameObject);
                gameObject.SetActive(false);
                return;
            }

            if (m_driverInitialized)
            {
                ms_globalInitialized = true;

                ms_maxTrackers = Plugin.sxCoreGetMaxTrackedDevices();
                ms_historySize = Plugin.sxCoreGetHistorySize();

                ms_Trackers = new Tracker[ms_maxTrackers];
                for (int i = 0; i < ms_maxTrackers; i++)
                {
                    ms_Trackers[i] = new Tracker(this, i);
                }

                ms_initialized = true;
            }
        }

        /// <summary>
        /// Restore original timestep, stop vibration
        /// </summary>
        void OnDisable()
        {
            if (m_fixedUpdate)
            {
                Time.fixedDeltaTime = m_fixedTime;
            }

            ms_initialized = false;

            if (m_driverInitialized)
            {
                for (int i = 0; i < ms_maxTrackers; i++)
                {
                    if (ms_Trackers[i] != null)
                    {
                        ms_Trackers[i].SetVibration(0);
                    }
                }
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                for (int i = 0; i < ms_maxTrackers; i++)
                {
                    if (ms_Trackers[i] != null)
                    {
                        ms_Trackers[i].SetVibration(0);
                    }
                }
            }
        }

        /// <summary>
        /// Update the tracking data once per physics update at 240hz
        /// </summary>
        void FixedUpdate()
        {
            if (!m_fixedUpdate || !ms_initialized)
                return;

            // update controller data
            for (int i = 0; i < ms_maxTrackers; i++)
            {
                if (ms_Trackers[i] != null)
                {
                    ms_Trackers[i].FixedUpdate();
                }
            }
        }

        /// <summary>
        /// Update the tracking data once per frame, called by SixenseCore.Device
        /// </summary>
        void Update()
        {
            if (!ms_initialized)
                return;

            ms_driverDistortionCorrection = true;
            ms_activeTrackers = Plugin.sxCoreGetNumConnectedTrackedDevices();

            for (int i = 0; i < ms_maxTrackers; i++)
            {
                if (ms_Trackers[i] != null)
                {
                    if(!m_fixedUpdate)
                        ms_Trackers[i].PreUpdate();

                    ms_Trackers[i].Update(); // get absolute latest for rendering
                }
            }
        }
#endregion

#region Native Interface
        private partial class Plugin
        {
            // fake functions for dependent library loading
            [DllImport("pthread")]
            public static extern void pthread_load();
            [DllImport("usb")]
            public static extern void libusb_load();
            [DllImport("ftdi")]
            public static extern void ftdi_load();

            const string module = "sxCore";

            [DllImport(module)]
            public static extern IntPtr sxCoreGetAPIVersion();

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreInit();
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreExit();
            [DllImport(module)]
            public static extern uint sxCoreGetReferenceCount();

            [DllImport(module)]
            public static extern uint sxCoreGetMaxSystems();    
            [DllImport(module)]
            public static extern uint sxCoreGetNumSystems();   
            [DllImport(module)]
            public static extern System sxCoreGetSystemType(uint system_index);     
            [DllImport(module)]
            public static extern uint sxCoreGetActiveSystemIndex();       
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetActiveSystem(uint system_index);                         // set the active system to the given index, similar to how the old sixenseSetActiveBase(int) worked, except that we now have a higher concept of different systems, and a system can potentially have more than one base

            [DllImport(module)]
            public static extern uint sxCoreGetMaxBases();   
            [DllImport(module)]
            public static extern uint sxCoreGetNumBases();    
            [DllImport(module)]
            public static extern BaseStatus sxCoreGetBaseStatus(uint base_index);   
            [DllImport(module)]
            public static extern uint sxCoreGetActiveBaseIndex();       
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetActiveBase(uint base_index);   
            
            [DllImport(module)]
            public static extern uint sxCoreGetMaxTrackedDevices();
            [DllImport(module)]
            public static extern uint sxCoreGetNumEnabledTrackedDevices();   
            [DllImport(module)]
            public static extern uint sxCoreGetNumConnectedTrackedDevices();                                         // how many do we actually have active

            [DllImport(module)]
            public static extern uint sxCoreGetHistorySize();                                                     // this used to be 50, but that was for systems that ran at 60 Hz, we should up this to 240 (for a 1 second history)

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetCurrentTime(out ulong current_time);                          // current time, this time can be used to compare with the sxCoreTrackedDeviceData.packet_time
            
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetFilterEnabled(uint filter_type, int on_or_off);      // enable/disable application controllable filters
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetFilterEnabled(uint filter_type, out int on_or_off);     // enable/disable application controllable filters

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetFilterParams(uint filter_type, out MovingAverageFilterParams filter_params);     // get application controllable filter parameters, "filter_params" can be NULL for filters that don't use it
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetFilterParams(uint filter_type, ref MovingAverageFilterParams filter_params);     // set application controllable filter parameters, "filter_params" can be NULL for filters that don't use it

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetFilterParams(uint filter_type, out MovingAverageWindowFilterParams filter_params);     // get application controllable filter parameters, "filter_params" can be NULL for filters that don't use it
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetFilterParams(uint filter_type, ref MovingAverageWindowFilterParams filter_params);     // set application controllable filter parameters, "filter_params" can be NULL for filters that don't use it

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetFilterParams(uint filter_type, out DoubleMovingAverageWindowFilterParams filter_params);     // get application controllable filter parameters, "filter_params" can be NULL for filters that don't use it
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetFilterParams(uint filter_type, ref DoubleMovingAverageWindowFilterParams filter_params);     // set application controllable filter parameters, "filter_params" can be NULL for filters that don't use it

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetFilterParams(uint filter_type, out JitterFilterParams filter_params);     // get application controllable filter parameters, "filter_params" can be NULL for filters that don't use it
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetFilterParams(uint filter_type, ref JitterFilterParams filter_params);     // set application controllable filter parameters, "filter_params" can be NULL for filters that don't use it

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetHemisphereTrackingInitVector(uint tracked_id, out float x, out float y, out float z);   // get the hemisphere tracking init vector for a sync id
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetHemisphereTrackingInitVector(uint tracked_id, float x, float y, float z);               // set the hemisphere tracking init vector for a sync id

            public delegate PluginTypes.Result sxCoreRequestSingleHemisphereCallback(uint tracked_device_index, float x, float y, float z, int use_hemisphere_tracking, int saved_to_nvram);

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetSingleHemisphere(uint tracked_device_index, float x, float y, float z, int use_hemisphere_tracking, int save_to_nvram);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreRequestSingleHemisphere(uint tracked_device_index, int nvram_setting, sxCoreRequestSingleHemisphereCallback callback);

            public delegate PluginTypes.Result sxCoreRequestEMFrequencyChannelCallback(uint tracked_device_index, int em_freq_channel);

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreRequestEMFrequencyChannel(uint tracked_device_index, sxCoreRequestEMFrequencyChannelCallback callback);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetEMFrequencyChannel(uint tracked_device_index, int em_freq_channel);

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetNetworkBridgeAddress(string ip_address);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetNetworkBridgeEnabled(int on_or_off);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetNetworkBridgeEnabled(out int on_or_off);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetNetworkBridgeAddressIndex(string ip_address, uint index);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetNetworkBridgeAddressIndexMaxAddresses(out uint max_addresses);

            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreRequestAutoSyncEnabled(out int on_or_off);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetAutoSyncEnabled(int on_or_off);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreSetDeviceAutoSyncEnabled(uint tracked_device_index, int on_or_off);
            [DllImport(module)]
            public static extern PluginTypes.Result sxCoreGetDeviceAutoSyncStatus(uint tracked_device_index, out AutoSyncStatus status);
        }
        #endregion

        #region Extended for ARZ
        public void ForceUpdateTrackers()
        {
            if (!ms_initialized)
                return;

            ms_driverDistortionCorrection = true;
            ms_activeTrackers = Plugin.sxCoreGetNumConnectedTrackedDevices();

            for (int i = 0; i < ms_maxTrackers; i++)
            {
                if (ms_Trackers[i] != null)
                {
                    if (!m_fixedUpdate)
                        ms_Trackers[i].PreUpdate();

                    ms_Trackers[i].Update(); // get absolute latest for rendering
                }
            }
        }
        #endregion
    }
}
