using UnityEngine;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using AOT;
using agora_gaming_rtc;
/* class IRtcEngine provides c# API for Unity 3D
 * app. Use IRtcEngine to access underlying Agora
 * sdk.
 * 
 * Agora sdk only supports single instance by now. So here
 * provides GetEngine() and Destroy() to create/delete the
 * only instance.
 */

namespace agora_gaming_rtc
{
    public class IRtcEngine : IRtcEngineBase
    {
        #region set callback here for user
        public delegate void JoinChannelSuccessHandler(string channelName, uint uid, int elapsed);
        public JoinChannelSuccessHandler OnJoinChannelSuccess;

        public delegate void ReJoinChannelSuccessHandler(string channelName, uint uid, int elapsed);
        public ReJoinChannelSuccessHandler OnReJoinChannelSuccess;

        public delegate void ConnectionLostHandler();
        public ConnectionLostHandler OnConnectionLost;

        public delegate void ConnectionInterruptedHandler();
        public ConnectionInterruptedHandler OnConnectionInterrupted;

        public delegate void RequestTokenHandler();
        public RequestTokenHandler OnRequestToken;

        public delegate void UserJoinedHandler(uint uid, int elapsed);
        public UserJoinedHandler OnUserJoined;

        public delegate void UserOfflineHandler(uint uid, USER_OFFLINE_REASON reason);
        public UserOfflineHandler OnUserOffline;

        public delegate void LeaveChannelHandler(RtcStats stats);
        public LeaveChannelHandler OnLeaveChannel;

        public delegate void VolumeIndicationHandler(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume);
        public VolumeIndicationHandler OnVolumeIndication;

        public delegate void UserMutedHandler(uint uid, bool muted);
        public UserMutedHandler OnUserMuted;

        public delegate void SDKWarningHandler(int warn, string msg);
        public SDKWarningHandler OnWarning;

        public delegate void SDKErrorHandler(int error, string msg);
        public SDKErrorHandler OnError;

        public delegate void RtcStatsHandler(RtcStats stats);
        public RtcStatsHandler OnRtcStats;

        public delegate void AudioMixingFinishedHandler();
        public AudioMixingFinishedHandler OnAudioMixingFinished;

        public delegate void AudioRouteChangedHandler(AUDIO_ROUTE route);
        public AudioRouteChangedHandler OnAudioRouteChanged;

        public delegate void OnFirstRemoteVideoDecodedHandler(uint uid, int width, int height, int elapsed);
        public OnFirstRemoteVideoDecodedHandler OnFirstRemoteVideoDecoded;

        public delegate void OnVideoSizeChangedHandler(uint uid, int width, int height, int elapsed);
        public OnVideoSizeChangedHandler OnVideoSizeChanged;

        public delegate void OnClientRoleChangedHandler(int oldRole, int newRole);
        public OnClientRoleChangedHandler OnClientRoleChanged;

        public delegate void OnUserMuteVideoHandler(uint uid, bool muted);
        public OnUserMuteVideoHandler OnUserMuteVideo;

        public delegate void OnMicrophoneEnabledHandler(bool isEnabled);
        public OnMicrophoneEnabledHandler OnMicrophoneEnabled;

        public delegate void OnFirstRemoteAudioFrameHandler(uint userId, int elapsed);
        public OnFirstRemoteAudioFrameHandler OnFirstRemoteAudioFrame;

        public delegate void OnFirstLocalAudioFrameHandler(int elapsed);
        public OnFirstLocalAudioFrameHandler OnFirstLocalAudioFrame;

        public delegate void OnApiExecutedHandler(int err, string api, string result);
        public OnApiExecutedHandler OnApiExecuted;

        public delegate void OnLastmileQualityHandler(int quality);
        public OnLastmileQualityHandler OnLastmileQuality;

        public delegate void OnAudioQualityHandler(uint userId, int quality, ushort delay, ushort lost);
        public OnAudioQualityHandler OnAudioQuality;

        public delegate void OnStreamInjectedStatusHandler(string url, uint userId, int status);
        public OnStreamInjectedStatusHandler OnStreamInjectedStatus;

        public delegate void OnStreamUnpublishedHandler(string url);
        public OnStreamUnpublishedHandler OnStreamUnpublished;

        public delegate void OnStreamPublishedHandler(string url, int error);
        public OnStreamPublishedHandler OnStreamPublished;

        public delegate void OnStreamMessageErrorHandler(uint userId, int streamId, int code, int missed, int cached);
        public OnStreamMessageErrorHandler OnStreamMessageError;

        public delegate void OnStreamMessageHandler(uint userId, int streamId, string data, int length);
        public OnStreamMessageHandler OnStreamMessage;

        public delegate void OnConnectionBannedHandler();
        public OnConnectionBannedHandler OnConnectionBanned;
        #endregion  set callback here for user

        private readonly AudioEffectManagerImpl mAudioEffectM;

