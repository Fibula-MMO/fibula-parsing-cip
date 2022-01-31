// -----------------------------------------------------------------
// <copyright file="CipFileParser.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Parsing.CipFiles
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fibula.Definitions.Enumerations;
    using Fibula.Parsing.CipFiles.Enumerations;
    using Fibula.Parsing.CipFiles.Models;
    using Fibula.Parsing.Contracts.Abstractions;
    using Fibula.Utilities.Common.Extensions;
    using Fibula.Utilities.Validation;
    using Sprache;

    /// <summary>
    /// Static class that parses Cip files.
    /// </summary>
    public static class CipFileParser
    {
        /// <summary>
        /// The comma character.
        /// </summary>
        public const char Comma = ',';

        /// <summary>
        /// The space character.
        /// </summary>
        public const char Space = ' ';

        /// <summary>
        /// The equals sign character.
        /// </summary>
        public const char EqualsSign = '=';

        /// <summary>
        /// The open parenthesis character.
        /// </summary>
        public const char OpenParenthesis = '(';

        /// <summary>
        /// The close parenthesis character.
        /// </summary>
        public const char CloseParenthesis = ')';

        /// <summary>
        /// The open curly brace character.
        /// </summary>
        public const char OpenCurly = '{';

        /// <summary>
        /// The close curly brace character.
        /// </summary>
        public const char CloseCurly = '}';

        /// <summary>
        /// The name of the content attribute.
        /// </summary>
        public const string ContentAttributeName = "Content";

        /// <summary>
        /// Attempts to parse a string into a <see cref="CipElement"/>.
        /// </summary>
        /// <param name="inputStr">The input string.</param>
        /// <returns>A collection of <see cref="CipElement"/>s parsed.</returns>
        public static IEnumerable<CipElement> Parse(string inputStr)
        {
            if (string.IsNullOrWhiteSpace(inputStr))
            {
                return null;
            }

            var enclosingChars = new List<(char, char)> { (OpenCurly, CloseCurly), (OpenParenthesis, CloseParenthesis) };

            inputStr = inputStr.Trim(Space); // remove extra leading and trailing spaces.
            inputStr = inputStr.TrimEnclosures(enclosingChars);

            var enclosures = inputStr.GetEnclosedStrings(enclosingChars);
            var pendingContent = new Stack<IParsedAttribute>();

            var root = new CipElement(-1, new List<IParsedAttribute>() { new CipAttribute() });

            // root is guaranteed to have at least one attribute.
            pendingContent.Push(root.Attributes.First());

            foreach (var enclosure in enclosures)
            {
                // comma separate but watch for strings in quotes ("").
                var elements = enclosure.SplitByToken(Comma).Select(ParseElement).ToList();

                var attribute = pendingContent.Pop();

                foreach (var element in elements)
                {
                    foreach (var attr in element.Attributes.Where(a => a.Name.Equals(ContentAttributeName)))
                    {
                        pendingContent.Push(attr);
                    }
                }

                attribute.Value = elements;
            }

            return root.Attributes.First().Value as IEnumerable<CipElement>;
        }

        /// <summary>
        /// Reads a <see cref="CipMonster"/> model out of a monster file.
        /// </summary>
        /// <param name="monsterFileInfo">The information about the monster file.</param>
        /// <returns>The <see cref="CipMonster"/> model loaded.</returns>
        public static CipMonster ParseMonsterFile(FileInfo monsterFileInfo)
        {
            monsterFileInfo.ThrowIfNull(nameof(monsterFileInfo));

            if (!monsterFileInfo.Exists)
            {
                return null;
            }

            var monsterModel = new CipMonster();

            foreach ((string name, string value) in BreakMonsterFileInDataTuples(File.ReadLines(monsterFileInfo.FullName)))
            {
                switch (name)
                {
                    case "racenumber":
                        monsterModel.RaceId = Convert.ToUInt32(value);
                        break;
                    case "name":
                        monsterModel.Name = value;
                        break;
                    case "article":
                        monsterModel.Article = value;
                        break;
                    case "outfit":
                        var (lookTypeId, headColor, bodyColor, legsColor, feetColor) = CipFileParser.ParseMonsterOutfit(value);

                        monsterModel.Outfit = new CipOutfit()
                        {
                            Type = lookTypeId == 0 ? CipOutfitType.Invisible :
                                   headColor + bodyColor + legsColor + feetColor == 0 ? CipOutfitType.Race :
                                   CipOutfitType.Outfit,
                            Values = new int[] { lookTypeId, headColor, bodyColor, legsColor, feetColor },
                        };
                        break;
                    case "corpse":
                        monsterModel.Corpse = Convert.ToUInt16(value);
                        break;
                    case "blood":
                        if (Enum.TryParse(value, out BloodType bloodType))
                        {
                            monsterModel.BloodType = bloodType;
                        }

                        break;
                    case "experience":
                        monsterModel.Experience = Convert.ToUInt32(value);
                        break;
                    case "summoncost":
                        monsterModel.SummonCost = Convert.ToUInt16(value);
                        break;
                    case "fleethreshold":
                        monsterModel.FleeThreshold = Convert.ToUInt16(value);
                        break;
                    case "attack":
                        monsterModel.Attack = Convert.ToUInt16(value);
                        break;
                    case "defend":
                        monsterModel.Defense = Convert.ToUInt16(value);
                        break;
                    case "armor":
                        monsterModel.Armor = Convert.ToUInt16(value);
                        break;
                    case "poison":
                        // monsterType.SetConditionInfect(ConditionType.Posioned, Convert.ToUInt16(value));
                        break;
                    case "losetarget":
                        monsterModel.LoseTarget = Convert.ToByte(value);
                        break;
                    case "strategy":
                        monsterModel.Strategy = CipFileParser.ParseMonsterStrategy(value);
                        break;
                    case "flags":
                        var parsedElements = CipFileParser.Parse(value);

                        foreach (var element in parsedElements)
                        {
                            if (!element.IsFlag || element.Attributes == null || !element.Attributes.Any())
                            {
                                continue;
                            }

                            if (Enum.TryParse(element.Attributes.First().Name, out CipCreatureFlag flagMatch))
                            {
                                monsterModel.Flags.Add(flagMatch);
                            }
                        }

                        break;
                    case "skills":
                        monsterModel.Skills = CipFileParser.ParseMonsterSkills(value).ToList();

                        break;
                    case "spells":
                        monsterModel.Spells = CipFileParser.ParseMonsterSpells(value).ToList();

                        break;
                    case "inventory":
                        monsterModel.Inventory = CipFileParser.ParseMonsterInventory(value).ToList();

                        break;
                    case "talk":
                        monsterModel.Phrases = CipFileParser.ParsePhrases(value).ToList();

                        break;
                }
            }

            return monsterModel;
        }

        /// <summary>
        /// Parses a monster outfit out of a string.
        /// </summary>
        /// <param name="outfitStr">The string to parse.</param>
        /// <returns>A tuple containing the monster outfit values.</returns>
        public static (ushort lookTypeId, byte headColor, byte bodyColor, byte legsColor, byte feetColor) ParseMonsterOutfit(string outfitStr)
        {
            return CipGrammar.MonsterOutfit.Parse(outfitStr);
        }

        /// <summary>
        /// Parses monster spells out of a string.
        /// </summary>
        /// <param name="spellsStr">The string to parse.</param>
        /// <returns>A collection of data containing the spell conditions, effects and an asociated chance.</returns>
        public static IEnumerable<(CipMonsterSpellCastCondition condition, CipMonsterSpellEffect effects, byte chance)> ParseMonsterSpells(string spellsStr)
        {
            return CipGrammar.MonsterSpellRules.Parse(spellsStr);
        }

        /// <summary>
        /// Parses a monster's possible inventory items out of a string.
        /// </summary>
        /// <param name="inventoryStr">The string to parse.</param>
        /// <returns>A collection of tuples containing the possible inventory items of the monster.</returns>
        public static IEnumerable<(ushort typeId, byte maxAmount, ushort dropChance)> ParseMonsterInventory(string inventoryStr)
        {
            return CipGrammar.MonsterInventory.Parse(inventoryStr);
        }

        /// <summary>
        /// Parses a monster's skills out of a string.
        /// </summary>
        /// <param name="skillsStr">The string to parse.</param>
        /// <returns>A collection of tuples containing the skills information.</returns>
        public static IEnumerable<(string, ushort, ushort, ushort, uint, uint, byte)> ParseMonsterSkills(string skillsStr)
        {
            return CipGrammar.MonsterSkills.Parse(skillsStr);
        }

        /// <summary>
        /// Parses a monster's strategy values out of a string.
        /// </summary>
        /// <param name="strategyStr">The string to parse.</param>
        /// <returns>A tuple containing the monster strategy chances.</returns>
        public static (byte closest, byte lowestHp, byte mostDmgDealt, byte random) ParseMonsterStrategy(string strategyStr)
        {
            return CipGrammar.MonsterStrategy.Parse(strategyStr);
        }

        /// <summary>
        /// Parses a creature's phrases out of a string.
        /// </summary>
        /// <param name="phrasesStr">The string to parse.</param>
        /// <returns>A collection of strings, the phrases of the creature.</returns>
        public static IEnumerable<string> ParsePhrases(string phrasesStr)
        {
            return CipGrammar.CreaturePhrases.Parse(phrasesStr);
        }

        /// <summary>
        /// Parses an element string value into a <see cref="CipElement"/>.
        /// </summary>
        /// <param name="elementStr">The element string.</param>
        /// <returns>The new instance of <see cref="CipElement"/>.</returns>
        private static CipElement ParseElement(string elementStr)
        {
            elementStr.ThrowIfNullOrWhiteSpace(nameof(elementStr));

            var attrs = elementStr.SplitByToken();
            var attributesList = attrs as IList<string> ?? attrs.ToList();
            var hasIdData = int.TryParse(attributesList.FirstOrDefault(), out int intValue);

            IList<IParsedAttribute> attributes = attributesList.Skip(hasIdData ? 1 : 0).Select(ParseAttribute).ToList();

            var element = new CipElement(hasIdData ? intValue : -1, attributes);

            return element;
        }

        /// <summary>
        /// Parses an attribute string value into a <see cref="IParsedAttribute"/>.
        /// </summary>
        /// <param name="attributeStr">The attribute string.</param>
        /// <returns>The new instance of <see cref="IParsedAttribute"/>.</returns>
        private static IParsedAttribute ParseAttribute(string attributeStr)
        {
            IParsedAttribute attribute = new CipAttribute();

            var sections = attributeStr.Split(new[] { EqualsSign }, 2);

            if (sections.Length < 2)
            {
                attribute.Name = sections[0].EndsWith(EqualsSign) ? sections[0][0..^1] : sections[0];
            }
            else
            {
                attribute.Name = sections[0];
                attribute.Value = int.TryParse(sections[1], out int numericValue) ? (object)numericValue : sections[1];
            }

            return attribute;
        }

        /// <summary>
        /// Reads data out of multiple lines in the input files.
        /// </summary>
        /// <param name="fileLines">The file's lines.</param>
        /// <returns>A collection of mappings of properties names to values.</returns>
        private static IEnumerable<(string propName, string propValue)> BreakMonsterFileInDataTuples(IEnumerable<string> fileLines)
        {
            fileLines.ThrowIfNull(nameof(fileLines));

            const char CommentSymbol = '#';
            const char PropertyValueSeparator = '=';

            var propName = string.Empty;
            var propData = string.Empty;

            foreach (var readLine in fileLines)
            {
                var inLine = readLine.TrimStart();

                // ignore comments and empty lines.
                if (string.IsNullOrWhiteSpace(inLine) || inLine.StartsWith(CommentSymbol))
                {
                    continue;
                }

                var data = inLine.Split(new[] { PropertyValueSeparator }, 2);

                if (data.Length > 2)
                {
                    throw new InvalidDataException($"Malformed line [{inLine}] in monster file.");
                }

                if (data.Length == 1)
                {
                    // line is a continuation of the last prop.
                    propData += data[0].Trim();
                }
                else
                {
                    if (propName.Length > 0 && propData.Length > 0)
                    {
                        yield return (propName, propData);
                    }

                    propName = data[0].ToLower().Trim();
                    propData = data[1].Trim();
                }
            }

            if (propName.Length > 0 && propData.Length > 0)
            {
                yield return (propName, propData);
            }
        }
    }
}
