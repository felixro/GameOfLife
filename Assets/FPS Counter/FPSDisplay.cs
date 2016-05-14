using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour 
{
    public Text fpsLabel;

    FPSCounter fpsCounter;
    string notEnoughDataDisplay = "---";

    [System.Serializable]
    struct FPSColor
    {
        public Color color;
        public int minimumFps;
    }

    [SerializeField]
    FPSColor[] fpsColors;

    void Awake()
    {
        fpsCounter = GetComponent<FPSCounter>();
    }

    void Update()
    {
        Display(fpsLabel, fpsCounter.AverageFPS);
    }

    void Display(Text label, int fps)
    {
        if ( fpsCounter.AverageFPS == -1 )
        {
            label.text = notEnoughDataDisplay;
        }else
        {
            label.text = fpsCounter.AverageFPS.ToString();   
        }

        for (int i = 0; i < fpsColors.Length; i++)
        {
            if ( fps >= fpsColors[i].minimumFps)
            {
                label.color = fpsColors[i].color;
                break;
            }
        }
    }
}