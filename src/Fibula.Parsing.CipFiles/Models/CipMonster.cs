// -----------------------------------------------------------------
// <copyright file="CipMonster.cs" company="2Dudes">
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
    using Fibula.Definitions.Enumerations;
    using Fibula.Parsing.CipFiles.Enumerations;

    /// <summary>
    /// Class that represents a monster.
    /// </summary>
    public sealed class CipMonster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CipMonster"/> class.
        /// </summary>
        public CipMonster()
        {
            this.Flags = new List<CipCreatureFlag>();
            this.Skills = new List<(string Name, uint DefaultLevel, uint CurrentLevel, uint MaximumLevel, uint TargetCount, uint CountIncreaseFactor, byte IncreaserPerLevel)>();
            this.Spells = new List<(CipMonsterSpellCastCondition condition, CipMonsterSpellEffect effect, byte chance)>();
            this.Inventory = new List<(ushort typeId, byte maxAmount, ushort dropChance)>();
            this.Phrases = new List<string>();
        }

        /// <summary>
        /// Gets or sets the monster's race identifier.
        /// </summary>
        public uint RaceId { get; set; }

        /// <summary>
        /// Gets or sets the monster's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the article to use when describing the monster.
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Gets or sets the monster's outfit.
        /// </summary>
        public CipOutfit Outfit { get; set; }

        /// <summary>
        /// Gets or sets the monster's corpse.
        /// </summary>
        public uint Corpse { get; set; }

        /// <summary>
        /// Gets or sets the monster's blood type.
        /// </summary>
        public BloodType BloodType { get; set; }

        /// <summary>
        /// Gets or sets the monster's experience yield.
        /// </summary>
        public uint Experience { get; set; }

        /// <summary>
        /// Gets or sets the monster's cost to summon.
        /// </summary>
        public ushort SummonCost { get; set; }

        /// <summary>
        /// Gets or sets the monster's fleeing threshold.
        /// </summary>
        public ushort FleeThreshold { get; set; }

        /// <summary>
        /// Gets or sets the monster's attack power.
        /// </summary>
        public ushort Attack { get; set; }

        /// <summary>
        /// Gets or sets the monster's defense power.
        /// </summary>
        public ushort Defense { get; set; }

        /// <summary>
        /// Gets or sets the monster's armor rating.
        /// </summary>
        public ushort Armor { get; set; }

        /// <summary>
        /// Gets or sets the monster's poison effect.
        /// </summary>
        public ushort Poison { get; set; }

        /// <summary>
        /// Gets or sets the monster's target lost threshold.
        /// </summary>
        public byte LoseTarget { get; set; }

        /// <summary>
        /// Gets or sets the monster's strategy.
        /// </summary>
        public (byte Closest, byte Weakest, byte Strongest, byte Random) Strategy { get; set; }

        /// <summary>
        /// Gets or sets the monster's creature flags.
        /// </summary>
        public IList<CipCreatureFlag> Flags { get; set; }

        /// <summary>
        /// Gets or sets the monster's skills.
        /// </summary>
        public IList<(string Name, uint DefaultLevel, uint CurrentLevel, uint MaximumLevel, uint TargetCount, uint CountIncreaseFactor, byte IncreaserPerLevel)> Skills { get; set; }

        /// <summary>
        /// Gets or sets the monster's spells.
        /// </summary>
        public IList<(CipMonsterSpellCastCondition condition, CipMonsterSpellEffect effect, byte chance)> Spells { get; set; }

        /// <summary>
        /// Gets or sets the monster's inventory.
        /// </summary>
        public IList<(ushort typeId, byte maxAmount, ushort dropChance)> Inventory { get; set; }

        /// <summary>
        /// Gets or sets the monster's phrases.
        /// </summary>
        public IList<string> Phrases { get; set; }
    }
}
