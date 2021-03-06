﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using GameLogic;

using CodeTimeProfiler = Helpers.Profiler;


namespace GameLogic
{
    public class SpawnPoint : MonoBehaviour
    {
        public int AreaCheckRadius;

        public int EntitiesNearby { get; private set; }
        public bool PlayerNearby { get; private set; }
        public int EnemyEntitiesNearby { get; private set; }

        private int Counter;

        private int CheckFrequency;

        private GameManager Manager;
        
        public void Start()
        {
            UpdateSurroundingAreaInformation();
            Counter = 0;
            CheckFrequency = 30;


            Manager = GameManagerLocator.Manager;
        }

        public void Update()
        {
            Counter++;
            if(Counter == CheckFrequency)
            {
                UpdateSurroundingAreaInformation();

                Counter = 0;
            }
        }

        public void Spawn()
        {
            throw new NotImplementedException();
        }

        private void UpdateSurroundingAreaInformation()
        {
            RaycastHit[] Hits = Physics.SphereCastAll(transform.position, AreaCheckRadius, Vector3.forward, 0.001f);

            int nearbyEntities = 0;
            bool playerNearby = false;
            int enemyEntitiesNearby = 0;

            if(Hits.Length > 0)
            {
                for(int i = 0; i < Hits.Length; ++i)
                {
                    GameObject HitObject = Hits[i].collider.gameObject;

                    if(HitObject.tag == "GameEntity")
                    {
                        nearbyEntities++;
                        EntityTag TagManager = GetComponent<EntityTag>() as EntityTag;

                        if (TagManager.Is(Tag.Enemy))
                            enemyEntitiesNearby++;

                        if (TagManager.Is(Tag.Player))
                            playerNearby = true;
                    }
                }

                PlayerNearby = playerNearby;
                EnemyEntitiesNearby = enemyEntitiesNearby;
            }
        }
    }
}