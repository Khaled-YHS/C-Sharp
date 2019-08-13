﻿using System;

namespace Algorithms.Search.AStar
{

    /// <summary>
    /// A pathfinding exception is thrown when the Pathfinder encounters a critical error and can not continue.
    /// </summary>
    public class PathfindingException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PathfindingException(string message)
            : base(message)
        {

        }
    }
}
