﻿using System;
using UnityEngine;

namespace Legacy
{
    public class MPatrol : MBehaviour
    {
        public Monster Monster { get; private set; }
        public float MoveTime { get; set; }
        public float Speed { get; set; }
        private float StartTime { get; set; }

        public MPatrol(Monster monster, string animationName, float speed = 0f, float moveTime = 1f, params Action[] actions)
        {
            Monster = monster;
            AnimationName = animationName;
            MoveTime = moveTime;
            Speed = speed;

            foreach (var action in actions)
            {
                Update += action;
            }

            Start += PatrolStart;
            Update += PatrolUpdate;
        }

        private void PatrolStart()
        {
            StartTime = Time.time;
        }

        private void PatrolUpdate()
        {
            var curSclae = Monster.transform.localScale;

            if (Time.time - StartTime >= MoveTime)
            {
                StartTime = Time.time;
                Monster.transform.localScale = new Vector3(-curSclae.x, curSclae.y, curSclae.z);

                //Monster.Renderer.flipX = !Monster.Renderer.flipX;
            }

            int direction = curSclae.x < 0f ? -1 : 1;

            Monster.RB.velocity = new Vector2(Speed * direction * Time.deltaTime, Monster.RB.velocity.y);
        }
    }
}