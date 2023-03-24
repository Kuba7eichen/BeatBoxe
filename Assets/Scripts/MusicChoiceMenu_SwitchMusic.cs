using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChoiceMenu_SwitchMusic : MonoBehaviour
{
    public void SwitchFromMenuMusicToActualSelectedMusic()
    {
        GameManager.Instance.musicAudioSource.clip = GameManager.Instance.Musics[GameManager.Instance.ActualMusicIndex].musicDatas.audioClip;
        GameManager.Instance.musicAudioSource.Play();
    }
}
