using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClipInfo clips;
}

public class AudioClipInfo{
    public string name;         // 오디오의 이름
    public AudioClip clip;      // 오디오 클립
}