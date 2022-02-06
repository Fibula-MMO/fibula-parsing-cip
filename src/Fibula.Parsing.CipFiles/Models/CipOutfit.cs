// -----------------------------------------------------------------
// <copyright file="CipOutfit.cs" company="2Dudes">
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
    using Fibula.Parsing.CipFiles.Enumerations;

    /// <summary>
    /// Class that represents an outfit from CipSoft files.
    /// </summary>
    public sealed class CipOutfit
    {
        /// <summary>
        /// Gets or sets the type of this outfit.
        /// </summary>
        public CipOutfitType Type { get; set; }

        /// <summary>
        /// Gets or sets this outfit's id.
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets this outfit's head value.
        /// </summary>
        public byte Head { get; set; }

        /// <summary>
        /// Gets or sets this outfit's body value.
        /// </summary>
        public byte Body { get; set; }

        /// <summary>
        /// Gets or sets this outfit's legs value.
        /// </summary>
        public byte Legs { get; set; }

        /// <summary>
        /// Gets or sets this outfit's feet value.
        /// </summary>
        public byte Feet { get; set; }
    }
}
