// -----------------------------------------------------------------
// <copyright file="CipOutfitType.cs" company="2Dudes">
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
    /// An enumeration of possible outfit types.
    /// </summary>
    public enum CipOutfitType
    {
        /// <summary>
        /// A fully defined outfit.
        /// </summary>
        Outfit,

        /// <summary>
        /// Only defines a race.
        /// </summary>
        Race,

        /// <summary>
        /// The invisible outfit.
        /// </summary>
        Invisible,

        /// <summary>
        /// An outfit that looks like an item.
        /// </summary>
        Item,
    }
}
