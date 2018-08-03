﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Profiling;
using UnityEngine.Experimental.PlayerLoop;
using System.Text;

namespace UTJ
{
    public class ProfilingUI : MonoBehaviour
    {
        private Text text;
        private StringBuilder stringBuilderBuffer = new StringBuilder();
        private Recorder recordGfxWaitForPresent;
        private Recorder finishRenderRecord;


        private void Start()
        {
            text = this.GetComponent<Text>();
            recordGfxWaitForPresent = Recorder.Get("Gfx.WaitForPresent");

            finishRenderRecord = Recorder.Get("PostLateUpdate.FinishFrameRendering");
        }
        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % 20 == 0)
            {
                stringBuilderBuffer.Length = 0;
                stringBuilderBuffer.Append("Exec:")
                    .AddMsecFromSec(CustomPlayerLoop.GetLastExecuteTime())
                    .Append("ms\n");

                stringBuilderBuffer.Append("ScriptRunBehaviourUpdate:")
                    .AddMsecFromSec(CustomPlayerLoop.GetProfilingTime<Update.ScriptRunBehaviourUpdate>())
                    .Append("ms\n");

                stringBuilderBuffer.Append("Update.ScriptRunDelayedDynamicFrameRate:")
                    .AddMsecFromSec(CustomPlayerLoop.GetProfilingTime<Update.ScriptRunDelayedDynamicFrameRate>())
                    .Append("ms\n");



                text.text = stringBuilderBuffer.ToString();
            }
        }
    }
    public static class StringBuilderExtention
    {
        public static StringBuilder AddMsecFromSec(this StringBuilder sb, float tm)
        {
            const int num = 4;
            int output = (int)(tm * 1000.0f * GetPow10(num));

            sb.Append(output / GetPow10(num));
            sb.Append(".");
            for (int i = 1; i < num; ++i)
            {
                int tmp = output / GetPow10(num - i) % 10;
                sb.Append(tmp);
            }
            return sb;
        }

        public static StringBuilder AddMsecFromNanosec(this StringBuilder sb, long nanosec)
        {
            float sec = (nanosec / 1000000000.0f);

            return sb.AddMsecFromSec(sec);
        }
        private static int GetPow10(int p)
        {
            int param = 1;
            for (int i = 0; i < p; ++i)
            {
                param *= 10;
            }
            return param;
        }
    }
}
