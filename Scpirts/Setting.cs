using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider music_slider;
    public Slider sound_slider;

    public AudioSource music;
    public AudioSource[] sound;

    private float volume_music;
    private float volume_sound;

    private void Start()
    {
        float savedVolumeM = PlayerPrefs.GetFloat("Music", 1f);
        float savedVolumeS = PlayerPrefs.GetFloat("Sound", 1f);

        SetVolumeM(savedVolumeM);
        SetVolumeS(savedVolumeS);

        if (music_slider != null)
        {
            music_slider.value = savedVolumeM;
            music_slider.onValueChanged.AddListener(SetVolumeM);
        }
        if (sound_slider != null)
        {
            sound_slider.value = savedVolumeS;
            sound_slider.onValueChanged.AddListener(SetVolumeS);
        }



    }

    public void SetVolumeM(float volume)
    {
        if (music != null)
            music.volume = volume;
        PlayerPrefs.SetFloat("Music", volume);
    }
    public void SetVolumeS(float volume)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound != null)
            {
                sound[i].volume = volume;
            }
        }
        PlayerPrefs.SetFloat("Sound", volume);
    }
}
