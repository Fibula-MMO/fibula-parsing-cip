// -----------------------------------------------------------------
// <copyright file="CipMonsterSpellCastType.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Parsing.CipFiles.Enumerations
{
    /// <summary>
    /// Enumerates all known CIP monster spell cast types.
    /// </summary>
    public enum CipMonsterSpellCastType : byte
    {
        /// <summary>
        /// Casting on itself.
        /// </summary>
        Actor,

        /// <summary>
        /// Casting towards a given direction, with a distance and spread (cone).
        /// </summary>
        Angle,

        /// <summary>
        /// Casting directly at a destination location.
        /// </summary>
        Destination,

        /// <summary>
        /// Casting at the monster location.
        /// </summary>
        Origin,

        /// <summary>
        /// Casting on the victim.
        /// </summary>
        Victim,
    }
}
