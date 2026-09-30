using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DosingCalculator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private TextBox txtLiters;

        private TextBox txtStartCa, txtStartMg, txtStartKh;
        private TextBox txtTargetCa, txtTargetMg, txtTargetKh;

        private TextBox txtDays;
        private ComboBox cmbProfile;

        private Label lblTotalP1, lblTotalP2, lblTotalP3, lblTotalP4;
        private Label lblDailyP1, lblDailyP2, lblDailyP3, lblDailyP4;

        private Button btnCalc;

        private readonly Dictionary<string, (double p1, double p2, double p3, double p4)> _profiles = new Dictionary<string, (double p1, double p2, double p3, double p4)>
          {
                { "Mixed Reef – Exceptional / Good (5-10-2.5-2.5)", (5, 10, 2.5, 2.5) },
                { "Mixed Reef – Great / Great (4-8-2-2)", (4, 8, 2, 2) },
                { "Mixed Reef – Good / Exceptional (3-6-1.5-1.5)", (3, 6, 1.5, 1.5) },

                { "SPS Dominant – Exceptional / Good (7-14-3.5-3.5)", (7, 14, 3.5, 3.5) },
                { "SPS Dominant – Great / Great (6-12-3-3)", (6, 12, 3, 3) },
                { "SPS Dominant – Good / Exceptional (3-6-1.5-1.5)", (3, 6, 1.5, 1.5) },

                { "Frags – Exceptional / Good (7-14-3.5-3.5)", (7, 14, 3.5, 3.5) },

                { "ULNS – Good / Exceptional (3-6-1.5-1.5)", (3, 6, 1.5, 1.5) },
          };

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 600);
            this.Text = "Complete Reef Care - 4-Part Dosing Calculator";
            BuildUi();
        }

        private void BuildUi()
        {
            int left = 20;
            int top = 20;

            Controls.Add(new Label { Left = left, Top = top, Width = 220, Text = "Water volume (liters):" });
            txtLiters = new TextBox { Left = left + 230, Top = top - 3, Width = 120, Text = "100" };
            Controls.Add(txtLiters);

            // Aquarium profile selector (ratios)
            Controls.Add(new Label { Left = left + 380, Top = top, Width = 120, Text = "Reef profile:" });
            cmbProfile = new ComboBox
            {
                Left = left + 500,
                Top = top - 4,
                Width = 320,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var key in _profiles.Keys) cmbProfile.Items.Add(key);
            cmbProfile.SelectedIndex = 1; // default: Mixed Reef – Great/Great
            Controls.Add(cmbProfile);

            top += 45;

            Controls.Add(new Label { Left = left, Top = top, Width = 200, Text = "Current values" });
            Controls.Add(new Label { Left = left + 260, Top = top, Width = 200, Text = "Target values" });
            top += 30;

            // CA
            Controls.Add(new Label { Left = left, Top = top, Width = 70, Text = "Ca (ppm)" });
            txtStartCa = new TextBox { Left = left + 80, Top = top - 3, Width = 120, Text = "" };
            txtTargetCa = new TextBox { Left = left + 260, Top = top - 3, Width = 120, Text = "450" };
            Controls.Add(txtStartCa);
            Controls.Add(txtTargetCa);

            top += 35;

            // Mg (fields retained; not used in calculations)
            Controls.Add(new Label { Left = left, Top = top, Width = 70, Text = "Mg (ppm)" });
            txtStartMg = new TextBox { Left = left + 80, Top = top - 3, Width = 120, Text = "" };
            txtTargetMg = new TextBox { Left = left + 260, Top = top - 3, Width = 120, Text = "1350" };
            Controls.Add(txtStartMg);
            Controls.Add(txtTargetMg);

            top += 35;

            // KH
            Controls.Add(new Label { Left = left, Top = top, Width = 70, Text = "KH (dKH)" });
            txtStartKh = new TextBox { Left = left + 80, Top = top - 3, Width = 120, Text = "" };
            txtTargetKh = new TextBox { Left = left + 260, Top = top - 3, Width = 120, Text = "8.5" };
            Controls.Add(txtStartKh);
            Controls.Add(txtTargetKh);

            top += 45;

            // Number of days
            Controls.Add(new Label { Left = left, Top = top, Width = 220, Text = "Spread over (whole days):" });
            txtDays = new TextBox { Left = left + 230, Top = top - 3, Width = 120, Text = "4" };
            Controls.Add(txtDays);

            btnCalc = new Button
            {
                Left = left + 380,
                Top = top - 5,
                Width = 140,
                Height = 34,
                Text = "Calculate"
            };
            btnCalc.Click += BtnCalc_Click;
            Controls.Add(btnCalc);

            top += 55;

            // Results - totals
            lblTotalP1 = new Label { Left = left, Top = top, Width = 800, Text = "Part #1 (Ca) total: -" };
            top += 24;
            lblTotalP2 = new Label { Left = left, Top = top, Width = 800, Text = "Part #2 (KH) total: -" };
            top += 24;
            lblTotalP3 = new Label { Left = left, Top = top, Width = 800, Text = "Part #3 (Iodine - Potassium) total (proportional): -" };
            top += 24;
            lblTotalP4 = new Label { Left = left, Top = top, Width = 800, Text = "Part #4 (Trace) total (proportional): -" };
            top += 34;

            Controls.Add(lblTotalP1);
            Controls.Add(lblTotalP2);
            Controls.Add(lblTotalP3);
            Controls.Add(lblTotalP4);

            // Results - daily doses
            lblDailyP1 = new Label { Left = left, Top = top, Width = 800, Text = "Daily Part #1 (Ca): -" };
            top += 24;
            lblDailyP2 = new Label { Left = left, Top = top, Width = 800, Text = "Daily Part #2 (KH): -" };
            top += 24;
            lblDailyP3 = new Label { Left = left, Top = top, Width = 800, Text = "Daily Part #3 (Iodine - Potassium): -" };
            top += 24;
            lblDailyP4 = new Label { Left = left, Top = top, Width = 800, Text = "Daily Part #4 (Trace): -" };

            Controls.Add(lblDailyP1);
            Controls.Add(lblDailyP2);
            Controls.Add(lblDailyP3);
            Controls.Add(lblDailyP4);

            top += 40;

            Controls.Add(new Label
            {
                Left = left,
                Top = top,
                Width = 820,
                Height = 100,
                ForeColor = Color.DimGray,
                Text =
                    "Calculation basis:\n" +
                    "Part #1: 1 ml / 100 L = +1.4 ppm Ca\n" +
                    "Part #2: 1 ml / 100 L = +0.1 dKH\n" +
                    "Parts #3 and #4: proportional to Part #1 using the selected profile ratios.\n" +
                    "Mg values are not used to calculate a separate correction dose."
            });
        }

        private void BtnCalc_Click(object sender, EventArgs e)
        {
            if (!TryParseDouble(txtLiters.Text, out double liters) || liters <= 0)
            {
                MessageBox.Show("Enter a valid water volume in liters (a positive number).");
                return;
            }

            if (!int.TryParse(txtDays.Text.Trim(), out int days) || days <= 0)
            {
                MessageBox.Show("Enter a valid number of days (a positive whole number).");
                return;
            }

            if (!TryParseDouble(txtStartCa.Text, out double startCa) ||
                !TryParseDouble(txtTargetCa.Text, out double targetCa) ||
                !TryParseDouble(txtStartKh.Text, out double startKh) ||
                !TryParseDouble(txtTargetKh.Text, out double targetKh))
            {
                MessageBox.Show("Enter valid numbers in the current and target Ca and KH fields.");
                return;
            }

            if (cmbProfile.SelectedItem == null)
            {
                MessageBox.Show("Select an aquarium profile.");
                return;
            }

            var profileKey = cmbProfile.SelectedItem.ToString();
            var ratios = _profiles[profileKey]; // p1,p2,p3,p4

            double deltaCa = targetCa - startCa; // ppm
            double deltaKh = targetKh - startKh; // dKH

            // Based on the Red Sea Complete Reef Care manual:
            // Part #1: 1 ml / 100 L -> +1.4 ppm Ca
            // Part #2: 1 ml / 100 L -> +0.1 dKH
            double mlP1 = 0;
            double mlP2 = 0;
            string warnings = "";

            if (deltaCa > 0)
                mlP1 = (deltaCa / 1.4) * (liters / 100.0);
            else if (deltaCa < 0)
                warnings += "Target Ca is below the current value. No Part #1 correction dose is calculated.\n";

            if (deltaKh > 0)
                mlP2 = (deltaKh / 0.1) * (liters / 100.0);
            else if (deltaKh < 0)
                warnings += "Target KH is below the current value. No Part #2 correction dose is calculated.\n";

            // Parts #3 and #4: proportional to Part #1 using the profile table ratios
            // e.g. 4-8-2-2 => p3/p1 = 0.5, p4/p1 = 0.5
            double mlP3 = (ratios.p1 > 0) ? mlP1 * (ratios.p3 / ratios.p1) : 0;
            double mlP4 = (ratios.p1 > 0) ? mlP1 * (ratios.p4 / ratios.p1) : 0;

            // Daily breakdown
            double dailyP1 = mlP1 / days;
            double dailyP2 = mlP2 / days;
            double dailyP3 = mlP3 / days;
            double dailyP4 = mlP4 / days;

            lblTotalP1.Text = $"Part #1 (Ca) total: {mlP1:0.00} ml";
            lblTotalP2.Text = $"Part #2 (KH) total: {mlP2:0.00} ml";
            lblTotalP3.Text = $"Part #3 total (proportional): {mlP3:0.00} ml";
            lblTotalP4.Text = $"Part #4 total (proportional): {mlP4:0.00} ml";

            lblDailyP1.Text = $"Daily Part #1: {dailyP1:0.00} ml / day  (total days: {days})";
            lblDailyP2.Text = $"Daily Part #2: {dailyP2:0.00} ml / day  (total days: {days})";
            lblDailyP3.Text = $"Daily Part #3: {dailyP3:0.00} ml / day  (total days: {days})";
            lblDailyP4.Text = $"Daily Part #4: {dailyP4:0.00} ml / day  (total days: {days})";

            if (!string.IsNullOrWhiteSpace(warnings))
                MessageBox.Show(warnings.Trim(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool TryParseDouble(string s, out double value)
        {
            s = (s ?? "").Trim().Replace(',', '.');
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        #endregion
    }
}

