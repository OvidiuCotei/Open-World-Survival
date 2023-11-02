using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace OvyKode.Timeline.Subtitles
{
    public class SubtitleTrackMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Text text = playerData as Text;
            string currentText = string.Empty;
            float currentAlpha = 0f;

            if(!text)
            {
                return;
            }

            int inputCount = playable.GetInputCount();

            for(int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if(inputWeight > 0f)
                {
                    ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                    SubtitleBehaviour input = inputPlayable.GetBehaviour();
                    currentText = input.subtitleText;
                    currentAlpha = inputWeight;
                }
            }

            text.text = currentText;
            text.color = new Color(1, 1, 1, currentAlpha);
        }
    }
}