﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.ComponentModel;

using UESRPG_Character_Manager.Common;

namespace UESRPG_Character_Manager.CharacterComponents
{
    /// <summary>
    /// Embodies a generic Skill.
    /// </summary>
    /// <todo>Skills should be able to calculate their own difficulty.</todo>
    public class Skill : ICloneable, IIdentifiable
    {
        public static uint NextAvailableId { get; set; }

        [XmlAttribute()]
        public string Name { get; set; }
        [XmlAttribute()]
        public int Rank { get; set; }
        public string Description { get; set; }
        public int[] Characteristics { get; set; }
        public bool isDefaultSkill = false;
        [XmlIgnore(), Browsable(false)]
        public uint Id { get; private set; }
        public void ResetId()
        {
            Id = NextAvailableId;
            NextAvailableId++;
        }

        public Skill()
        {
            // Assign each skill a unique ID. This is used for skill editing purposes.
            Id = NextAvailableId;
            NextAvailableId++;
        }

        // A constructor used for duplicating Skill objects in the Clone method
        private Skill(uint skillId)
        {
            Id = skillId;
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            Skill s = new Skill(this.Id);

            s.Name = this.Name;
            s.Rank = this.Rank;
            s.Description = this.Description;
            s.Characteristics = (int[])this.Characteristics.Clone();
            s.isDefaultSkill = this.isDefaultSkill;

            return s;
        }
    }
}
