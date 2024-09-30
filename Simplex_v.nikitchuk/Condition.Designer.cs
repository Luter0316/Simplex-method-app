namespace Simplex_v.nikitchuk
{
    public partial class Condition
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.CBMethod = new System.Windows.Forms.ComboBox();
            this.NaturalRB = new System.Windows.Forms.RadioButton();
            this.DecimalRB = new System.Windows.Forms.RadioButton();
            this.dataGridViewRestrictions = new System.Windows.Forms.DataGridView();
            this.RestrictionsCount = new System.Windows.Forms.NumericUpDown();
            this.dataGridViewTargetFunction = new System.Windows.Forms.DataGridView();
            this.ClearButton = new System.Windows.Forms.Button();
            this.LabelRestrictionsCount = new System.Windows.Forms.Label();
            this.VariableCount = new System.Windows.Forms.NumericUpDown();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.LabelVariableCount = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRestrictions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RestrictionsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTargetFunction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariableCount)).BeginInit();
            this.SuspendLayout();
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1314, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // CBMethod
            // 
            this.CBMethod.FormattingEnabled = true;
            this.CBMethod.Location = new System.Drawing.Point(12, 133);
            this.CBMethod.Name = "CBMethod";
            this.CBMethod.Size = new System.Drawing.Size(279, 21);
            this.CBMethod.TabIndex = 27;
            // 
            // NaturalRB
            // 
            this.NaturalRB.AutoSize = true;
            this.NaturalRB.Location = new System.Drawing.Point(15, 105);
            this.NaturalRB.Name = "NaturalRB";
            this.NaturalRB.Size = new System.Drawing.Size(136, 17);
            this.NaturalRB.TabIndex = 26;
            this.NaturalRB.TabStop = true;
            this.NaturalRB.Text = "Обыкновенные дроби";
            this.NaturalRB.UseVisualStyleBackColor = true;
            // 
            // DecimalRB
            // 
            this.DecimalRB.AutoSize = true;
            this.DecimalRB.Location = new System.Drawing.Point(15, 85);
            this.DecimalRB.Name = "DecimalRB";
            this.DecimalRB.Size = new System.Drawing.Size(121, 17);
            this.DecimalRB.TabIndex = 25;
            this.DecimalRB.TabStop = true;
            this.DecimalRB.Text = "Десятичные дроби";
            this.DecimalRB.UseVisualStyleBackColor = true;
            // 
            // dataGridViewRestrictions
            // 
            this.dataGridViewRestrictions.AllowUserToAddRows = false;
            this.dataGridViewRestrictions.AllowUserToDeleteRows = false;
            this.dataGridViewRestrictions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRestrictions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewRestrictions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRestrictions.Location = new System.Drawing.Point(338, 78);
            this.dataGridViewRestrictions.MaximumSize = new System.Drawing.Size(965, 380);
            this.dataGridViewRestrictions.MinimumSize = new System.Drawing.Size(965, 380);
            this.dataGridViewRestrictions.Name = "dataGridViewRestrictions";
            this.dataGridViewRestrictions.Size = new System.Drawing.Size(965, 380);
            this.dataGridViewRestrictions.TabIndex = 24;
            this.dataGridViewRestrictions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRestrictions_CellContentClick);
            this.dataGridViewRestrictions.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewRestrictions_EditingControlShowing);
            this.dataGridViewRestrictions.SelectionChanged += new System.EventHandler(this.dataGridViewRestrictions_SelectionChanged);
            // 
            // RestrictionsCount
            // 
            this.RestrictionsCount.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestrictionsCount.Location = new System.Drawing.Point(277, 60);
            this.RestrictionsCount.Name = "RestrictionsCount";
            this.RestrictionsCount.Size = new System.Drawing.Size(40, 27);
            this.RestrictionsCount.TabIndex = 20;
            this.RestrictionsCount.ValueChanged += new System.EventHandler(this.RestrictionsCount_ValueChanged);
            // 
            // dataGridViewTargetFunction
            // 
            this.dataGridViewTargetFunction.AllowUserToAddRows = false;
            this.dataGridViewTargetFunction.AllowUserToDeleteRows = false;
            this.dataGridViewTargetFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTargetFunction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewTargetFunction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTargetFunction.Location = new System.Drawing.Point(338, 30);
            this.dataGridViewTargetFunction.MaximumSize = new System.Drawing.Size(965, 49);
            this.dataGridViewTargetFunction.MinimumSize = new System.Drawing.Size(965, 49);
            this.dataGridViewTargetFunction.Name = "dataGridViewTargetFunction";
            this.dataGridViewTargetFunction.Size = new System.Drawing.Size(965, 49);
            this.dataGridViewTargetFunction.TabIndex = 19;
            this.dataGridViewTargetFunction.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTargetFunction_CellContentClick);
            this.dataGridViewTargetFunction.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewTargetFunction_EditingControlShowing);
            this.dataGridViewTargetFunction.SelectionChanged += new System.EventHandler(this.dataGridViewTargetFunction_SelectionChanged);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(142, 169);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 23;
            this.ClearButton.Text = "Очистить";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // LabelRestrictionsCount
            // 
            this.LabelRestrictionsCount.AutoSize = true;
            this.LabelRestrictionsCount.Font = new System.Drawing.Font("Montserrat ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelRestrictionsCount.Location = new System.Drawing.Point(12, 60);
            this.LabelRestrictionsCount.Name = "LabelRestrictionsCount";
            this.LabelRestrictionsCount.Size = new System.Drawing.Size(288, 26);
            this.LabelRestrictionsCount.TabIndex = 18;
            this.LabelRestrictionsCount.Text = "Количество ограничений:";
            // 
            // VariableCount
            // 
            this.VariableCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VariableCount.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VariableCount.Location = new System.Drawing.Point(277, 30);
            this.VariableCount.Name = "VariableCount";
            this.VariableCount.Size = new System.Drawing.Size(40, 27);
            this.VariableCount.TabIndex = 21;
            this.VariableCount.ValueChanged += new System.EventHandler(this.VariableCount_ValueChanged);
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(61, 169);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(75, 23);
            this.CalculateButton.TabIndex = 22;
            this.CalculateButton.Text = "Рассчитать";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // LabelVariableCount
            // 
            this.LabelVariableCount.AutoSize = true;
            this.LabelVariableCount.Font = new System.Drawing.Font("Montserrat ExtraBold", 14.25F, System.Drawing.FontStyle.Bold);
            this.LabelVariableCount.Location = new System.Drawing.Point(12, 30);
            this.LabelVariableCount.Name = "LabelVariableCount";
            this.LabelVariableCount.Size = new System.Drawing.Size(281, 26);
            this.LabelVariableCount.TabIndex = 17;
            this.LabelVariableCount.Text = "Количество переменных:";
            // 
            // Condition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1314, 470);
            this.Controls.Add(this.CBMethod);
            this.Controls.Add(this.NaturalRB);
            this.Controls.Add(this.DecimalRB);
            this.Controls.Add(this.dataGridViewRestrictions);
            this.Controls.Add(this.RestrictionsCount);
            this.Controls.Add(this.dataGridViewTargetFunction);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.LabelRestrictionsCount);
            this.Controls.Add(this.VariableCount);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.LabelVariableCount);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Condition";
            this.ShowIcon = false;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRestrictions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RestrictionsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTargetFunction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariableCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Label LabelRestrictionsCount;
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.Label LabelVariableCount;
        private System.Windows.Forms.ComboBox CBMethod;
        private System.Windows.Forms.RadioButton NaturalRB;
        private System.Windows.Forms.RadioButton DecimalRB;
        private System.Windows.Forms.DataGridView dataGridViewRestrictions;
        private System.Windows.Forms.DataGridView dataGridViewTargetFunction;
        private System.Windows.Forms.NumericUpDown RestrictionsCount;
        private System.Windows.Forms.NumericUpDown VariableCount;
    }
}
