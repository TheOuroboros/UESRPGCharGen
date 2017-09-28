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

using UESRPG_Character_Manager.Controllers;

namespace UESRPG_Character_Manager.UI.MainWindow
{
    public partial class MainWindow : Form
    {
        private string _currentFile = "";
        private Character _activeCharacter;

        private const string FILE_TYPE_STRING = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

        public MainWindow ()
        {
            InitializeComponent ();

            // Subscribe Character views to the character change event
            CharacterController.Instance.SelectedCharacterChanged += onSelectedCharacterChanged;
            CharacterController.Instance.ForceUpdate();

            spellDamageView_rollsPage.SelectedSpellChanged += checkRollView_rollsPage.OnSelectedSpellChanged;

            spellListView_statsPage.SpellListChanged += spellDamageView_rollsPage.OnSpellListChanged;

            skillListView_statsPage.SkillListChanged += checkRollView_rollsPage.OnSkillListChanged;

            /*CUSTOM EVENT BINDINGS*/
            this.characterNotesRtb.LostFocus += characterNotesRtb_LostFocus;
            /*END CUSTOM EVENT BINDINGS*/

            saveMi.Enabled = false;
        }

        private void onSelectedCharacterChanged(object sender, EventArgs e)
        {
            _activeCharacter = CharacterController.Instance.ActiveCharacter;
            nameTb.Text = _activeCharacter.Name;
        }

        private void characterNotesRtb_LostFocus(object sender, EventArgs e)
        {
            _activeCharacter.Notes = characterNotesRtb.Text;
        }

        private void nameTb_TextChanged (object sender, EventArgs e)
        {
            _activeCharacter.Name = nameTb.Text;
        }

        /// <summary>
        /// This function is bound to our NumericUpDowns' GotFocus event. It will cause the numeric values to be highlighted when tabbed into.
        /// </summary>
        private void numberBoxFocus (object sender, EventArgs e)
        {
            NumericUpDown theNb = (NumericUpDown)sender;
            theNb.Select (0, 3);
        }

        private void saveMi_Click (object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentFile))
            {
                string message;
                bool result = CharacterController.Instance.SaveChar(_currentFile, out message);

                if(!result)
                {
                    MessageBox.Show(message);
                }
            }
        }

        private void saveAsMi_Click (object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".xml",
                Filter = FILE_TYPE_STRING
            };
            DialogResult sfdResult = sfd.ShowDialog ();

            if (sfdResult == DialogResult.OK)
            {
                string message;
                bool result = CharacterController.Instance.SaveChar(sfd.FileName, out message);

                if (result)
                {
                    _currentFile = sfd.FileName;
                    saveMi.Enabled = true;
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
        }

        private void loadMi_Click (object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = ".xml",
                Filter = FILE_TYPE_STRING
            };
            DialogResult ofdResult = ofd.ShowDialog();

            if (ofdResult == DialogResult.OK)
            {
                string message;
                bool result = CharacterController.Instance.LoadChar(ofd.FileName, out message);

                if (result)
                {
                    _currentFile = ofd.FileName;
                    saveMi.Enabled = true;
                    nameTb.Text = _activeCharacter.Name;
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
        }

        private void notesCommitChangesBt_Click(object sender, EventArgs e)
        {
            _activeCharacter.Notes = characterNotesRtb.Text;
        }
    }
}
