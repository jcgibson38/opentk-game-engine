﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Config
{
    static class JConfig
    {
        /// <summary>
        /// The maximum binary search recursion count used by JMousePicker.
        /// </summary>
        public static readonly int MOUSE_PICKER_RECURSION_COUNT = 200;

        /// <summary>
        /// The projection length of the Ray used by JMousePicker.
        /// </summary>
        public static readonly float MOUSE_PICKER_RAY_RANGE = 600;

        /// <summary>
        /// 
        /// </summary>
        public static readonly float FOV = 70;

        /// <summary>
        /// 
        /// </summary>
        public static readonly float NEAR_PLANE = 0.1f;

        /// <summary>
        /// 
        /// </summary>
        public static readonly float FAR_PLANE = 1000;

        /// <summary>
        /// 
        /// </summary>
        public static readonly float MAX_TERRAIN_HEIGHT = 50.0f;

        /// <summary>
        /// The translational movement speed of the JCamera.
        /// </summary>
        public static readonly float JCAMERA_MOVEMENT_SPEED = 10.0f;
    }
}
