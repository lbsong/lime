using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace voice2text
{
    class SpeechRecognizer
    {
        private const int RecordingBufferSize = 3200;

        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        private string sessionId;

        public bool IsListening { get; set; }

        public void SetParameter(string param, string value)
        {
            parameters[param] = value;
        }

        public void StartListening()
        {
            string argument = String.Empty;

            foreach(var param in parameters)
            {
                string arg = String.Format("{0} = {1},", param.Key, param.Value);
                argument += arg;
            }

            int errorCode = 0;
            var sessId = MSCDll.QISRSessionBegin(null, argument.Trim(','), ref errorCode);

            //TODO: check error code 

            this.sessionId = Marshal.PtrToStringAnsi(sessId);
        }

        public void StopListening()
        {
            MSCDll.QISRSessionEnd(sessionId, string.Empty);
        }

        public void WriteAudio(byte[] buffer, int offset, int length)
        {
            IntPtr bp = Marshal.AllocHGlobal(RecordingBufferSize);
            Marshal.Copy(buffer, 0, bp, length);

            int epStatus = 0;
            int recStatus = 0;

            ///开始向服务器发送音频数据
            int ret = MSCDll.QISRAudioWrite(this.sessionId, bp, (uint)RecordingBufferSize, 2, ref epStatus, ref recStatus);

            if (ret != 0)
            {
                Console.WriteLine(ret);
                throw new Exception("QISRAudioWrite err,errCode=" + ret);
            }
        }
    }

    public interface IRecognizerListener
    {
        void onVolumeChanged(int paramInt);

        void onBeginOfSpeech();

        void onEndOfSpeech();

        void onResult(string paramRecognizerResult, bool paramBoolean);

        void onEvent(int paramInt1, int paramInt2, int paramInt3, String paramString);
    }

    public class RecognizerListener
    {
        
    }

    public static class SpeechConstant
    {
        public const string ACCENT = "accent";
        public const string APPID = "appid";
        public const string ASR_AUDIO_PATH = "asr_audio_path";
        public const string ASR_NBEST = "asr_nbest";
        public const string ASR_NET_PERF = "asr_net_perf";
        public const string ASR_PTT = "asr_ptt";
        public const string ASR_SOURCE_PATH = "asr_source_path";
        public const string ASR_WBEST = "asr_wbest";
        public const string AUDIO_FORMAT = "audio_format";
        public const string AUDIO_SOURCE = "audio_source";
        public const string BACKGROUND_SOUND = "background_sound";
        public const string CLOUD_GRAMMAR = "cloud_grammar";
        public const string DATA_TYPE = "data_type";
        public const string DOMAIN = "domain";
        public const string ENG_ASR = "asr";
        public const string ENG_IVP = "ivp";
        public const string ENG_IVW = "ivw";
        public const string ENG_NLU = "nlu";
        public const string ENG_TTS = "tts";
        public const string ENGINE_MODE = "engine_mode";
        public const string ENGINE_TYPE = "engine_type";
        public const string FILTER_AUDIO_TIME = "filter_audio_time";
        public const string GRAMMAR_LIST = "grammar_list";
        public const string GRAMMAR_NAME = "grammar_name";
        public const string GRAMMAR_TYPE = "grammar_type";
        public const string ISV_AUDIO_PATH = "isv_audio_path";
        public const string ISV_AUTHID = "auth_id";
        public const string ISV_CMD = "cmd";
        public const string ISV_INTERRUPT_ERROR = "isv_interrupt_error";
        public const string ISV_PWD = "ptxt";
        public const string ISV_PWDT = "pwdt";
        public const string ISV_RGN = "rgn";
        public const string ISV_SST = "sst";
        public const string ISV_VID = "vid";
        public const string KEY_SPEECH_TIMEOUT = "speech_timeout";
        public const string LANGUAGE = "language";
        public const string LIB_NAME_32 = "lib_name_32";
        public const string LIB_NAME_64 = "lib_name_64";
        public const string LOCAL_GRAMMAR = "local_grammar";
        public const string MIXED_THRESHOLD = "mixed_threshold";
        public const string MIXED_TIMEOUT = "mixed_timeout";
        public const string MIXED_TYPE = "mixed_type";
        public const string MODE_AUTO = "auto";
        public const string MODE_MSC = "msc";
        public const string MODE_PLUS = "plus";
        public const string NET_TIMEOUT = "timeout";
        public const string NLP_VERSION = "nlp_version";
        public const string PARAMS = "params";
        public const string PITCH = "pitch";
        public const string RESULT_TYPE = "result_type";
        public const string SAMPLE_RATE = "sample_rate";
        public const string SPEED = "speed";
        public const string STREAM_TYPE = "stream_type";
        public const string TEXT_ENCODING = "text_encoding";
        public const string TTS_AUDIO_PATH = "tts_audio_path";
        public const string TTS_BUFFER_EVENT = "tts_buf_event";
        public const string TTS_BUFFER_TIME = "tts_buffer_time";
        public const string TYPE_AUTO = "auto";
        public const string TYPE_CLOUD = "cloud";
        public const string TYPE_LOCAL = "local";
        public const string TYPE_MIX = "mix";
        public const string VAD_BOS = "vad_bos";
        public const string VAD_EOS = "vad_eos";
        public const string VOICE_NAME = "voice_name";
        public const string VOLUME = "volume";
        public const string WAP_PROXY = "wap_proxy";
    }
}
