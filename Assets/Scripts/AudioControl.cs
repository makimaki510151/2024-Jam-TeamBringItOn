using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{

    private readonly List<AudioSource> bgmSourceList = new();
    private readonly List<AudioSource> seSourceList = new();

    private readonly int numberOfBgm = 3;
    private readonly int numberOfSe = 20;

    private float bgmVol = 1.0f;
    private float seVol = 1.0f;

    public static AudioControl Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        for (int i = 0; i < numberOfBgm; i++)
        {
            GameObject go_ = new();
            AudioSource as_;

            go_.name = "BGM:" + (i).ToString();
            as_ = go_.AddComponent<AudioSource>();
            as_.playOnAwake = false;
            as_.loop = true;
            as_.spatialBlend = 0f;
            bgmSourceList.Add(as_);

            go_.transform.parent = transform;
        }
        for (int i = 0; i < numberOfSe; i++)
        {
            GameObject go_ = new();
            AudioSource as_;

            go_.name = "SE:" + (i).ToString();
            as_ = go_.AddComponent<AudioSource>();
            as_.playOnAwake = false;
            as_.spatialBlend = 0f;
            seSourceList.Add(as_);

            go_.transform.parent = transform;
        }
        Instance = this;
    }
    public void SetBGMVol(float f_)
    {
        for (int i = 0; bgmSourceList.Count > i; i++)
        {
            bgmSourceList[i].volume = f_;

        }
        bgmVol = f_;
    }
    public void SetSEVol(float f_)
    {
        seVol = f_;
    }
    public float GetBGMVol()
    {
        return bgmVol;
    }
    public float GetSEVol()
    {
        return seVol;
    }

    public void PlaySE(AudioClip audioClip, Transform t_ = null)
    {
        Vector3 pos;
        float blend = 0;
        if (!t_)
        {
            pos = Camera.main.transform.position;
        }
        else
        {
            pos = t_.position;
            blend = 0.7f;
        }

        for (int i = 0; i < seSourceList.Count; i++)
        {
            if (!seSourceList[i].isPlaying)
            {
                pos.z = Camera.main.transform.position.z;
                seSourceList[i].transform.position = pos;
                seSourceList[i].clip = audioClip;
                seSourceList[i].spatialBlend = blend;
                seSourceList[i].volume = seVol;
                seSourceList[i].Play();
                break;
            }
        }
    }
    public void PlayBGM(AudioClip audioClip, bool cover = false, bool loop = false)
    {
        if (!cover)
        {
            if (bgmSourceList[0].clip != audioClip || !bgmSourceList[0].isPlaying)
            {
                bgmSourceList[0].transform.position = Camera.main.transform.position;
                bgmSourceList[0].clip = audioClip;
                bgmSourceList[0].spatialBlend = 0;
                bgmSourceList[0].volume = bgmVol;
                bgmSourceList[0].Play();
            }
        }
        else
        {
            for (int i = 0; i < bgmSourceList.Count; i++)
            {
                if (!bgmSourceList[i].isPlaying)
                {
                    bgmSourceList[i].transform.position = Camera.main.transform.position;
                    bgmSourceList[i].clip = audioClip;
                    bgmSourceList[i].spatialBlend = 0;
                    bgmSourceList[i].volume = bgmVol;
                    bgmSourceList[i].loop = loop;
                    bgmSourceList[i].Play();
                }
            }
        }
    }
    public void StopBGM()
    {
        for (int i = 0; i < bgmSourceList.Count; i++)
        {
            bgmSourceList[i].Stop();
        }
    }
    public void StopSE()
    {
        for (int i = 0; i < seSourceList.Count; i++)
        {
            seSourceList[i].Stop();
        }
    }
}
