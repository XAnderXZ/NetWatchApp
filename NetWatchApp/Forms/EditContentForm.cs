﻿using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class EditContentForm : Form
    {
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblReleaseYear;
        private System.Windows.Forms.NumericUpDown numReleaseYear;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.ComboBox cmbPlatform;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.PictureBox picContentImage;
        private System.Windows.Forms.Label lblImageUrl;
        private System.Windows.Forms.TextBox txtImageUrl;
        private System.Windows.Forms.Button btnPreviewImage;
        private System.Windows.Forms.Panel pnlEpisodes;
        private System.Windows.Forms.Label lblEpisodes;
        private System.Windows.Forms.DataGridView dgvEpisodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeDuration;
        private System.Windows.Forms.Button btnAddEpisode;
        private System.Windows.Forms.Button btnEditEpisode;
        private System.Windows.Forms.Button btnDeleteEpisode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private readonly ContentRepository _contentRepository;
        private List<Episode> _episodes = new List<Episode>();
        private Content _content;
        private int _contentId;
        private string _selectedImageUrl = null;
        private string _currentImageUrl = null;
        private bool _imageChanged = false;

        public EditContentForm(int contentId)
        {
            InitializeComponent();
            _contentRepository = new ContentRepository(new Data.EntityFramework.NetWatchDbContext());
            _contentId = contentId;

            // Populate genre combobox
            cmbGenre.Items.AddRange(new object[] {
              "Action", "Adventure", "Comedy", "Drama", "Fantasy",
              "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller", "Documentary",
              "Animation", "Crime", "Biography", "Family", "History", "Musical", "Western"
          });

            // Populate content type combobox
            cmbType.Items.AddRange(new object[] { "Movie", "Series" });

            // Populate platform combobox
            cmbPlatform.Items.AddRange(new object[] {
              "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu",
              "Apple TV+", "Peacock", "Paramount+", "YouTube Premium", "Crunchyroll", "Other"
          });

            // Set up event handlers
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
            btnAddEpisode.Click += BtnAddEpisode_Click;
            btnEditEpisode.Click += BtnEditEpisode_Click;
            btnDeleteEpisode.Click += BtnDeleteEpisode_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnPreviewImage.Click += BtnPreviewImage_Click;

            // Load content data
            LoadContent();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblReleaseYear = new System.Windows.Forms.Label();
            this.numReleaseYear = new System.Windows.Forms.NumericUpDown();
            this.lblGenre = new System.Windows.Forms.Label();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.cmbPlatform = new System.Windows.Forms.ComboBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.lblImage = new System.Windows.Forms.Label();
            this.picContentImage = new System.Windows.Forms.PictureBox();
            this.lblImageUrl = new System.Windows.Forms.Label();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.btnPreviewImage = new System.Windows.Forms.Button();
            this.pnlEpisodes = new System.Windows.Forms.Panel();
            this.lblEpisodes = new System.Windows.Forms.Label();
            this.dgvEpisodes = new System.Windows.Forms.DataGridView();
            this.colEpisodeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddEpisode = new System.Windows.Forms.Button();
            this.btnEditEpisode = new System.Windows.Forms.Button();
            this.btnDeleteEpisode = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(38, 20);
            this.lblTitle.Text = "Title:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(150, 20);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(300, 27);

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 60);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(85, 20);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(150, 60);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(300, 100);

            // lblReleaseYear
            this.lblReleaseYear.AutoSize = true;
            this.lblReleaseYear.Location = new System.Drawing.Point(20, 170);
            this.lblReleaseYear.Name = "lblReleaseYear";
            this.lblReleaseYear.Size = new System.Drawing.Size(93, 20);
            this.lblReleaseYear.Text = "Release Year:";

            // numReleaseYear
            this.numReleaseYear.Location = new System.Drawing.Point(150, 170);
            this.numReleaseYear.Maximum = 2100;
            this.numReleaseYear.Minimum = 1900;
            this.numReleaseYear.Name = "numReleaseYear";
            this.numReleaseYear.Size = new System.Drawing.Size(150, 27);
            this.numReleaseYear.Value = DateTime.Now.Year;

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(20, 210);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(48, 20);
            this.lblGenre.Text = "Genre:";

            // cmbGenre
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(150, 210);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(200, 28);

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(20, 250);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(43, 20);
            this.lblType.Text = "Type:";

            // cmbType
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(150, 250);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(200, 28);

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(20, 290);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(68, 20);
            this.lblPlatform.Text = "Platform:";

            // cmbPlatform
            this.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlatform.FormattingEnabled = true;
            this.cmbPlatform.Location = new System.Drawing.Point(150, 290);
            this.cmbPlatform.Name = "cmbPlatform";
            this.cmbPlatform.Size = new System.Drawing.Size(200, 28);

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(20, 330);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(124, 20);
            this.lblDuration.Text = "Duration (minutes):";

            // numDuration
            this.numDuration.Location = new System.Drawing.Point(150, 330);
            this.numDuration.Maximum = 1000;
            this.numDuration.Minimum = 1;
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(150, 27);
            this.numDuration.Value = 90;

            // lblImage
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(20, 370);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(51, 20);
            this.lblImage.Text = "Image:";

            // picContentImage
            this.picContentImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picContentImage.Location = new System.Drawing.Point(150, 370);
            this.picContentImage.Name = "picContentImage";
            this.picContentImage.Size = new System.Drawing.Size(150, 200);
            this.picContentImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picContentImage.TabIndex = 0;
            this.picContentImage.TabStop = false;

            // lblImageUrl
            this.lblImageUrl.AutoSize = true;
            this.lblImageUrl.Location = new System.Drawing.Point(20, 580);
            this.lblImageUrl.Name = "lblImageUrl";
            this.lblImageUrl.Size = new System.Drawing.Size(82, 20);
            this.lblImageUrl.Text = "Image URL:";

            // txtImageUrl
            this.txtImageUrl.Location = new System.Drawing.Point(150, 580);
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.Size = new System.Drawing.Size(300, 27);
            this.txtImageUrl.PlaceholderText = "Enter URL to image";

            // btnPreviewImage
            this.btnPreviewImage.Location = new System.Drawing.Point(460, 580);
            this.btnPreviewImage.Name = "btnPreviewImage";
            this.btnPreviewImage.Size = new System.Drawing.Size(100, 27);
            this.btnPreviewImage.Text = "Preview";
            this.btnPreviewImage.UseVisualStyleBackColor = true;

            // pnlEpisodes
            this.pnlEpisodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEpisodes.Controls.Add(this.lblEpisodes);
            this.pnlEpisodes.Controls.Add(this.dgvEpisodes);
            this.pnlEpisodes.Controls.Add(this.btnAddEpisode);
            this.pnlEpisodes.Controls.Add(this.btnEditEpisode);
            this.pnlEpisodes.Controls.Add(this.btnDeleteEpisode);
            this.pnlEpisodes.Location = new System.Drawing.Point(20, 610);
            this.pnlEpisodes.Name = "pnlEpisodes";
            this.pnlEpisodes.Size = new System.Drawing.Size(430, 200);
            this.pnlEpisodes.Visible = false;

            // lblEpisodes
            this.lblEpisodes.AutoSize = true;
            this.lblEpisodes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEpisodes.Location = new System.Drawing.Point(10, 10);
            this.lblEpisodes.Name = "lblEpisodes";
            this.lblEpisodes.Size = new System.Drawing.Size(70, 20);
            this.lblEpisodes.Text = "Episodes:";

            // dgvEpisodes
            this.dgvEpisodes.AllowUserToAddRows = false;
            this.dgvEpisodes.AllowUserToDeleteRows = false;
            this.dgvEpisodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEpisodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
              this.colEpisodeNumber,
              this.colEpisodeTitle,
              this.colEpisodeDuration
          });
            this.dgvEpisodes.Location = new System.Drawing.Point(10, 40);
            this.dgvEpisodes.Name = "dgvEpisodes";
            this.dgvEpisodes.ReadOnly = true;
            this.dgvEpisodes.RowHeadersWidth = 51;
            this.dgvEpisodes.Size = new System.Drawing.Size(410, 110);
            this.dgvEpisodes.TabIndex = 0;

            // colEpisodeNumber
            this.colEpisodeNumber.HeaderText = "Number";
            this.colEpisodeNumber.MinimumWidth = 6;
            this.colEpisodeNumber.Name = "colEpisodeNumber";
            this.colEpisodeNumber.ReadOnly = true;
            this.colEpisodeNumber.Width = 70;

            // colEpisodeTitle
            this.colEpisodeTitle.HeaderText = "Title";
            this.colEpisodeTitle.MinimumWidth = 6;
            this.colEpisodeTitle.Name = "colEpisodeTitle";
            this.colEpisodeTitle.ReadOnly = true;
            this.colEpisodeTitle.Width = 200;

            // colEpisodeDuration
            this.colEpisodeDuration.HeaderText = "Duration";
            this.colEpisodeDuration.MinimumWidth = 6;
            this.colEpisodeDuration.Name = "colEpisodeDuration";
            this.colEpisodeDuration.ReadOnly = true;
            this.colEpisodeDuration.Width = 80;

            // btnAddEpisode
            this.btnAddEpisode.Location = new System.Drawing.Point(10, 160);
            this.btnAddEpisode.Name = "btnAddEpisode";
            this.btnAddEpisode.Size = new System.Drawing.Size(110, 30);
            this.btnAddEpisode.Text = "Add";
            this.btnAddEpisode.UseVisualStyleBackColor = true;

            // btnEditEpisode
            this.btnEditEpisode.Location = new System.Drawing.Point(130, 160);
            this.btnEditEpisode.Name = "btnEditEpisode";
            this.btnEditEpisode.Size = new System.Drawing.Size(110, 30);
            this.btnEditEpisode.Text = "Edit";
            this.btnEditEpisode.UseVisualStyleBackColor = true;

            // btnDeleteEpisode
            this.btnDeleteEpisode.Location = new System.Drawing.Point(250, 160);
            this.btnDeleteEpisode.Name = "btnDeleteEpisode";
            this.btnDeleteEpisode.Size = new System.Drawing.Size(110, 30);
            this.btnDeleteEpisode.Text = "Delete";
            this.btnDeleteEpisode.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(150, 830);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(270, 830);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // EditContentForm
            this.ClientSize = new System.Drawing.Size(580, 880);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblReleaseYear);
            this.Controls.Add(this.numReleaseYear);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.cmbGenre);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.cmbPlatform);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.numDuration);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.picContentImage);
            this.Controls.Add(this.lblImageUrl);
            this.Controls.Add(this.txtImageUrl);
            this.Controls.Add(this.btnPreviewImage);
            this.Controls.Add(this.pnlEpisodes);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditContentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Content";
        }

        private void LoadContent()
        {
            try
            {
                _content = _contentRepository.GetById(_contentId);
                if (_content == null)
                {
                    MessageBox.Show("Content not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                // Fill form with content data
                txtTitle.Text = _content.Title;
                txtDescription.Text = _content.Description;
                numReleaseYear.Value = _content.ReleaseYear;
                cmbGenre.SelectedItem = _content.Genre;
                cmbType.SelectedItem = _content.Type;
                cmbPlatform.SelectedItem = _content.Platform;

                if (_content.Type == "Movie")
                {
                    numDuration.Value = _content.Duration;
                    pnlEpisodes.Visible = false;
                }
                else
                {
                    pnlEpisodes.Visible = true;
                    _episodes = _content.Episodes.ToList();
                    RefreshEpisodesGrid();
                }

                // Load image if available
                _currentImageUrl = _content.ImagePath;
                txtImageUrl.Text = _currentImageUrl;
                if (!string.IsNullOrEmpty(_currentImageUrl))
                {
                    try
                    {
                        using (var httpClient = new System.Net.Http.HttpClient())
                        {
                            var imageData = httpClient.GetByteArrayAsync(_currentImageUrl).Result;
                            using (var ms = new System.IO.MemoryStream(imageData))
                            {
                                picContentImage.Image = Image.FromStream(ms);
                            }
                        }
                        _selectedImageUrl = _currentImageUrl;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Adjust form height based on content type
                bool isSeries = _content.Type == "Series";
                this.ClientSize = new Size(580, isSeries ? 880 : 680);
                btnSave.Location = new Point(150, isSeries ? 830 : 630);
                btnCancel.Location = new Point(270, isSeries ? 830 : 630);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSeries = cmbType.SelectedItem.ToString() == "Series";
            pnlEpisodes.Visible = isSeries;
            lblDuration.Visible = !isSeries;
            numDuration.Visible = !isSeries;

            // Adjust form height based on content type
            this.ClientSize = new Size(580, isSeries ? 880 : 680);
            btnSave.Location = new Point(150, isSeries ? 830 : 630);
            btnCancel.Location = new Point(270, isSeries ? 830 : 630);

            // Clear episodes if switching from series to movie
            if (!isSeries)
            {
                _episodes.Clear();
                RefreshEpisodesGrid();
            }
        }

        private void BtnPreviewImage_Click(object sender, EventArgs e)
        {
            string imageUrl = txtImageUrl.Text.Trim();
            if (string.IsNullOrEmpty(imageUrl))
            {
                MessageBox.Show("Please enter an image URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Clear current image
                if (picContentImage.Image != null)
                {
                    picContentImage.Image.Dispose();
                    picContentImage.Image = null;
                }

                // Download and display image
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var imageData = httpClient.GetByteArrayAsync(imageUrl).Result;
                    using (var ms = new System.IO.MemoryStream(imageData))
                    {
                        picContentImage.Image = Image.FromStream(ms);
                    }
                }

                _selectedImageUrl = imageUrl;
                _imageChanged = true;
                MessageBox.Show("Image loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _imageChanged = false;
            }
        }

        private void BtnAddEpisode_Click(object sender, EventArgs e)
        {
            using (var episodeForm = new AddEpisodeForm(_episodes.Count > 0 ? _episodes.Max(ep => ep.EpisodeNumber) + 1 : 1))
            {
                if (episodeForm.ShowDialog() == DialogResult.OK)
                {
                    _episodes.Add(episodeForm.Episode);
                    RefreshEpisodesGrid();
                }
            }
        }

        private void BtnEditEpisode_Click(object sender, EventArgs e)
        {
            if (dgvEpisodes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an episode to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int episodeNumber = Convert.ToInt32(dgvEpisodes.SelectedRows[0].Cells[0].Value);
            Episode episode = _episodes.FirstOrDefault(ep => ep.EpisodeNumber == episodeNumber);

            if (episode != null)
            {
                using (var episodeForm = new EditEpisodeForm(episode))
                {
                    if (episodeForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update episode in the list
                        int index = _episodes.FindIndex(ep => ep.EpisodeNumber == episodeNumber);
                        _episodes[index] = episodeForm.Episode;
                        RefreshEpisodesGrid();
                    }
                }
            }
        }

        private void BtnDeleteEpisode_Click(object sender, EventArgs e)
        {
            if (dgvEpisodes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an episode to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int episodeNumber = Convert.ToInt32(dgvEpisodes.SelectedRows[0].Cells[0].Value);

            if (MessageBox.Show("Are you sure you want to delete this episode?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _episodes.RemoveAll(ep => ep.EpisodeNumber == episodeNumber);
                RefreshEpisodesGrid();
            }
        }

        private void RefreshEpisodesGrid()
        {
            dgvEpisodes.Rows.Clear();
            foreach (var episode in _episodes.OrderBy(e => e.EpisodeNumber))
            {
                dgvEpisodes.Rows.Add(episode.EpisodeNumber, episode.Title, episode.Duration);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    // Update content properties
                    _content.Title = txtTitle.Text.Trim();
                    _content.Description = txtDescription.Text.Trim();
                    _content.ReleaseYear = (int)numReleaseYear.Value;
                    _content.Genre = cmbGenre.SelectedItem.ToString();
                    _content.Type = cmbType.SelectedItem.ToString();
                    _content.Platform = cmbPlatform.SelectedItem.ToString();

                    // Update image URL if changed
                    if (_imageChanged && !string.IsNullOrEmpty(_selectedImageUrl))
                    {
                        _content.ImagePath = _selectedImageUrl;
                    }

                    if (_content.Type == "Movie")
                    {
                        _content.Duration = (int)numDuration.Value;
                        _content.Episodes.Clear();
                    }
                    else
                    {
                        _content.Duration = 0;

                        // Create a new list to avoid reference issues
                        var updatedEpisodes = new List<Episode>();

                        foreach (var episode in _episodes)
                        {
                            // Create new episode objects to avoid reference issues
                            var updatedEpisode = new Episode
                            {
                                Id = episode.Id,
                                ContentId = _content.Id,
                                EpisodeNumber = episode.EpisodeNumber,
                                Title = episode.Title,
                                Duration = episode.Duration
                            };

                            updatedEpisodes.Add(updatedEpisode);
                        }

                        // Clear and add all episodes
                        _content.Episodes.Clear();
                        foreach (var episode in updatedEpisodes)
                        {
                            _content.Episodes.Add(episode);
                        }
                    }

                    _contentRepository.Update(_content);
                    MessageBox.Show("Content updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbGenre.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a genre.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbPlatform.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a platform.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbType.SelectedItem.ToString() == "Series" && _episodes.Count == 0)
            {
                MessageBox.Show("Please add at least one episode for the series.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtImageUrl.Text.Trim()))
            {
                MessageBox.Show("Please enter an image URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                if (picContentImage.Image != null)
                {
                    picContentImage.Image.Dispose();
                    picContentImage.Image = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}

