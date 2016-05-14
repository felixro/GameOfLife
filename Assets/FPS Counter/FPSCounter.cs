using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour {

    public int AverageFPS { get; private set; }
    public int frameRange = 120;
	
    int[] fpsBuffer;
    int fpsBufferIndex;
    bool hasEnoughData = false;

	void Update () 
    {
        if ( fpsBuffer == null || fpsBuffer.Length != frameRange )
        {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
	}

    void InitializeBuffer ( )
    {
        if ( frameRange <= 0 )
        {
            frameRange = 1;
        }
        fpsBuffer = new int[frameRange];
        fpsBufferIndex = 0;
    }

    void CalculateFPS()
    {
        int sum = 0;
        for (int i = 0; i < frameRange; i++)
        {
            if ( ! hasEnoughData )
            {
                // not enough data yet
                AverageFPS = -1;
                return;
            }
            sum += fpsBuffer[i];
        }

        AverageFPS = (int)((float)sum/frameRange);
    }

    void UpdateBuffer()
    {
        fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if ( fpsBufferIndex >= frameRange )
        {
            fpsBufferIndex = 0;
            hasEnoughData = true;
        }
    }
}
