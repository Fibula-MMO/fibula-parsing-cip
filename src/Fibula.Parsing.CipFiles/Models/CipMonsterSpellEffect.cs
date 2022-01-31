// -----------------------------------------------------------------
// <copyright file="CipMonsterSpellEffect.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Parsing.CipFiles.Models
{
    using System.Collections.Generic;
    using Fibula.Parsing.CipFiles.Enumerations;

    /// <summary>
    /// Class that represents a monster spell effect in a monster spell rule.
    /// </summary>
    public sealed class CipMonsterSpellEffect
    {
        /// <summary>
        /// Gets or sets the type of effect.
        /// </summary>
        public CipMonsterSpellEffectType Type { get; set; }

        /// <summary>
        /// Gets or sets the values for the effect.
        /// </summary>
        public IEnumerable<uint> Values { get; set; }
    }
}