        [MonoPInvokeCallback(typeof(EngineEventOnJoinChannelSuccessHandler))]
        private static void OnJoinChannelSuccessCallBack(string channel, uint uid, int elapsed)
        {
            if (instance != null && instance.OnJoinChannelSuccess != null)
            {
                object[] obj = new object[4];
                obj[0] = CallBackType.ON_JOIN_CHANNEL_SUCCESS;
                obj[1] = channel;
                obj[2] = uid;
                obj[3] = elapsed;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLeaveChannelHandler))]
        private static void OnLeaveChannelCallBack(uint duration, uint txBytes, uint rxBytes,
                                    ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate,
                                    ushort txAudioKBitRate, ushort rxVideoKBitRate,
                                    ushort txVideoKBitRate, ushort lastmileQuality, uint userCount, double cpuAppUsage,
                                    double cpuTotalUsage)
        {
            if (instance != null && instance.OnLeaveChannel != null)
            {
                object[] obj = new object[14];
                obj[0] = CallBackType.ON_LEAVE_CHANNEL;
                obj[1] = duration;
                obj[2] = txBytes;
                obj[3] = rxBytes;
                obj[4] = txKBitRate;
                obj[5] = rxKBitRate;
                obj[6] = rxAudioKBitRate;
                obj[7] = txAudioKBitRate;
                obj[8] = rxVideoKBitRate;
                obj[9] = txVideoKBitRate;
                obj[10] = lastmileQuality;
                obj[11] = userCount;
                obj[12] = cpuAppUsage;
                obj[13] = cpuTotalUsage;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnReJoinChannelSuccessHandler))]
        private static void OnReJoinChannelSuccessCallBack(string channelName, uint uid, int elapsed)
        {
            if (instance != null && instance.OnReJoinChannelSuccess != null)
            {
                object[] obj = new object[4];
                obj[0] = CallBackType.ON_REJOIN_CHANNEL_SUCCESS;
                obj[1] = channelName;
                obj[2] = uid;
                obj[3] = elapsed;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnConnectionLostHandler))]
        private static void OnConnectionLostCallBack()
        {
            if (instance != null && instance.OnConnectionLost != null)
            {
                object[] obj = new object[1];
                obj[0] = CallBackType.ON_CONNECTION_LOST;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnConnectionInterruptedHandler))]
        private static void OnConnectionInterruptedCallBack()
        {
            if (instance != null && instance.OnConnectionInterrupted != null)
            {
                object[] obj = new object[1];
                obj[0] = CallBackType.ON_CONNECTION_INTERRUPTED;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRequestTokenHandler))]
        private static void OnRequestTokenCallBack()
        {
            if (instance != null && instance.OnRequestToken != null)
            {
                object[] obj = new object[1];
                obj[0] = CallBackType.ON_REQUEST_TOKEN;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnUserJoinedHandler))]
        private static void OnUserJoinedCallBack(uint uid, int elapsed)
        {
            if (instance != null && instance.OnUserJoined != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_USER_JOINED;
                obj[1] = uid;
                obj[2] = elapsed;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnUserOfflineHandler))]
        private static void OnUserOfflineCallBack(uint uid, int reason)
        {
            if (instance != null && instance.OnUserOffline != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_USER_OFFLINE;
                obj[1] = uid;
                obj[2] = reason;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioVolumeIndicationHandler))]
        private static void OnAudioVolumeIndicationCallBack(string volumeInfo, int speakerNumber, int totalVolume)
        {
            if (instance != null && instance.OnVolumeIndication != null)
            {
                object[] obj = new object[4];
                obj[0] = CallBackType.ON_AUDIO_VOLUME_INDICATION;
                obj[1] = volumeInfo;
                obj[2] = speakerNumber;
                obj[3] = totalVolume;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnUserMuteAudioHandler))]
        private static void OnUserMuteAudioCallBack(uint uid, bool muted)
        {
            if (instance != null && instance.OnUserMuted != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_USER_MUTE_AUDIO;
                obj[1] = uid;
                obj[2] = muted;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnSDKWarningHandler))]
        private static void OnSDKWarningCallBack(int warn, string msg)
        {
            if (instance != null && instance.OnWarning != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_SDK_WARNING;
                obj[1] = warn;
                obj[2] = msg;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnSDKErrorHandler))]
        private static void OnSDKErrorCallBack(int error, string msg)
        {
            if (instance != null && instance.OnError != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_SDK_ERROR;
                obj[1] = error;
                obj[2] = msg;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRtcStatsHandler))]
        private static void OnRtcStatsCallBack(uint duration, uint txBytes, uint rxBytes,
                                    ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate,
                                    ushort txAudioKBitRate, ushort rxVideoKBitRate,
                                    ushort txVideoKBitRate, ushort lastmileQuality, uint userCount, double cpuAppUsage,
                                    double cpuTotalUsage)
        {
            if (instance != null && instance.OnRtcStats != null)
            {
                object[] obj = new object[14];
                obj[0] = CallBackType.ON_RTC_STATS;
                obj[1] = duration;
                obj[2] = txBytes;
                obj[3] = rxBytes;
                obj[4] = txKBitRate;
                obj[5] = rxKBitRate;
                obj[6] = rxAudioKBitRate;
                obj[7] = txAudioKBitRate;
                obj[8] = rxVideoKBitRate;
                obj[9] = txVideoKBitRate;
                obj[10] = lastmileQuality;
                obj[11] = userCount;
                obj[12] = cpuAppUsage;
                obj[13] = cpuTotalUsage;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioMixingFinishedHandler))]
        private static void OnAudioMixingFinishedCallBack()
        {
            if (instance != null && instance.OnAudioMixingFinished != null)
            {
                object[] obj = new object[1];
                obj[0] = CallBackType.ON_AUDIO_MIXING_FINISHED;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioRouteChangedHandler))]
        private static void OnAudioRouteChangedCallBack(int route)
        {
            if (instance != null && instance.OnAudioRouteChanged != null)
            {
                object[] obj = new object[2];
                obj[0] = CallBackType.ON_AUDIO_ROUTE_CHANGED;
                obj[1] = route;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstRemoteVideoDecodedHandler))]
        private static void OnFirstRemoteVideoDecodedCallBack(uint uid, int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteVideoDecoded != null)
            {
                object[] obj = new object[5];
                obj[0] = CallBackType.ON_FIRST_REMOTE_VIDEO_DECODED;
                obj[1] = uid;
                obj[2] = width;
                obj[3] = height;
                obj[4] = elapsed;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnVideoSizeChangedHandler))]
        private static void OnVideoSizeChangedCallBack(uint uid, int width, int height, int elapsed)
        {
            if (instance != null && instance.OnVideoSizeChanged != null)
            {

            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnClientRoleChangedHandler))]
        private static void OnClientRoleChangedCallBack(int oldRole, int newRole)
        {
            if (instance != null && instance.OnClientRoleChanged != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_CLIENT_ROLE_CHANGED;
                obj[1] = oldRole;
                obj[2] = newRole;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnUserMuteVideoHandler))]
        private static void OnUserMuteVideoCallBack(uint uid, bool muted)
        {
            if (instance != null && instance.OnUserMuteVideo != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_USER_MUTE_VIDEO;
                obj[1] = uid;
                obj[2] = muted;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnMicrophoneEnabledHandler))]
        private static void OnMicrophoneEnabledCallBack(bool isEnabled)
        {
            if (instance != null && instance.OnMicrophoneEnabled != null)
            {
                object[] obj = new object[2];
                obj[0] = CallBackType.ON_MICROPHONE_ENABLED;
                obj[1] = isEnabled;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnApiExecutedHandler))]
        private static void OnApiExecutedCallBack(int err, string api, string result)
        {
            if (instance != null && instance.OnApiExecuted != null)
            {
                object[] obj = new object[4];
                obj[0] = CallBackType.ON_API_EXECUTED;
                obj[1] = err;
                obj[2] = api;
                obj[3] = result;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnFirstLocalAudioFrameHandler))]
        private static void OnFirstLocalAudioFrameCallBack(int elapsed)
        {
            if (instance != null && instance.OnFirstLocalAudioFrame != null)
            {
                object[] obj = new object[2];
                obj[0] = CallBackType.ON_FIRST_LOCAL_AUDIO_FRAME;
                obj[1] = elapsed;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnFirstRemoteAudioFrameHandler))]
        private static void OnFirstRemoteAudioFrameCallBack(uint userId, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteAudioFrame != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_FIRST_REMOTE_AUDIO_FRAME;
                obj[1] = userId;
                obj[2] = elapsed;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLastmileQualityHandler))]
        private static void OnLastmileQualityCallBack(int quality)
        {
            if (instance != null && instance.OnLastmileQuality != null)
            {
                object[] obj = new object[2];
                obj[0] = CallBackType.ON_LAST_MILE_QUALITY;
                obj[1] = quality;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioQualityHandler))]
        private static void OnAudioQualityCallBack(uint userId, int quality, ushort delay, ushort lost)
        {
            if (instance != null && instance.OnAudioQuality != null)
            {
                object[] obj = new object[5];
                obj[0] = CallBackType.ON_AUDIO_QUALITY;
                obj[1] = userId;
                obj[2] = quality;
                obj[3] = delay;
                obj[4] = lost;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnStreamInjectedStatusHandler))]
        private static void OnStreamInjectedStatusCallBack(string url, uint userId, int status)
        {
            if (instance != null && instance.OnStreamInjectedStatus != null)
            {
                object[] obj = new object[4];
                obj[0] = CallBackType.ON_STREAM_INJECTED_STATUS;
                obj[1] = url;
                obj[2] = userId;
                obj[3] = status;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnStreamUnpublishedHandler))]
        private static void OnStreamUnpublishedCallBack(string url)
        {
            if (instance != null && instance.OnStreamUnpublished != null)
            {
                object[] obj = new object[2];
                obj[0] = CallBackType.ON_STREAM_UN_PUBLISHED;
                obj[1] = url;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnStreamPublishedHandler))]
        private static void OnStreamPublishedCallBack(string url, int error)
        {
            if (instance != null && instance.OnStreamPublished != null)
            {
                object[] obj = new object[3];
                obj[0] = CallBackType.ON_STREAM_PUBLISHED;
                obj[1] = url;
                obj[2] = error;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnStreamMessageErrorHandler))]
        private static void OnStreamMessageErrorCallBack(uint userId, int streamId, int code, int missed, int cached)
        {
            if (instance != null && instance.OnStreamMessageError != null)
            {
                object[] obj = new object[6];
                obj[0] = CallBackType.ON_STREAM_MESSAGE_ERROR;
                obj[1] = userId;
                obj[2] = streamId;
                obj[3] = code;
                obj[4] = missed;
                obj[5] = cached;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnStreamMessageHandler))]
        private static void OnStreamMessageCallBack(uint userId, int streamId, string data, int length)
        {
            if (instance != null && instance.OnStreamMessage != null)
            {
                object[] obj = new object[5];
                obj[0] = CallBackType.ON_STREAM_MESSAGE;
                obj[1] = userId;
                obj[2] = streamId;
                obj[3] = data;
                obj[4] = length;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnConnectionBannedHandler))]
        private static void OnConnectionBannedCallBack()
        {
            if (instance != null && instance.OnConnectionBanned != null)
            {
                object[] obj = new object[1];
                obj[0] = CallBackType.ON_CONNECTION_BANNED;
                AgoraCallBackQueue.Current.EnQueue(obj);
            }
        }
        private IRtcEngine(string appId)
        {
            //AgoraLoom.Initialize();
            InitGameObject();
            initCallBackObject();
            InitEngineCallBack();
            createEngine(appId);
            mAudioEffectM = new AudioEffectManagerImpl(this);
        }

        private void InitGameObject()
        {
            agoraGameObject = new GameObject(agoraGameObjectName);
            agoraGameObject.AddComponent<AgoraCallBackQueue>();
            GameObject.DontDestroyOnLoad(agoraGameObject);
            agoraGameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private static void DeInitGameObject()
        {
            GameObject agoraGameObject = GameObject.Find(agoraGameObjectName);
            if (!ReferenceEquals(agoraGameObject, null))
            {
                AgoraCallBackQueue.Current.ClearQueue();
                GameObject.Destroy(agoraGameObject);
                agoraGameObject = null;
            }
        }
        /**
		 */
        private void InitEngineCallBack()
        {
            initEventOnJoinChannelSuccess(OnJoinChannelSuccessCallBack);
            initEventOnLeaveChannel(OnLeaveChannelCallBack);
            initEventOnReJoinChannelSuccess(OnReJoinChannelSuccessCallBack);
            initEventOnConnectionLost(OnConnectionLostCallBack);
            initEventOnConnectionInterrupted(OnConnectionInterruptedCallBack);
            initEventOnRequestToken(OnRequestTokenCallBack);
            initEventOnUserJoined(OnUserJoinedCallBack);
            initEventOnUserOffline(OnUserOfflineCallBack);
            initEventOnAudioVolumeIndication(OnAudioVolumeIndicationCallBack);
            initEventOnUserMuteAudio(OnUserMuteAudioCallBack);
            initEventOnSDKWarning(OnSDKWarningCallBack);
            initEventOnSDKError(OnSDKErrorCallBack);
            initEventOnRtcStats(OnRtcStatsCallBack);
            initEventOnAudioMixingFinished(OnAudioMixingFinishedCallBack);
            initEventOnAudioRouteChanged(OnAudioRouteChangedCallBack);
            initEventOnFirstRemoteVideoDecoded(OnFirstRemoteVideoDecodedCallBack);
            initEventOnVideoSizeChanged(OnVideoSizeChangedCallBack);
            initEventOnClientRoleChanged(OnClientRoleChangedCallBack);
            initEventOnUserMuteVideo(OnUserMuteVideoCallBack);
            initEventOnMicrophoneEnabled(OnMicrophoneEnabledCallBack);
            initEventOnApiExecuted(OnApiExecutedCallBack);
            initEventOnFirstLocalAudioFrame(OnFirstLocalAudioFrameCallBack);
            initEventOnFirstRemoteAudioFrame(OnFirstRemoteAudioFrameCallBack);
            initEventOnLastmileQuality(OnLastmileQualityCallBack);
            initEventOnAudioQuality(OnAudioQualityCallBack);
            initEventOnStreamInjectedStatus(OnStreamInjectedStatusCallBack);
            initEventOnStreamUnpublished(OnStreamUnpublishedCallBack);
            initEventOnStreamPublished(OnStreamPublishedCallBack);
            initEventOnStreamMessageError(OnStreamMessageErrorCallBack);
            initEventOnStreamMessage(OnStreamMessageCallBack);
            initEventOnConnectionBanned(OnConnectionBannedCallBack);
        }

        private class AgoraCallBackQueue : MonoBehaviour
        {
            private static Queue<object[]> queue = new Queue<object[]>();
            private static AgoraCallBackQueue _current;
            public static AgoraCallBackQueue Current
            {
                get
                {
                    return _current;
                }
            }

            public void ClearQueue()
            {
                lock (queue)
                {
                    queue.Clear();
                }
            }

            public void EnQueue(object[] obj)
            {
                lock (queue)
                {
                    if (queue.Count >= 100)
                    {
                        queue.Dequeue();
                    }
                    queue.Enqueue(obj);
                }
            }

            private object[] DeQueue()
            {
                object[] obj = null;
                lock (queue)
                {
                    obj = queue.Dequeue();
                }
                return obj;
            }

            void Awake()
            {
                _current = this;
            }
            // Update is called once per frame
            void Update()
            {
                if (queue.Count > 0)
                {
                    object[] obj = DeQueue();
                    if (obj != null)
                    {
                        OnEvent(obj);
                    }
                }
            }

            void OnEvent(object[] obj)
            {
                CallBackType callBackType = (CallBackType)obj[0];
                if (instance != null)
                {
                    switch (callBackType)
                    {
                        case CallBackType.ON_JOIN_CHANNEL_SUCCESS:
                            string channel = (string)obj[1];
                            uint uid = (uint)obj[2];
                            int elapsed = (int)obj[3];
                            instance.OnJoinChannelSuccess(channel, uid, elapsed);
                            obj = null;
                            break;

                        case CallBackType.ON_LEAVE_CHANNEL:
                            //Log("Agora_zt    OnEvent   leaveChannel start");
                            RtcStats rtcStats = new RtcStats();
                            rtcStats.duration = (uint)obj[1];
                            rtcStats.txBytes = (uint)obj[2];
                            rtcStats.rxBytes = (uint)obj[3];
                            rtcStats.txKBitRate = (ushort)obj[4];
                            rtcStats.rxKBitRate = (ushort)obj[5];
                            rtcStats.rxAudioKBitRate = (ushort)obj[6];
                            rtcStats.txAudioKBitRate = (ushort)obj[7];
                            rtcStats.rxVideoKBitRate = (ushort)obj[8];
                            rtcStats.txVideoKBitRate = (ushort)obj[9];
                            rtcStats.lastmileQuality = (ushort)obj[10];
                            rtcStats.users = (uint)obj[11];
                            rtcStats.cpuAppUsage = (double)obj[12];
                            rtcStats.cpuTotalUsage = (double)obj[13];
                            instance.OnLeaveChannel(rtcStats);
                            obj = null;
                            break;

                        case CallBackType.ON_REJOIN_CHANNEL_SUCCESS:
                            instance.OnReJoinChannelSuccess((string)obj[1], (uint)obj[2], (int)obj[3]);
                            obj = null;
                            break;

                        case CallBackType.ON_USER_JOINED:
                            instance.OnUserJoined((uint)obj[1], (int)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_USER_OFFLINE:
                            instance.OnUserOffline((uint)obj[1], (USER_OFFLINE_REASON)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_CONNECTION_LOST:
                            instance.OnConnectionLost();
                            obj = null;
                            break;

                        case CallBackType.ON_CONNECTION_INTERRUPTED:
                            instance.OnConnectionInterrupted();
                            obj = null;
                            break;

                        case CallBackType.ON_AUDIO_VOLUME_INDICATION:
                            string volumeInfo = (string)obj[1];
                            int speakerNumber = (int)obj[2];
                            int totalVolume = (int)obj[3];

                            string[] sArray = volumeInfo.Split('\t');
                            int j = 1;
                            AudioVolumeInfo[] infos = new AudioVolumeInfo[speakerNumber];
                            if (speakerNumber > 0)
                            {
                                for (int i = 0; i < speakerNumber; i++)
                                {
                                    uint uids = (uint)int.Parse(sArray[j++]);
                                    uint vol = (uint)int.Parse(sArray[j++]);
                                    infos[i].uid = uids;
                                    infos[i].volume = vol;
                                }
                            }
                            instance.OnVolumeIndication(infos, speakerNumber, totalVolume);
                            obj = null;
                            break;

                        case CallBackType.ON_USER_MUTE_AUDIO:
                            instance.OnUserMuted((uint)obj[1], (bool)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_USER_MUTE_VIDEO:
                            instance.OnUserMuteVideo((uint)obj[1], (bool)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_SDK_ERROR:
                            instance.OnError((int)obj[1], (string)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_SDK_WARNING:
                            instance.OnWarning((int)obj[1], (string)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_MICROPHONE_ENABLED:
                            instance.OnMicrophoneEnabled((bool)obj[1]);
                            obj = null;
                            break;

                        case CallBackType.ON_AUDIO_ROUTE_CHANGED:
                            instance.OnAudioRouteChanged((AUDIO_ROUTE)obj[1]);
                            obj = null;
                            break;

                        case CallBackType.ON_CLIENT_ROLE_CHANGED:
                            instance.OnClientRoleChanged((int)obj[1], (int)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_REQUEST_TOKEN:
                            instance.OnRequestToken();
                            obj = null;
                            break;

                        case CallBackType.ON_AUDIO_MIXING_FINISHED:
                            instance.OnAudioMixingFinished();
                            obj = null;
                            break;

                        case CallBackType.ON_FIRST_REMOTE_VIDEO_DECODED:
                            instance.OnFirstRemoteVideoDecoded((uint)obj[1], (int)obj[2], (int)obj[3], (int)obj[4]);
                            obj = null;
                            break;

                        case CallBackType.ON_RTC_STATS:
                            RtcStats rtcStatss = new RtcStats();
                            rtcStatss.duration = (uint)obj[1];
                            rtcStatss.txBytes = (uint)obj[2];
                            rtcStatss.rxBytes = (uint)obj[3];
                            rtcStatss.txKBitRate = (ushort)obj[4];
                            rtcStatss.rxKBitRate = (ushort)obj[5];
                            rtcStatss.rxAudioKBitRate = (ushort)obj[6];
                            rtcStatss.txAudioKBitRate = (ushort)obj[7];
                            rtcStatss.rxVideoKBitRate = (ushort)obj[8];
                            rtcStatss.txVideoKBitRate = (ushort)obj[9];
                            rtcStats.lastmileQuality = (ushort)obj[10];
                            rtcStatss.users = (uint)obj[11];
                            rtcStatss.cpuAppUsage = (double)obj[12];
                            rtcStatss.cpuTotalUsage = (double)obj[13];
                            instance.OnRtcStats(rtcStatss);
                            obj = null;
                            break;

                        case CallBackType.ON_API_EXECUTED:
                            instance.OnApiExecuted((int)obj[1], (string)obj[2], (string)obj[3]);
                            obj = null;
                            break;

                        case CallBackType.ON_FIRST_LOCAL_AUDIO_FRAME:
                            instance.OnFirstLocalAudioFrame((int)obj[1]);
                            obj = null;
                            break;

                        case CallBackType.ON_FIRST_REMOTE_AUDIO_FRAME:
                            instance.OnFirstRemoteAudioFrame((uint)obj[1], (int)obj[2]);
                            obj = null;
                            break;

                        case CallBackType.ON_AUDIO_QUALITY:
                            instance.OnAudioQuality((uint)obj[1], (int)obj[2], (ushort)obj[3], (ushort)obj[4]);
                            break;

                        case CallBackType.ON_STREAM_INJECTED_STATUS:
                            instance.OnStreamInjectedStatus((string)obj[1], (uint)obj[2], (int)obj[3]);
                            break;

                        case CallBackType.ON_STREAM_UN_PUBLISHED:
                            instance.OnStreamUnpublished((string)obj[1]);
                            break;

                        case CallBackType.ON_STREAM_PUBLISHED:
                            instance.OnStreamPublished((string)obj[1], (int)obj[2]);
                            break;

                        case CallBackType.ON_STREAM_MESSAGE_ERROR:
                            instance.OnStreamMessageError((uint)obj[1], (int)obj[2], (int)obj[3], (int)obj[4], (int)obj[5]);
                            break;

                        case CallBackType.ON_STREAM_MESSAGE:
                            instance.OnStreamMessage((uint)obj[1], (int)obj[2], (string)obj[3], (int)obj[4]);
                            break;

                        case CallBackType.ON_CONNECTION_BANNED:
                            instance.OnConnectionBanned();
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public string doFormat(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        /**
		 * get the version information of the SDK
		 *
		 * @return return the version string
		 */
        public static string GetSdkVersion()
        {
            return Marshal.PtrToStringAnsi(getSdkVersion());
        }

        /**
		 * get the error description from SDK (deprecated)
		 * @param [in] code
		 *        the error code
		 * @return return the error description string
		 */
        public static string GetErrorDescription(int code)
        {
            return "Unknown";
        }

        /**
		 * Set the channel profile: such as game free mode, game command mode
		 *
		 * @param profile the channel profile
		 * @return return 0 if success or an error code
		 */
        public int SetChannelProfile(CHANNEL_PROFILE profile)
        {
            return setChannelProfile((int)profile);
        }

        /**
		 * Set the role of user: such as broadcaster, audience
		 *
		 * @param role the role of client
		 * @param permissionKey the permission key to apply the role
		 * @return return 0 if success or an error code
		 */
        public int SetClientRole(CLIENT_ROLE role)
        {
            return setClientRole((int)role);
        }

        /**
		 * set the log information filter level
		 *
		 * @param [in] filter
		 *        the filter level
		 * @return return 0 if success or an error code
		 */
        public int SetLogFilter(LOG_FILTER filter)
        {
            return setLogFilter((uint)filter);
        }

        // about audio effects: use c interface instead of interface IAudioEffectManager
        /**
		 * set path to save the log file
		 *
		 * @param [in] filePath
		 *        the .log file path you want to saved
		 * @return return 0 if success or an error code
		 */
        public int SetLogFile(string filePath)
        {
            return setLogFile(filePath);
        }

        /**
		 * join the channel, if the channel have not been created, it will been created automatically
		 *
		 * @param [in] channelName
		 *        the channel name
		 * @param [in] info
		 *        the additional information, it can be null here
		 * @param [in] uid
		 *        the uid of you, if 0 the system will automatically allocate one for you
		 * @return return 0 if success or an error code
		 */
        public int JoinChannel(string channelName, string info, uint uid)
        {
            return JoinChannelByKey(null, channelName, info, uid);
        }

        public int JoinChannelByKey(string channelKey, string channelName, string info, uint uid)
        {
            int r = joinChannel(channelKey, channelName, info, uid);
            return r;
        }

        public int RenewToken(string token)
        {
            // save parameters
            return renewToken(token);
        }

        /**
		 * leave the current channel
		 *
		 * @return return 0 if success or an error code
		 */
        public int LeaveChannel()
        {
            int r = leaveChannel(); // leave uncondionally
            return r;
        }

        /**
		 * Pause only control audio,you can use DisableAudio replace this api
		 */
        public void Pause()
        {
            Debug.Log("Pause engine");
            DisableAudio();
        }

        /**
		 * Resume only control audio,you can use EnableAudio replace this api
		 */
        public void Resume()
        {
            Debug.Log("Resume engine");
            EnableAudio();
        }

        /**
		 * set parameters of the SDK
		 *
		 * @param [in] parameters
		 *        the parameters(in json format)
		 * @return return 0 if success or an error code
		 */
        public int SetParameters(string parameters)
        {
            return setParameters(parameters);
        }

        public int SetParameter(string parameter, int value)
        {
            string parameters = doFormat("{{\"{0}\": {1}}}", parameter, value);
            return setParameters(parameters);
        }

        public int SetParameter(string parameter, double value)
        {
            string parameters = doFormat("{{\"{0}\": {1}}}", parameter, value);
            return setParameters(parameters);
        }

        public int SetParameter(string parameter, bool value)
        {
            string boolValue = value ? "true" : "false";
            string parameters = doFormat("{{\"{0}\": {1}}}", parameter, boolValue);
            return setParameters(parameters);
        }

        public string GetCallId()
        {
            string s = null;
            IntPtr res = getCallId();
            if (res != IntPtr.Zero)
            {
                s = Marshal.PtrToStringAnsi(res);
                freeObject(res);
            }
            return s;
        }

        public int Rate(string callId, int rating, string desc)
        {
            return rate(callId, rating, desc);
        }

        public int Complain(string callId, string desc)
        {
            return complain(callId, desc);
        }


        /**
		 * enable audio function, which is enabled by deault.
		 *
		 * @return return 0 if success or an error code
		 */
        public int EnableAudio()
        {
            return enableAudio();
        }

        /**
		 * disable audio function
		 *
		 * @return return 0 if success or an error code
		 */
        public int DisableAudio()
        {
            return disableAudio();
        }

        /**
		 * mute/unmute the local audio stream capturing
		 *
		 * @param [in] mute
		 *       true: mute
		 *       false: unmute
		 * @return return 0 if success or an error code
		 */
        public int MuteLocalAudioStream(bool mute)
        {
            return muteLocalAudioStream(mute);
        }

        /**
		 * mute/unmute all the remote audio stream receiving
		 *
		 * @param [in] mute
		 *       true: mute
		 *       false: unmute
		 * @return return 0 if success or an error code
		 */
        public int MuteAllRemoteAudioStreams(bool mute)
        {
            return muteAllRemoteAudioStreams(mute);
        }

        /**
		 * mute/unmute specified remote audio stream receiving
		 *
		 * @param [in] uid
		 *        the uid of the remote user you want to mute/unmute
		 * @param [in] mute
		 *       true: mute
		 *       false: unmute
		 * @return return 0 if success or an error code
		 */
        public int MuteRemoteAudioStream(uint uid, bool mute)
        {
            return muteRemoteAudioStream(uid, mute);
        }

        public int SetEnableSpeakerphone(bool speakerphone)
        {
            return setEnableSpeakerphone(speakerphone);
        }

        public int SetDefaultAudioRouteToSpeakerphone(bool speakerphone)
        {
            return setDefaultAudioRoutetoSpeakerphone(speakerphone);
        }

        public bool IsSpeakerphoneEnabled()
        {
            return isSpeakerphoneEnabled() != 0;
        }

        public int SwitchCamera()
        {
            return switchCamera();
        }

        public int SetVideoProfile(int profile, bool swapWidthAndHeight)
        {
            return setVideoProfile(profile, swapWidthAndHeight);
        }

        public int MuteLocalVideoStream(bool mute)
        {
            return muteLocalVideoStream(mute);
        }

        public int MuteAllRemoteVideoStreams(bool mute)
        {
            return muteAllRemoteVideoStreams(mute);
        }

        public int MuteRemoteVideoStream(uint uid, bool mute)
        {
            return muteRemoteVideoStream(uid, mute);
        }

        public int EnableDualStreamMode(bool enabled)
        {
            return enableDualStreamMode(enabled);
        }

        public int SetEncryptionMode(string encryptionMode)
        {
            return setEncryptionMode(encryptionMode);
        }

        public int SetEncryptionSecret(string secret)
        {
            return setEncryptionSecret(secret);
        }

        public int StartRecordingService(string recordingKey)
        {
            return startRecordingService(recordingKey);
        }

        public int StopRecordingService(string recordingKey)
        {
            return stopRecordingService(recordingKey);
        }

        public int RefreshRecordingServiceStatus()
        {
            return refreshRecordingServiceStatus();
        }


        public int CreateDataStream(bool reliable, bool ordered)
        {
            return createDataStream(reliable, ordered);
        }

        public int SendStreamMessage(int streamId, string data)
        {
            return sendStreamMessage(streamId, data, data.Length);
        }


        public int SetRecordingAudioFrameParametersWithSampleRate(int sampleRate, int channel, int mode, int samplesPerCall)
        {
            return setRecordingAudioFrameParametersWithSampleRate(sampleRate, channel, mode, samplesPerCall);
        }

        public int SetPlaybackAudioFrameParametersWithSampleRate(int sampleRate, int channel, int mode, int samplesPerCall)
        {
            return setPlaybackAudioFrameParametersWithSampleRate(sampleRate, channel, mode, samplesPerCall);
        }

        public int SetSpeakerphoneVolume(int volume)
        {
            return setSpeakerphoneVolume(volume);
        }

        public int SetVideoQualityParameters(bool preferFrameRateOverImageQuality)
        {
            return setVideoQualityParameters(preferFrameRateOverImageQuality);
        }

        public int StartEchoTest()
        {
            return startEchoTest();
        }

        public int StopEchoTest()
        {
            return stopEchoTest();
        }

        public int SetRemoteVideoStreamType(uint uid, int streamType)
        {
            return setRemoteVideoStreamType(uid, streamType);
        }

        public int SetMixedAudioFrameParameters(int sampleRate, int samplesPerCall)
        {
            return setMixedAudioFrameParameters(sampleRate, samplesPerCall);
        }

        public int SetAudioMixingPosition(int pos)
        {
            return setAudioMixingPosition(pos);
        }

        /**
		 * enable or disable the audio volume indication
		 *
		 * @param [in] interval
		 *        the period of the callback cycle, in ms
		 *        interval <= 0: disable
		 *        interval >  0: enable
		 * @param [in] smooth
		 *        the smooth parameter
		 * @return return 0 if success or an error code
		 */
        public int EnableAudioVolumeIndication(int interval, int smooth)
        {
            return enableAudioVolumeIndication(interval, smooth);
        }

        /**
		 * adjust recording signal volume
		 *
		 * @param [in] volume range from 0 to 400
		 * @return return 0 if success or an error code
		 */
        public int AdjustRecordingSignalVolume(int volume)
        {
            return adjustRecordingSignalVolume(volume);
        }

        /**
		 * adjust playback signal volume
		 *
		 * @param [in] volume range from 0 to 400
		 * @return return 0 if success or an error code
		 */
        public int AdjustPlaybackSignalVolume(int volume)
        {
            return adjustPlaybackSignalVolume(volume);
        }

        /**
		 * mix microphone and local audio file into the audio stream
		 *
		 * @param [in] filePath
		 *        specify the path and file name of the audio file to be played
		 * @param [in] loopback
		 *        specify if local and remote participant can hear the audio file.
		 *        false (default): both local and remote party can hear the the audio file
		 *        true: only the local party can hear the audio file
		 * @param [in] replace
		 *        false (default): mix the local microphone captured voice with the audio file
		 *        true: replace the microphone captured voice with the audio file
		 * @param [in] cycle
		 *        specify the number of cycles to play
		 *        -1, infinite loop playback
		 * @param [in] playTime (not support)
		 *        specify the start time(ms) of the audio file to play
		 *        0, from the start
		 * @return return 0 if success or an error code
		 */
        public int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle, int playTime = 0)
        {
            return startAudioMixing(filePath, loopback, replace, cycle, playTime);
        }

        /**
		 * stop mixing the local audio stream
		 *
		 * @return return 0 if success or an error code
		 */
        public int StopAudioMixing()
        {
            return stopAudioMixing();
        }

        /**
		 * pause mixing the local audio stream
		 *
		 * @return return 0 if success or an error code
		 */
        public int PauseAudioMixing()
        {
            return pauseAudioMixing();
        }

        /**
		 * resume mixing the local audio stream
		 *
		 * @return return 0 if success or an error code
		 */
        public int ResumeAudioMixing()
        {
            return resumeAudioMixing();
        }

        /**
		 * adjust mixing audio file volume
		 *
		 * @param [in] volume range from 0 to 100
		 * @return return 0 if success or an error code
		 */
        public int AdjustAudioMixingVolume(int volume)
        {
            return adjustAudioMixingVolume(volume);
        }

        /**
		 * get the duration of the specified mixing audio file
		 *
		 * @return return duration(ms)
		 */
        public int GetAudioMixingDuration()
        {
            return getAudioMixingDuration();
        }

        /**
		 * get the current playing position of the specified mixing audio file
		 *
		 * @return return the current playing(ms)
		 */
        public int GetAudioMixingCurrentPosition()
        {
            return getAudioMixingCurrentPosition();
        }

        /**
		 * start recording audio streaming to file specified by the file path
		 *
		 * @param filePath file path to save recorded audio streaming
		 * @return return 0 if success or an error code
		 * Deprecated. Use int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality)
		 */
        public int StartAudioRecording(string filePath)
        {
            return StartAudioRecording(filePath, AUDIO_RECORDING_QUALITY_TYPE.AUDIO_RECORDING_QUALITY_MEDIUM);
        }

        public int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality)
        {
            return startAudioRecording(filePath, (int)quality);
        }

        /**
		 * stop audio streaming recording
		 *
		 * @return return 0 if success or an error code
		 */
        public int StopAudioRecording()
        {
            return stopAudioRecording();
        }

        public IAudioEffectManager GetAudioEffectManager()
        {
            return mAudioEffectM;
        }

        /**
		 * start video engine and start record video data
		 * 
		 * @return return 0 if success or an error code
		 */
        public int EnableVideo()
        {
            return enableVideo();
        }

        /**
		 * stop video engine and stop record video data
		 *
		 * @return return 0 if success or an error code
		 */
        public int DisableVideo()
        {
            return disableVideo();
        }

        /**
		 * open/close your local video ,don't affect remote video.
		 * 
		 * @return return 0 if success or an error code
		 */
        public int EnableLocalVideo(bool enabled)
        {
            return enableLocalVideo(enabled);
        }

        /**
		 * open/close your local microphone, don't affect remote audio.
		 * 
		 * @return return 0 if success or an error code
		 */
        public int EnableLocalAudio(bool enabled)
        {
            return enableLocalAudio(enabled);
        }

        public int StartPreview()
        {
            return startPreview();
        }

        public int StopPreview()
        {
            return stopPreview();
        }

        public int GetEffectsVolume()
        {
            return getEffectsVolume();
        }

        public int SetEffectsVolume(int volume)
        {
            return setEffectsVolume(volume);
        }

        public int PlayEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish)
        {
            return playEffect(soundId, filePath, loopCount, pitch, pan, gain, publish);
        }

        public int StopEffect(int soundId)
        {
            return stopEffect(soundId);
        }

        public int StopAllEffects()
        {
            return stopAllEffects();
        }

        public int PreloadEffect(int soundId, string filePath)
        {
            return preloadEffect(soundId, filePath);
        }

        public int UnloadEffect(int soundId)
        {
            return unloadEffect(soundId);
        }

        public int PauseEffect(int soundId)
        {
            return pauseEffect(soundId);
        }

        public int PauseAllEffects()
        {
            return pauseAllEffects();
        }

        public int ResumeEffect(int soundId)
        {
            return resumeEffect(soundId);
        }

        public int ResumeAllEffects()
        {
            return resumeAllEffects();
        }

        /**
		 * If you want to video work in unity, you need to call both enableVideo and enableVideoObserver, 
		 * this api is used to get data from agora raw data which can produce texture by openGl.
		 *
		 * @return return 0 if success or error code.
		 */
        public int EnableVideoObserver()
        {
            return enableVideoObserver();
        }

        /**
		 * this api is only used to stop get video raw data from agora engine, it won't stop video engine.
		 * If you want to stop video engine ,you can call disableVideo.
		 *
		 * @return return 0 if success or error code.
		 */
        public int DisableVideoObserver()
        {
            return disableVideoObserver();
        }

        // load data to texture
        public int UpdateTexture(int tex, uint uid, IntPtr data, ref int width, ref int height)
        {
            int rc = updateTexture(tex, data, uid);
            if (rc == -1)
                return -1;
            width = (int)rc >> 16;
            height = (int)(rc & 0xffff);
            return 0;
        }

        public int GenerateNativeTexture()
        {
            return generateNativeTexture();
        }

        public void DeleteTexture(int tex)
        {
            deleteTexture(tex);
        }

        public int SetLocalVoicePitch(double pitch)
        {
            return setLocalVoicePitch(pitch);
        }

        public int SetRemoteVoicePosition(uint uid, double pan, double gain)
        {
            return setRemoteVoicePosition(uid, pan, gain);
        }

        public int SetVoiceOnlyMode(bool enable)
        {
            return setVoiceOnlyMode(enable);
        }

        public int SetDefaultMuteAllRemoteAudioStreams(bool mute)
        {
            return setDefaultMuteAllRemoteAudioStreams(mute);
        }

        public int SetDefaultMuteAllRemoteVideoStreams(bool mute)
        {
            return setDefaultMuteAllRemoteVideoStreams(mute);
        }

        public int EnableLastmileTest()
        {
            return enableLastmileTest();
        }

        public int DisableLastmileTest()
        {
            return disableLastmileTest();
        }

        public static IRtcEngine GetEngine(string appId)
        {
            if (instance == null)
            {
                instance = new IRtcEngine(appId);
            }
            return instance;
        }

        // depricated. use GetEngine instead
        public static IRtcEngine getEngine(string appId)
        {
            return GetEngine(appId);
        }

        public static void Destroy()
        {
            if (instance == null)
                return;

            // break the connection with mAudioEffectM
            AudioEffectManagerImpl am = (AudioEffectManagerImpl)instance.GetAudioEffectManager();
            am.SetEngine(null);

            deleteEngine();
            instance = null;
            DeInitGameObject();
        }

        // only query, do not create
        public static IRtcEngine QueryEngine()
        {
            return instance;
        }
        private static IRtcEngine instance = null;
    }
};

