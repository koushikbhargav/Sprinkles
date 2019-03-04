using UnityEngine;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using AOT;

namespace agora_gaming_rtc
{
    #region some enum types
    public enum CallBackType
    {
        ON_JOIN_CHANNEL_SUCCESS = 0,
        ON_LEAVE_CHANNEL = 1,
        ON_REJOIN_CHANNEL_SUCCESS = 2,
        ON_CONNECTION_LOST = 3,
        ON_CONNECTION_INTERRUPTED = 4,
        ON_REQUEST_TOKEN = 5,
        ON_USER_JOINED = 6,
        ON_USER_OFFLINE = 7,
        ON_AUDIO_VOLUME_INDICATION = 8,
        ON_USER_MUTE_AUDIO = 9,
        ON_SDK_WARNING = 10,
        ON_SDK_ERROR = 11,
        ON_RTC_STATS = 12,
        ON_AUDIO_MIXING_FINISHED = 13,
        ON_AUDIO_ROUTE_CHANGED = 14,
        ON_FIRST_REMOTE_VIDEO_DECODED = 15,
        ON_CLIENT_ROLE_CHANGED = 16,
        ON_USER_MUTE_VIDEO = 17,
        ON_MICROPHONE_ENABLED = 18,
        ON_API_EXECUTED = 19,
        ON_FIRST_LOCAL_AUDIO_FRAME = 20,
        ON_FIRST_REMOTE_AUDIO_FRAME = 21,
        ON_LAST_MILE_QUALITY = 22,
        ON_AUDIO_QUALITY = 23,
        ON_STREAM_INJECTED_STATUS = 24,
        ON_STREAM_UN_PUBLISHED = 25,
        ON_STREAM_PUBLISHED = 26,
        ON_STREAM_MESSAGE_ERROR = 27,
        ON_STREAM_MESSAGE = 28,
        ON_CONNECTION_BANNED = 29
    }


    public struct RtcStats
    {
        public uint duration;
        public uint txBytes;
        public uint rxBytes;
        public ushort txKBitRate;
        public ushort rxKBitRate;
        public ushort txAudioKBitRate;
        public ushort rxAudioKBitRate;
        public ushort txVideoKBitRate;
        public ushort rxVideoKBitRate;
        public ushort lastmileQuality;
        public uint users;
        public double cpuAppUsage;
        public double cpuTotalUsage;
    };

    public enum USER_OFFLINE_REASON
    {
        QUIT = 0,
        DROPPED = 1,
        BECOME_AUDIENCE = 2,
    };

    public struct AudioVolumeInfo
    {
        public uint uid;
        public uint volume; // [0, 255]
    };

    public enum LOG_FILTER
    {
        OFF = 0,
        DEBUG = 0x80f,
        INFO = 0x0f,
        WARNING = 0x0e,
        ERROR = 0x0c,
        CRITICAL = 0x08,
    };

    public enum CHANNEL_PROFILE
    {
        GAME_FREE_MODE = 0,
        GAME_COMMAND_MODE = 1,
    };

    public enum CLIENT_ROLE
    {
        BROADCASTER = 1,
        AUDIENCE = 2,
    };

    public enum AUDIO_RECORDING_QUALITY_TYPE
    {
        AUDIO_RECORDING_QUALITY_LOW = 0,
        AUDIO_RECORDING_QUALITY_MEDIUM = 1,
        AUDIO_RECORDING_QUALITY_HIGH = 2,
    };

    public enum AUDIO_ROUTE
    {
        DEFAULT = -1,
        HEADSET = 0,
        EARPIECE = 1,
        SPEAKERPHONE = 3,
        BLUETOOTH = 5,
    };
    #endregion some enum types

    public abstract class IRtcEngineBase
    {
        #region DllImport
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		public const string MyLibName = "agoraSdkCWrapper";
#else

#if UNITY_IPHONE
		public const string MyLibName = "__Internal";
#else
        public const string MyLibName = "agoraSdkCWrapper";
#endif
#endif

        protected const string agoraGameObjectName = "agora_engine_CallBackGamObject";
        protected static GameObject agoraGameObject = null;

