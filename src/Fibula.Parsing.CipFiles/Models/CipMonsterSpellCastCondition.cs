// -----------------------------------------------------------------
// <copyright file="CipMonsterSpellCastCondition.cs" company="2Dudes">
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
    /// Class that represents a cast condition in a monster spell rule.
    /// </summary>
    public sealed class CipMonsterSpellCastCondition
    {
        /// <summary>
        /// Gets or sets the type of cast.
        /// </summary>
        public CipMonsterSpellCastType Type { get; set; }

        /// <summary>
        /// Gets or sets the values for the cast condition.
        /// </summary>
        public IEnumerable<long> Values { get; set; }
    }
}
