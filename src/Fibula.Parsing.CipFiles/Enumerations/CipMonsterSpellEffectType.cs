// -----------------------------------------------------------------
// <copyright file="CipMonsterSpellEffectType.cs" company="2Dudes">
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
    /// Enumerates all known CIP monster spell effect types.
    /// </summary>
    public enum CipMonsterSpellEffectType : byte
    {
        /// <summary>
        /// Damage the target.
        /// </summary>
        Damage,

        /// <summary>
        /// Makes the target drunk.
        /// </summary>
        Drunken,

        /// <summary>
        /// Creates magic field(s) at the target area.
        /// </summary>
        Field,

        /// <summary>
        /// Heals the target.
        /// </summary>
        Healing,

        /// <summary>
        /// Changes the target's look.
        /// </summary>
        Outfit,

        /// <summary>
        /// Changes the target's speed.
        /// </summary>
        Speed,

        /// <summary>
        /// Changes the target's strength.
        /// </summary>
        Strength,

        /// <summary>
        /// Summons another monster.
        /// </summary>
        Summon,
    }
}