        // standard sdk api
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void createEngine(string appId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void deleteEngine();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getSdkVersion();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannel(string channelKey, string channelName, string info, uint uid);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoicePitch(double pitch);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVoicePosition(uint uid, double pan, double gain);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVoiceOnlyMode(bool enable);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int leaveChannel();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLastmileTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableLastmileTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableVideo();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableVideo();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalVideo(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalAudio(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startPreview();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopPreview();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableAudio();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableAudio();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setParameters(string options);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getCallId();
        // caller free the returned char * (through freeObject)
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int rate(string callId, int rating, string desc);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int complain(string callId, string desc);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEnableSpeakerphone(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int isSpeakerphoneEnabled();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultAudioRoutetoSpeakerphone(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableAudioVolumeIndication(int interval, int smooth);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecording(string filePath, int quality);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioRecording();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioMixing(string filePath, bool loopBack, bool replace, int cycle, int playTime);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioMixing();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseAudioMixing();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeAudioMixing();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingDuration();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingCurrentPosition();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalAudioStream(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteAudioStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteAudioStream(uint uid, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int switchCamera();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoProfile(int profile, bool swapWidthAndHeight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalVideoStream(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteVideoStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteVideoStream(uint uid, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFile(string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int renewToken(string token);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setChannelProfile(int profile);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole(int role);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableDualStreamMode(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionMode(string encryptionMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionSecret(string secret);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRecordingService(string recordingKey);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopRecordingService(string recordingKey);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int refreshRecordingServiceStatus();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createDataStream(bool reliable, bool ordered);
        // TODO! supports general data later. now only string is supported
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendStreamMessage(int streamId, string data, Int64 length);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRecordingAudioFrameParametersWithSampleRate(int sampleRate, int channel, int mode, int samplesPerCall);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setPlaybackAudioFrameParametersWithSampleRate(int sampleRate, int channel, int mode, int samplesPerCall);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setSpeakerphoneVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustRecordingSignalVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustPlaybackSignalVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setHighQualityAudioParametersWithFullband(int fullband, int stereo, int fullBitrate);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableInEarMonitoring(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableWebSdkInteroperability(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoQualityParameters(bool preferFrameRateOverImageQuality);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startEchoTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopEchoTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVideoStreamType(uint uid, int streamType);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setMixedAudioFrameParameters(int sampleRate, int samplesPerCall);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioMixingPosition(int pos);
        // setLogFilter: deprecated
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFilter(uint filter);
        // video texture stuff (extension for gaming)
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableVideoObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableVideoObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int generateNativeTexture();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateTexture(int tex, IntPtr data, uint uid);
        // return value: -1 for no update; otherwise (width << 16 | height)
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void deleteTexture(int tex);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setPlaybackDeviceVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getEffectsVolume();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEffectsVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int playEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAllEffects();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int preloadEffect(int soundId, string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unloadEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseAllEffects();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeAllEffects();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteAudioStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteVideoStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void freeObject(IntPtr obj);

        protected delegate void EngineEventOnJoinChannelSuccessHandler(string channel, uint uid, int elapsed);

        protected delegate void EngineEventOnLeaveChannelHandler(uint duration, uint txBytes, uint rxBytes,
                                    ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate,
                                    ushort txAudioKBitRate, ushort rxVideoKBitRate,
                                    ushort txVideoKBitRate, ushort lastmileQuality, uint userCount, double cpuAppUsage,
                                    double cpuTotalUsage);

        protected delegate void EngineEventOnReJoinChannelSuccessHandler(string channel, uint uid, int elapsed);

        protected delegate void EngineEventOnConnectionLostHandler();

        protected delegate void EngineEventOnConnectionInterruptedHandler();

        protected delegate void EngineEventOnRequestTokenHandler();

        protected delegate void EngineEventOnUserJoinedHandler(uint uid, int elapsed);

        protected delegate void EngineEventOnUserOfflineHandler(uint uid, int offLineReason);

        protected delegate void EngineEventOnAudioVolumeIndicationHandler(string volumeInfo, int speakerNumber, int totalVolume);

        protected delegate void EngineEventOnUserMuteAudioHandler(uint uid, bool muted);

        protected delegate void EngineEventOnSDKWarningHandler(int warn, string msg);

        protected delegate void EngineEventOnSDKErrorHandler(int error, string msg);

        protected delegate void EngineEventOnRtcStatsHandler(uint duration, uint txBytes, uint rxBytes,
                                    ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate,
                                    ushort txAudioKBitRate, ushort rxVideoKBitRate,
                                    ushort txVideoKBitRate, ushort lastmileQuality, uint userCount, double cpuAppUsage,
                                    double cpuTotalUsage);

        protected delegate void EngineEventOnAudioMixingFinishedHandler();

        protected delegate void EngineEventOnAudioRouteChangedHandler(int route);

        protected delegate void EngineEventOnFirstRemoteVideoDecodedHandler(uint uid, int width, int height, int elapsed);

        protected delegate void EngineEventOnVideoSizeChangedHandler(uint uid, int width, int height, int elapsed);

        protected delegate void EngineEventOnClientRoleChangedHandler(int oldRole, int newRole);

        protected delegate void EngineEventOnUserMuteVideoHandler(uint uid, bool muted);

        protected delegate void EngineEventOnMicrophoneEnabledHandler(bool isEnabled);

        protected delegate void EngineEventOnApiExecutedHandler(int err, string api, string result);

        protected delegate void EngineEventOnLastmileQualityHandler(int quality);

        protected delegate void EngineEventOnFirstLocalAudioFrameHandler(int elapsed);

        protected delegate void EngineEventOnFirstRemoteAudioFrameHandler(uint userId, int elapsed);

        protected delegate void EngineEventOnAudioQualityHandler(uint userId, int quality, ushort delay, ushort lost);

        protected delegate void EngineEventOnStreamInjectedStatusHandler(string url, uint userId, int status);

        protected delegate void EngineEventOnStreamUnpublishedHandler(string url);

        protected delegate void EngineEventOnStreamPublishedHandler(string url, int error);

        protected delegate void EngineEventOnStreamMessageErrorHandler(uint userId, int streamId, int code, int missed, int cached);

        protected delegate void EngineEventOnStreamMessageHandler(uint userId, int streamId, string data, int length);

        protected delegate void EngineEventOnConnectionBannedHandler();

        protected delegate void OnEngineEventHandler(int methodNumber, string data);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initCallBackObject();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnJoinChannelSuccess(EngineEventOnJoinChannelSuccessHandler OnJoinChannelSuccess);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnLeaveChannel(EngineEventOnLeaveChannelHandler OnLeaveChannel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnReJoinChannelSuccess(EngineEventOnReJoinChannelSuccessHandler OnReJoinChannelSuccess);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnConnectionLost(EngineEventOnConnectionLostHandler OnConnectionLost);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnConnectionInterrupted(EngineEventOnConnectionInterruptedHandler OnConnectionInterrupted);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnRequestToken(EngineEventOnRequestTokenHandler OnRequestToken);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnUserJoined(EngineEventOnUserJoinedHandler OnUserJoined);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnUserOffline(EngineEventOnUserOfflineHandler OnUserOffline);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnAudioVolumeIndication(EngineEventOnAudioVolumeIndicationHandler OnAudioVolumeIndication);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnUserMuteAudio(EngineEventOnUserMuteAudioHandler OnUserMuteAudio);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnSDKWarning(EngineEventOnSDKWarningHandler OnSDKWarning);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnSDKError(EngineEventOnSDKErrorHandler OnSDKError);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnRtcStats(EngineEventOnRtcStatsHandler OnRtcStats);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnAudioMixingFinished(EngineEventOnAudioMixingFinishedHandler OnAudioMixingFinished);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnAudioRouteChanged(EngineEventOnAudioRouteChangedHandler OnAudioRouteChanged);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnFirstRemoteVideoDecoded(EngineEventOnFirstRemoteVideoDecodedHandler OnFirstRemoteVideoDecoded);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnVideoSizeChanged(EngineEventOnVideoSizeChangedHandler OnVideoSizeChanged);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnClientRoleChanged(EngineEventOnClientRoleChangedHandler OnVideoSizeChanged);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnUserMuteVideo(EngineEventOnUserMuteVideoHandler OnUserMuteVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnMicrophoneEnabled(EngineEventOnMicrophoneEnabledHandler OnMicrophoneEnabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnApiExecuted(EngineEventOnApiExecutedHandler OnApiExecuted);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnFirstLocalAudioFrame(EngineEventOnFirstLocalAudioFrameHandler OnFirstLocalAudioFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnFirstRemoteAudioFrame(EngineEventOnFirstRemoteAudioFrameHandler OnFirstRemoteAudioFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnLastmileQuality(EngineEventOnLastmileQualityHandler OnLastmileQuality);
        
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnAudioQuality(EngineEventOnAudioQualityHandler onAudioQuality);
        
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnStreamInjectedStatus(EngineEventOnStreamInjectedStatusHandler onStreamInjectedStatus);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnStreamUnpublished(EngineEventOnStreamUnpublishedHandler onStreamUnpublished);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnStreamPublished(EngineEventOnStreamPublishedHandler onStreamPublished);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnStreamMessageError(EngineEventOnStreamMessageErrorHandler onStreamMessageError);
        
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnStreamMessage(EngineEventOnStreamMessageHandler onStreamMessage);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnConnectionBanned(EngineEventOnConnectionBannedHandler onConnectionBanned);

        #endregion engine callbacks   
    }
}