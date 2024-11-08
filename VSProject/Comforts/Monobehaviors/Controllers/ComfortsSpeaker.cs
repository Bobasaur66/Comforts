﻿using Comforts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Comforts.Monobehaviors.Controllers
{
    public class ComfortsSpeaker : MonoBehaviour
    {
        public static List<GameObject> allJukeboxes = new List<GameObject>();
        public static List<GameObject> allSpeakers = new List<GameObject>();

        private ComfortJukeboxController currentJukebox;
        private FMOD_CustomLoopingEmitter fmodEmitter
        {
            get
            {
                return gameObject.GetComponent<FMOD_CustomLoopingEmitter>();
            }
        }




        public static void UpdateMusicConstructables()
        {
            if (allJukeboxes.Count > 0)
            {
                foreach (GameObject obj in allJukeboxes)
                {
                    obj.GetComponent<ComfortJukeboxController>().Update();
                }
            }
            if (allSpeakers.Count > 0)
            {
                foreach (GameObject obj in allSpeakers)
                {
                    ComfortsSpeaker speaker = obj.GetComponent<ComfortsSpeaker>();

                    speaker.fmodEmitter.Stop();
                    speaker.currentJukebox = speaker.FindNearestJukebox().GetComponent<ComfortJukeboxController>();
                    speaker.Update();
                }
            }
        }
        

        private GameObject FindNearestJukebox()
        {
            return ComfortUtils.FindNearestGameObjectFromList(transform.position, allJukeboxes);
        }

        public void Update()
        {
            if (currentJukebox == null)
            {
                if (fmodEmitter.asset != null)
                {
                    fmodEmitter.asset = null;
                }
                if (fmodEmitter.playing)
                {
                    fmodEmitter.Stop();
                }
            }
            else
            {
                if (fmodEmitter.asset != currentJukebox.currentSong)
                {
                    fmodEmitter.asset = currentJukebox.currentSong;
                }
                if (currentJukebox.playSong)
                {
                    if (!fmodEmitter.playing)
                    {
                        fmodEmitter.Play();
                    }
                }
                else
                {
                    if (fmodEmitter.playing)
                    {
                        fmodEmitter.Stop();
                    }
                }
            }
        }

        public void Start()
        {
            allSpeakers.Add(gameObject);

            UpdateMusicConstructables();
        }

        public void OnDestroy()
        {
            allSpeakers.Remove(gameObject);
        }
    }
}
