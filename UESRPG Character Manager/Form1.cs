﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace UESRPG_Character_Manager
{
    public partial class Form1 : Form
    {
        private List<Character> _characterList;
        private Character _selectedChar;
        private int _selectedIndex = 0;

        private bool _isLoading = false;

        public Form1 ()
        {
            InitializeComponent ();
            _characterList = new List<Character> ();
            _selectedChar = new Character ();
            _characterList.Add (_selectedChar);

            charactersCb.Items.Add (_selectedChar.Name);
            charactersCb.SelectedIndex = 0;
            comboBox1.DataSource = ArmorLocationsData.Names; //TODO How do you rename these to something sensible?
            comboBox2.DataSource = ArmorQualityData.Names;
            comboBox3.DataSource = ArmorTypeData.Names;
            comboBox4.DataSource = ArmorMaterialData.Names;

            foreach (string characteristic in Characteristics.CharacteristicNames)
            {
                characteristicCb.Items.Add (characteristic);
            }

            characteristicCb.SelectedIndex = 0;
        }

        private Character SelectedCharacter ()
        {
            return _characterList[_selectedIndex];
        }

        private void nameTb_TextChanged (object sender, EventArgs e)
        {
            SelectedCharacter().Name = nameTb.Text;
            charactersCb.Items[_selectedIndex] = nameTb.Text;
        }

        /// <summary>
        /// A characteristic changed, so we will update all the character's values/mods and re-calculate the calculated fields.
        /// This function will do nothing if Characteristics are currently being updated somewhere else.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void characteristicChanged (object sender, EventArgs e)
        {
            if (!_isLoading)
            {
                _isLoading = true;
                SelectedCharacter ().Strength = (int)nbStrength.Value;
                SelectedCharacter ().Endurance = (int)nbEndurance.Value;
                SelectedCharacter ().Agility = (int)nbAgility.Value;
                SelectedCharacter ().Intelligence = (int)nbIntelligence.Value;
                SelectedCharacter ().Willpower = (int)nbWillpower.Value;
                SelectedCharacter ().Perception = (int)nbPerception.Value;
                SelectedCharacter ().Personality = (int)nbPersonality.Value;
                SelectedCharacter ().Luck = (int)nbLuck.Value;

                int health = 0;
                int.TryParse (healthTb.Text, out health);
                SelectedCharacter ().CurrentHealth = health;
                int stamina = 0;
                int.TryParse (staminaTb.Text, out stamina);
                SelectedCharacter ().CurrentStamina = stamina;
                int magicka = 0;
                int.TryParse (magickaTb.Text, out magicka);
                SelectedCharacter ().CurrentMagicka = magicka;
                int luck = 0;
                int.TryParse (luckPointsTb.Text, out luck);
                SelectedCharacter ().CurrentLuckPoints = luck;
                int ap = 0;
                int.TryParse (actionPointsTb.Text, out ap);
                SelectedCharacter ().CurrentAp = ap;

                SelectedCharacter ().HealthMod = (int)nbModHealth.Value;
                SelectedCharacter ().WoundThresholdMod = (int)nbModWoundThreshold.Value;
                SelectedCharacter ().StaminaMod = (int)nbModStamina.Value;
                SelectedCharacter ().MagickaMod = (int)nbModMagicka.Value;
                SelectedCharacter ().ActionPointsMod = (int)nbModActionPoints.Value;
                SelectedCharacter ().MovementRatingMod = (int)nbModMovementRating.Value;
                SelectedCharacter ().CarryRatingMod = (int)nbModCarryRating.Value;
                SelectedCharacter ().InitiativeRatingMod = (int)nbModInitiativeRating.Value;
                SelectedCharacter ().DamageBonusMod = (int)nbModDamageBonus.Value;
                SelectedCharacter ().LuckPointsMod = (int)nbModLuck.Value;

                updateEverything ();
                _isLoading = false;
            }
        }

        /// <summary>
        /// characteristicLoaded will update the UI representation of the character to match its data.
        /// This function will do nothing if Characteristics are currently being updated somewhere else.
        /// </summary>
        private void characteristicLoaded ()
        {
            if (!_isLoading)
            {
                _isLoading = true;
                nbStrength.Value = SelectedCharacter ().Strength;
                nbEndurance.Value = SelectedCharacter ().Endurance;
                nbAgility.Value = SelectedCharacter ().Agility;
                nbIntelligence.Value = SelectedCharacter ().Intelligence;
                nbWillpower.Value = SelectedCharacter ().Willpower;
                nbPerception.Value = SelectedCharacter ().Perception;
                nbPersonality.Value = SelectedCharacter ().Personality;
                nbLuck.Value = SelectedCharacter ().Luck;

                healthTb.Text = SelectedCharacter ().CurrentHealth.ToString();
                staminaTb.Text = SelectedCharacter ().CurrentStamina.ToString();
                magickaTb.Text = SelectedCharacter ().CurrentMagicka.ToString ();
                luckPointsTb.Text = SelectedCharacter ().CurrentLuckPoints.ToString ();
                actionPointsTb.Text = SelectedCharacter ().CurrentAp.ToString ();

                nbModHealth.Value = SelectedCharacter ().HealthMod;
                nbModWoundThreshold.Value = SelectedCharacter ().WoundThresholdMod;
                nbModStamina.Value = SelectedCharacter ().StaminaMod;
                nbModMagicka.Value = SelectedCharacter ().MagickaMod;
                nbModActionPoints.Value = SelectedCharacter ().ActionPointsMod;
                nbModMovementRating.Value = SelectedCharacter ().MovementRatingMod;
                nbModCarryRating.Value = SelectedCharacter ().CarryRatingMod;
                nbModInitiativeRating.Value = SelectedCharacter ().InitiativeRatingMod;
                nbModDamageBonus.Value = SelectedCharacter ().DamageBonusMod;
                nbModLuck.Value = SelectedCharacter ().LuckPointsMod;

                updateEverything ();
                _isLoading = false;
            }
        }

        /// <summary>
        /// Updates all calculated fields.
        /// </summary>
        private void updateEverything ()
        {
            maxLuckPointsTb.Text = "" + (SelectedCharacter().MaximumLuckPoints);
            initiativeRatingTb.Text = "" + (SelectedCharacter().InitiativeRating);
            maxActionPointsTb.Text = "" + (SelectedCharacter().MaximumAp);
            maxStaminaTb.Text = "" + (SelectedCharacter().Stamina);
            maxMagickaTb.Text = "" + (SelectedCharacter().MagickaPool);
            movementRatingTb.Text = "" + (SelectedCharacter().MovementRating);
            carryRatingTb.Text = "" + (SelectedCharacter().CarryRating);
            woundThresholdTb.Text = "" + (SelectedCharacter().WoundThreshold);
            maxHealthTb.Text = "" + (SelectedCharacter().MaxHealth);
            damageBonusTb.Text = "" + (SelectedCharacter().DamageBonus);
            dataGridView1.DataSource = null;
            if (SelectedCharacter().ArmorPieces.Count > 0)
            {
              dataGridView1.DataSource = SelectedCharacter().ArmorPieces;
            }
        }

        /// <summary>
        /// Will disable the skillLevel textbox if a skill roll is not selected, and perform a soft roll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skillRb_CheckedChanged (object sender, EventArgs e)
        {
            bool isActive = skillRb.Checked;
            skillLevelTb.Enabled = isActive;

            softRoll (sender, e);
        }

        /// <summary>
        /// Will perform a soft roll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void characteristicRb_CheckedChanged (object sender, EventArgs e)
        {
            softRoll (sender, e);
        }

        /// <summary>
        /// Rolls against selected parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rollBt_Click (object sender, EventArgs e)
        {
            bool isSkillRoll = skillRb.Checked;

            Random r = new Random ();

            int characteristicIndex = characteristicCb.SelectedIndex;
            int characteristic = SelectedCharacter().GetCharacteristic(characteristicIndex);

            int result = r.Next (0, 100);
            rollResultTb.Text = "" + result;

            int difference = 0;

            if (isSkillRoll)
            {
                int skillLevel = 0;
                int.TryParse (skillLevelTb.Text, out skillLevel);

                characteristic = (characteristic + (skillLevel * 10));
            }

            difference = (characteristic - result);

            int successes = SelectedCharacter().GetBonus (difference);

            rollBreakdownTb.Text = String.Format ("{0} - {1} = {2}", characteristic, result, difference);
            rollSuccessesTb.Text = "" + successes;
        }

        /// <summary>
        /// A soft roll performs all of the calculations of a regular roll, but does not change the roll result.
        /// This is useful for using outside roll values with the app, since we don't want to replace the player's
        /// entered roll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void softRoll (object sender, EventArgs e)
        {
            bool isSkillRoll = skillRb.Checked;

            Random r = new Random ();

            int characteristicIndex = characteristicCb.SelectedIndex;
            int characteristic = SelectedCharacter().GetCharacteristic(characteristicIndex);

            int result = 0;
            if (int.TryParse (rollResultTb.Text, out result))
            {
                int difference = 0;

                if (isSkillRoll)
                {
                    int skillLevel = 0;
                    int.TryParse (skillLevelTb.Text, out skillLevel);

                    characteristic = (characteristic + (skillLevel * 10));
                }

                difference = (characteristic - result);

                int successes = SelectedCharacter().GetBonus (difference);

                rollBreakdownTb.Text = String.Format ("{0} - {1} = {2}", characteristic, result, difference);
                rollSuccessesTb.Text = "" + successes;
            }
        }

        private void button1_Click (object sender, EventArgs e)
        {
            SaveChar ();
            LoadChar ();
        }

        /// <summary>
        /// This function is bound to our NumericUpDowns' GotFocus event. It will cause the numeric values to be highlighted when tabbed into.
        /// </summary>
        private void NumberBoxFocus (object sender, EventArgs e)
        {
            NumericUpDown theNb = (NumericUpDown)sender;
            theNb.Select (0, 3);
        }

        private void SaveChar ()
        {
            XmlSerializer xml = new XmlSerializer (typeof (List<Character>));
            FileStream fs = new FileStream ("char.xml", FileMode.Create);
            xml.Serialize (fs, _characterList);
            fs.Close ();
        }

        private void LoadChar ()
        {
            XmlSerializer xml = new XmlSerializer (typeof (List<Character>));
            FileStream fs = new FileStream ("char.xml", FileMode.Open);
            _characterList = (List<Character>)xml.Deserialize (fs);
            fs.Close ();
            characteristicLoaded ();

            nameTb.Text = SelectedCharacter().Name;
            charactersCb.Items.Clear ();
            foreach (Character c in _characterList)
            {
                charactersCb.Items.Add (c.Name);
            }
        }

        private void btAddCharacter_Click (object sender, EventArgs e)
        {
            Character newChar = new Character ();
            _characterList.Add (newChar);
            charactersCb.Items.Add (newChar.Name);
        }

        private void charactersCb_SelectedIndexChanged (object sender, EventArgs e)
        {
            _selectedIndex = charactersCb.SelectedIndex;
            nameTb.Text = SelectedCharacter().Name;
            characteristicLoaded ();
        }

        private void saveBt_Click (object sender, EventArgs e)
        {
            SaveChar ();
        }

        private void loadBt_Click (object sender, EventArgs e)
        {
            LoadChar ();
        }

        private void healthTb_TextChanged (object sender, EventArgs e)
        {

        }

        private void staminaTb_TextChanged (object sender, EventArgs e)
        {

        }

        private void magickaTb_TextChanged (object sender, EventArgs e)
        {

        }

        private void actionPointsTb_TextChanged (object sender, EventArgs e)
        {

        }

        private void luckPointsTb_TextChanged (object sender, EventArgs e)
        {

        }

    private void label32_Click(object sender, EventArgs e)
    {

    }

    private void button1_Click_1(object sender, EventArgs e)
    {
      // here we create new armor object
      double ar = Armor.CalculateAR((ArmorTypes)comboBox3.SelectedIndex,(ArmorMaterials)comboBox4.SelectedIndex,(ArmorQualitys) comboBox2.SelectedIndex);
      SelectedCharacter().AddArmorPiece(new Armor(textBox1.Text, ar, 0, 0, (ArmorLocations)comboBox1.SelectedIndex, null));
      System.Console.WriteLine("After Adding new armor piece");
      updateEverything();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
